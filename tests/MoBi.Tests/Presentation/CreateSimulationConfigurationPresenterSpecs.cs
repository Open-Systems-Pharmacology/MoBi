using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.IntegrationTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreateSimulationConfigurationPresenter : ContextForIntegration<CreateSimulationConfigurationPresenter>
   {
      protected ICreateSimulationConfigurationView _view;
      private IModelConstructor _modelConstructor;
      protected IUserSettings _userSettings;
      private ISimulationFactory _simulationFactory;
      protected ISubPresenterItemManager<ISimulationConfigurationItemPresenter> _subPresenterManager;
      private IDialogCreator _dialogCreator;
      protected IForbiddenNamesRetriever _forbiddenNameRetriever;
      protected IMoBiSimulation _simulation;
      protected SimulationConfiguration _simulationConfiguration;
      protected SimulationSettings _clonedSimulationSettings;
      protected IModuleConfigurationDTOToModuleConfigurationMapper _moduleConfigurationMapper;
      protected ISimulationConfigurationFactory _simulationConfigurationFactory;
      protected ICloneManagerForBuildingBlock _cloneManager;

      protected override void Context()
      {
         _view = A.Fake<ICreateSimulationConfigurationView>();
         _subPresenterManager = A.Fake<ISubPresenterItemManager<ISimulationConfigurationItemPresenter>>();
         _clonedSimulationSettings = new SimulationSettings();
         _moduleConfigurationMapper = A.Fake<IModuleConfigurationDTOToModuleConfigurationMapper>();

         _modelConstructor = A.Fake<IModelConstructor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _modelConstructor.CreateModelFrom(A<SimulationConfiguration>._, A<string>._)).Returns(A.Fake<CreationResult>());
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _simulationConfigurationFactory = A.Fake<ISimulationConfigurationFactory>();
         _userSettings = A.Fake<IUserSettings>();
         _userSettings.CheckCircularReference = true;
         _simulationFactory = A.Fake<ISimulationFactory>();
         _forbiddenNameRetriever = A.Fake<IForbiddenNamesRetriever>();
         sut = new CreateSimulationConfigurationPresenter(_view, _subPresenterManager, _dialogCreator,
            _forbiddenNameRetriever, _userSettings, _moduleConfigurationMapper, _simulationConfigurationFactory, _cloneManager);

         _simulation = new MoBiSimulation();
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         _simulationConfiguration = createBuildConfiguration();
         _simulation.Configuration = _simulationConfiguration;
      }

      private SimulationConfiguration createBuildConfiguration()
      {
         var simulationConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
         return simulationConfiguration;
      }
   }

   internal class When_cancelling_the_create_of_a_new_configuration : concern_for_CreateSimulationConfigurationPresenter
   {
      private SimulationConfiguration _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _result = sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void should_revert_commands()
      {
         _result.ShouldBeNull();
      }
   }

   public class configuring_an_existing_simulation_configuration : concern_for_CreateSimulationConfigurationPresenter
   {
      private SimulationConfiguration _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _cloneManager.Clone(_simulation.Settings)).Returns(_clonedSimulationSettings);
         A.CallTo(() => _view.Canceled).Returns(false);
         A.CallTo(() => _simulationConfigurationFactory.Create(_clonedSimulationSettings)).Returns(new SimulationConfiguration { SimulationSettings = _clonedSimulationSettings });
      }

      protected override void Because()
      {
         _result = sut.CreateBasedOn(_simulation, false);
      }

      [Observation]
      public void the_forbidden_names_must_not_be_initialized()
      {
         A.CallTo(() => _forbiddenNameRetriever.For(_simulation)).MustNotHaveHappened();
      }

      [Observation]
      public void the_view_must_disable_naming()
      {
         A.CallTo(() => _view.DisableNaming()).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_check_circular_reference_according_to_value_in_user_settings()
      {
         _simulationConfiguration.PerformCircularReferenceCheck.ShouldBeEqualTo(_userSettings.CheckCircularReference);
      }

      [Observation]
      public void the_simulation_settings_should_not_be_replaced()
      {
         _result.SimulationSettings.ShouldBeEqualTo(_clonedSimulationSettings);
      }
   }

   public class creating_a_new_simulation_configuration : concern_for_CreateSimulationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void the_forbidden_names_must_be_initialized()
      {
         A.CallTo(() => _forbiddenNameRetriever.For(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_check_circular_reference_according_to_value_in_user_settings()
      {
         _simulationConfiguration.PerformCircularReferenceCheck.ShouldBeEqualTo(_userSettings.CheckCircularReference);
      }
   }

   public class When_creating_configuration_and_overriding_molecule_calculation_methods : concern_for_CreateSimulationConfigurationPresenter
   {
      private SimulationConfiguration _result;
      private MoleculeBuilder _molecule;
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private IEditModuleConfigurationsPresenter _moduleConfigPresenter;
      private IEditIndividualAndExpressionConfigurationsPresenter _individualPresenter;
      private IEditMoleculeCalculationMethodsPresenter _calcMethodsPresenter;

      protected override void Context()
      {
         base.Context();

         _moduleConfigPresenter = A.Fake<IEditModuleConfigurationsPresenter>();
         _individualPresenter = A.Fake<IEditIndividualAndExpressionConfigurationsPresenter>();
         _calcMethodsPresenter = A.Fake<IEditMoleculeCalculationMethodsPresenter>();

         A.CallTo(() => _subPresenterManager.PresenterAt(SimulationItems.ModuleConfiguration)).Returns(_moduleConfigPresenter);
         A.CallTo(() => _subPresenterManager.PresenterAt(SimulationItems.IndividualAndExpressionConfiguration)).Returns(_individualPresenter);
         A.CallTo(() => _subPresenterManager.PresenterAt(SimulationItems.MoleculeCalculationMethodsConfiguration)).Returns(_calcMethodsPresenter);

         _molecule = new MoleculeBuilder().WithName("Drug");
         _molecule.AddUsedCalculationMethod(new UsedCalculationMethod("Absorption", "OriginalAbsorption"));
         _molecule.AddUsedCalculationMethod(new UsedCalculationMethod("Distribution", "OriginalDistribution"));

         _moleculeBuildingBlock = new MoleculeBuildingBlock { _molecule };
         var module = new Module { _moleculeBuildingBlock };
         var moduleConfig = new ModuleConfiguration(module);

         var moduleConfigDTO = new ModuleConfigurationDTO(moduleConfig);
         A.CallTo(() => _moduleConfigPresenter.ModuleConfigurationDTOs).Returns(new List<ModuleConfigurationDTO> { moduleConfigDTO });
         A.CallTo(() => _moduleConfigurationMapper.MapFrom(moduleConfigDTO)).Returns(moduleConfig);

         A.CallTo(() => _individualPresenter.ExpressionProfiles).Returns(new List<ExpressionProfileBuildingBlock>());
         A.CallTo(() => _individualPresenter.SelectedIndividual).Returns(null);


         var usedCalculationMethodList = new List<UsedCalculationMethod>{
            new UsedCalculationMethod("Absorption", "OverriddenAbsorption"),
            new UsedCalculationMethod("Distribution", "OverriddenDistribution")
         };

         A.CallTo(() => _calcMethodsPresenter.MoleculeNames).Returns(new List<string> { _molecule.Name });
         A.CallTo(() => _calcMethodsPresenter.AllUsedCalculationMethodsFor(_molecule.Name)).Returns(usedCalculationMethodList);
         A.CallTo(() => _simulationConfigurationFactory.Create(A<SimulationSettings>._)).Returns(new SimulationConfiguration());
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         _result = sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void should_update_absorption_calculation_method_to_overridden_value()
      {
         _result.CalculationMethodOverridesFor(_molecule.Name).UsedCalculationMethods.Single(x => x.Category == "Absorption").CalculationMethod.ShouldBeEqualTo("OverriddenAbsorption");
      }

      [Observation]
      public void should_update_distribution_calculation_method_to_overridden_value()
      {
         _result.CalculationMethodOverridesFor(_molecule.Name).UsedCalculationMethods.Single(x => x.Category == "Distribution").CalculationMethod.ShouldBeEqualTo("OverriddenDistribution");
      }

      [Observation]
      public void should_add_module_configuration_to_the_result()
      {
         _result.ModuleConfigurations.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_wizard_next_is_called_from_the_page_before_calculation_methods : concern_for_CreateSimulationConfigurationPresenter
   {
      private IEditModuleConfigurationsPresenter _moduleConfigPresenter;
      private IEditMoleculeCalculationMethodsPresenter _calcMethodsPresenter;
      private IReadOnlyList<ModuleConfigurationDTO> _moduleConfigurationDTOs;

      protected override void Context()
      {
         base.Context();

         _moduleConfigPresenter = A.Fake<IEditModuleConfigurationsPresenter>();
         _calcMethodsPresenter = A.Fake<IEditMoleculeCalculationMethodsPresenter>();

         A.CallTo(() => _subPresenterManager.PresenterAt(SimulationItems.ModuleConfiguration)).Returns(_moduleConfigPresenter);
         A.CallTo(() => _subPresenterManager.PresenterAt(SimulationItems.MoleculeCalculationMethodsConfiguration)).Returns(_calcMethodsPresenter);

         var module = new Module { new MoleculeBuildingBlock() };
         var moduleConfig = new ModuleConfiguration(module);
         _moduleConfigurationDTOs = new List<ModuleConfigurationDTO> { new ModuleConfigurationDTO(moduleConfig) };
         A.CallTo(() => _moduleConfigPresenter.ModuleConfigurationDTOs).Returns(_moduleConfigurationDTOs);
      }

      protected override void Because()
      {
         sut.WizardNext(SimulationItems.MoleculeCalculationMethodsConfiguration.Index - 1);
      }

      [Observation]
      public void should_refresh_the_calculation_methods_presenter_with_module_configuration_dtos()
      {
         A.CallTo(() => _calcMethodsPresenter.RefreshWith(_moduleConfigurationDTOs)).MustHaveHappened();
      }
   }
}