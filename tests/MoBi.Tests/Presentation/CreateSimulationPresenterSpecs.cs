using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Helpers;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_CreateSimulationPresenter : ContextSpecification<ICreateSimulationPresenter>
   {
      protected ICreateSimulationView _view;
      protected IEditSimulationConfigurationPresenter _simulationConfigurationPresenter;
      protected ISelectAndEditMoleculesStartValuesPresenter _moleculeStartValuesPresenter;
      protected ISelectAndEditParameterStartValuesPresenter _parameterStartValuesPresenter;
      protected IFinalOptionsPresenter _finalActionPresenter;
      protected IMoBiContext _context;
      protected IModelConstructor _modelConstructor;
      protected IDimensionValidator _validationVisitor;
      protected IUserSettings _userSettings;
      protected ISimulationFactory _simulationFactory;
      protected IMoBiApplicationController _applicationController;
      protected IHeavyWorkManager _heavyWorkManager;
      private ISubPresenterItemManager<ISimulationItemPresenter> _subPresenterManager;
      private IDialogCreator _dialogCreator;
      private IForbiddenNamesRetriever _forbiddenNameRetriever;
      protected IMoBiSimulation _simulation;
      protected string _templateId = "Template";
      protected SimulationConfiguration _buildConfiguration;
      protected const string _useId = "ToUse";

      protected override void Context()
      {
         _view = A.Fake<ICreateSimulationView>();
         _subPresenterManager = A.Fake<ISubPresenterItemManager<ISimulationItemPresenter>>();
         _simulationConfigurationPresenter = _subPresenterManager.CreateFake(SimulationItems.BuildConfiguration);
         _moleculeStartValuesPresenter = _subPresenterManager.CreateFake(SimulationItems.MoleculeStartValues);
         _parameterStartValuesPresenter = _subPresenterManager.CreateFake(SimulationItems.ParameterStartValues);
         _finalActionPresenter = _subPresenterManager.CreateFake(SimulationItems.FinalOptions);

         A.CallTo(() => _subPresenterManager.AllSubPresenters).Returns(new ISimulationItemPresenter[] { _simulationConfigurationPresenter, _moleculeStartValuesPresenter, _parameterStartValuesPresenter, _finalActionPresenter });
         _context = A.Fake<IMoBiContext>();
         _modelConstructor = A.Fake<IModelConstructor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _modelConstructor.CreateModelFrom(A<SimulationConfiguration>._, A<string>._)).Returns(A.Fake<CreationResult>());


         _validationVisitor = A.Fake<IDimensionValidator>();
         _userSettings = A.Fake<IUserSettings>();
         _userSettings.CheckCircularReference = true;
         _simulationFactory = A.Fake<ISimulationFactory>();
         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         _forbiddenNameRetriever = A.Fake<IForbiddenNamesRetriever>();
         sut = new CreateSimulationPresenter(_view, _context, _modelConstructor, _validationVisitor,
            _simulationFactory, _heavyWorkManager, _subPresenterManager, _dialogCreator,
            _forbiddenNameRetriever, _userSettings);

         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         _buildConfiguration = createBuildConfiguration();
         A.CallTo(() => _simulation.Configuration).Returns(_buildConfiguration);
         A.CallTo(() => _moleculeStartValuesPresenter.StartValues).Returns(A.Fake<MoleculeStartValuesBuildingBlock>().WithId(_useId));
         A.CallTo(() => _parameterStartValuesPresenter.StartValues).Returns(A.Fake<ParameterStartValuesBuildingBlock>().WithId(_useId));
      }

      private SimulationConfiguration createBuildConfiguration()
      {
         return DomainFactoryForSpecs.CreateDefaultConfiguration();
      }
   }

   internal class When_finishing_creation_of_a_simulation : concern_for_CreateSimulationPresenter
   {
      private IMoBiSimulation _result;

      protected override void Because()
      {
         _result = sut.Create();
      }

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      [Observation]
      public void should_revert_commands()
      {
         _result.ShouldBeNull();
      }
   }

   internal class When_creating_a_new_simulation : concern_for_CreateSimulationPresenter
   {
      private IMoBiBuildConfiguration _newBuildConfiguration;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         _newBuildConfiguration = A.Fake<IMoBiBuildConfiguration>();
      }

      protected override void Because()
      {
         sut.Create();
      }

      [Observation]
      public void should_ask_simulation_factory_for_new_simulation()
      {
         A.CallTo(() => _simulationFactory.Create()).MustHaveHappened();
      }

      [Observation]
      public void should_initialise_sub_edit_presenter()
      {
         A.CallTo(() => _simulationConfigurationPresenter.Edit(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_create_model()
      {
         // A.CallTo(() => _modelConstructor.CreateModelFrom(_newBuildConfiguration, A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_check_circular_reference_according_to_value_in_user_settings()
      {
         _buildConfiguration.PerformCircularReferenceCheck.ShouldBeEqualTo(_userSettings.CheckCircularReference);
      }

      [Observation]
      public void should_generate_a_correct_build_configuration()
      {
         checkInfo(_buildConfiguration.MoleculeStartValues);
         checkInfo(_buildConfiguration.ParameterStartValues);
      }

      private void checkInfo<T>(T info) where T : class, IBuildingBlock
      {
         info.Id.ShouldBeEqualTo(_templateId);
      }
   }

   internal class When_used_start_values_where_changed_in_creation_process : concern_for_CreateSimulationPresenter
   {
      private IMoBiSimulation _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
         _moleculeStartValuesPresenter.StartValues.Version = 2;
         _parameterStartValuesPresenter.StartValues.Version = 1;
         A.CallTo(() => _simulationFactory.CreateFrom(A<SimulationConfiguration>._, A<IModel>._)).Returns(_simulation);
      }

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void should_mark_start_value_building_block_as_changed()
      {
         Assert.False(true);
         // var buildConfiguration = _result.Configuration;
         // buildConfiguration.HasChangedBuildingBlocks().ShouldBeTrue();
         // buildConfiguration.MoleculeStartValuesInfo.SimulationHasChanged.ShouldBeTrue();
         // buildConfiguration.ParameterStartValuesInfo.SimulationHasChanged.ShouldBeTrue();
      }
   }

   public class When_the_selected_spatial_structure_is_being_changed : concern_for_CreateSimulationPresenter
   {
      protected override void Because()
      {
         // _simulationConfigurationPresenter.SpatialStructureChangedEvent += Raise.WithEmpty();
      }

      [Observation]
      public void should_refresh_the_molecule_start_values()
      {
         A.CallTo(() => _moleculeStartValuesPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_parameter_start_values()
      {
         A.CallTo(() => _parameterStartValuesPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_selected_molecule_building_block_is_being_changed : concern_for_CreateSimulationPresenter
   {
      protected override void Because()
      {
         // _simulationConfigurationPresenter.MoleculeBuildingBlockChangedEvent += Raise.WithEmpty();
      }

      [Observation]
      public void should_refresh_the_molecule_start_values()
      {
         A.CallTo(() => _moleculeStartValuesPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_parameter_start_values()
      {
         A.CallTo(() => _parameterStartValuesPresenter.Refresh()).MustHaveHappened();
      }
   }
}