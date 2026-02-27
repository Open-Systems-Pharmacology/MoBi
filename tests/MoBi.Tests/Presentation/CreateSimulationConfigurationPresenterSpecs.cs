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
using OSPSuite.Utility.Collections;

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

         var overrides = new Cache<string, IReadOnlyList<UsedCalculationMethod>>
         {
            ["Drug"] = new List<UsedCalculationMethod>
            {
               new UsedCalculationMethod("Absorption", "OverriddenAbsorption"),
               new UsedCalculationMethod("Distribution", "OverriddenDistribution")
            }
         };
         A.CallTo(() => _calcMethodsPresenter.CalculationMethodOverrides).Returns(overrides);

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
         _molecule.UsedCalculationMethods.Single(x => x.Category == "Absorption").CalculationMethod.ShouldBeEqualTo("OverriddenAbsorption");
      }

      [Observation]
      public void should_update_distribution_calculation_method_to_overridden_value()
      {
         _molecule.UsedCalculationMethods.Single(x => x.Category == "Distribution").CalculationMethod.ShouldBeEqualTo("OverriddenDistribution");
      }

      [Observation]
      public void should_add_module_configuration_to_the_result()
      {
         _result.ModuleConfigurations.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_creating_configuration_with_multiple_modules_and_calculation_method_overrides : concern_for_CreateSimulationConfigurationPresenter
   {
      private MoleculeBuilder _moleculeInModule1;
      private MoleculeBuilder _moleculeInModule2;
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

         // Module 1 with a molecule
         _moleculeInModule1 = new MoleculeBuilder().WithName("DrugA");
         _moleculeInModule1.AddUsedCalculationMethod(new UsedCalculationMethod("Absorption", "OldAbsorption"));

         var moleculeBB1 = new MoleculeBuildingBlock { _moleculeInModule1 };
         var module1 = new Module { moleculeBB1 };
         var moduleConfig1 = new ModuleConfiguration(module1);

         // Module 2 with a different molecule
         _moleculeInModule2 = new MoleculeBuilder().WithName("DrugB");
         _moleculeInModule2.AddUsedCalculationMethod(new UsedCalculationMethod("Elimination", "OldElimination"));

         var moleculeBB2 = new MoleculeBuildingBlock { _moleculeInModule2 };
         var module2 = new Module { moleculeBB2 };
         var moduleConfig2 = new ModuleConfiguration(module2);

         var dto1 = new ModuleConfigurationDTO(moduleConfig1);
         var dto2 = new ModuleConfigurationDTO(moduleConfig2);
         A.CallTo(() => _moduleConfigPresenter.ModuleConfigurationDTOs).Returns(new List<ModuleConfigurationDTO> { dto1, dto2 });
         A.CallTo(() => _moduleConfigurationMapper.MapFrom(dto1)).Returns(moduleConfig1);
         A.CallTo(() => _moduleConfigurationMapper.MapFrom(dto2)).Returns(moduleConfig2);

         A.CallTo(() => _individualPresenter.ExpressionProfiles).Returns(new List<ExpressionProfileBuildingBlock>());
         A.CallTo(() => _individualPresenter.SelectedIndividual).Returns(null);

         var overrides = new Cache<string, IReadOnlyList<UsedCalculationMethod>>
         {
            ["DrugA"] = new List<UsedCalculationMethod> { new UsedCalculationMethod("Absorption", "NewAbsorption") },
            ["DrugB"] = new List<UsedCalculationMethod> { new UsedCalculationMethod("Elimination", "NewElimination") }
         };
         A.CallTo(() => _calcMethodsPresenter.CalculationMethodOverrides).Returns(overrides);

         A.CallTo(() => _simulationConfigurationFactory.Create(A<SimulationSettings>._)).Returns(new SimulationConfiguration());
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void should_update_calculation_method_in_first_module()
      {
         _moleculeInModule1.UsedCalculationMethods.Single(x => x.Category == "Absorption").CalculationMethod.ShouldBeEqualTo("NewAbsorption");
      }

      [Observation]
      public void should_update_calculation_method_in_second_module()
      {
         _moleculeInModule2.UsedCalculationMethods.Single(x => x.Category == "Elimination").CalculationMethod.ShouldBeEqualTo("NewElimination");
      }
   }

   public class When_creating_configuration_with_overrides_missing_a_molecule : concern_for_CreateSimulationConfigurationPresenter
   {
      private MoleculeBuilder _moleculeWithOverride;
      private MoleculeBuilder _moleculeWithoutOverride;
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

         _moleculeWithOverride = new MoleculeBuilder().WithName("DrugA");
         _moleculeWithOverride.AddUsedCalculationMethod(new UsedCalculationMethod("Absorption", "OldAbsorption"));

         _moleculeWithoutOverride = new MoleculeBuilder().WithName("DrugB");
         _moleculeWithoutOverride.AddUsedCalculationMethod(new UsedCalculationMethod("Elimination", "OriginalElimination"));

         var moleculeBuildingBlock = new MoleculeBuildingBlock { _moleculeWithOverride, _moleculeWithoutOverride };
         var module = new Module { moleculeBuildingBlock };
         var moduleConfig = new ModuleConfiguration(module);

         var moduleConfigDTO = new ModuleConfigurationDTO(moduleConfig);
         A.CallTo(() => _moduleConfigPresenter.ModuleConfigurationDTOs).Returns(new List<ModuleConfigurationDTO> { moduleConfigDTO });
         A.CallTo(() => _moduleConfigurationMapper.MapFrom(moduleConfigDTO)).Returns(moduleConfig);

         A.CallTo(() => _individualPresenter.ExpressionProfiles).Returns(new List<ExpressionProfileBuildingBlock>());
         A.CallTo(() => _individualPresenter.SelectedIndividual).Returns(null);

         // Only DrugA has an override — DrugB is intentionally missing
         var overrides = new Cache<string, IReadOnlyList<UsedCalculationMethod>>
         {
            ["DrugA"] = new List<UsedCalculationMethod> { new UsedCalculationMethod("Absorption", "NewAbsorption") }
         };
         A.CallTo(() => _calcMethodsPresenter.CalculationMethodOverrides).Returns(overrides);

         A.CallTo(() => _simulationConfigurationFactory.Create(A<SimulationSettings>._)).Returns(new SimulationConfiguration());
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         sut.CreateBasedOn(_simulation);
      }

      [Observation]
      public void should_update_the_molecule_that_has_an_override()
      {
         _moleculeWithOverride.UsedCalculationMethods.Single(x => x.Category == "Absorption").CalculationMethod.ShouldBeEqualTo("NewAbsorption");
      }

      [Observation]
      public void should_leave_the_molecule_without_an_override_unchanged()
      {
         _moleculeWithoutOverride.UsedCalculationMethods.Single(x => x.Category == "Elimination").CalculationMethod.ShouldBeEqualTo("OriginalElimination");
      }
   }
}