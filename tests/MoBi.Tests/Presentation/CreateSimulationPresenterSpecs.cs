using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
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
      protected IEditBuildConfigurationPresenter _buildConfigurationPresenter;
      protected ISelectAndEditMoleculesStartValuesPresenter _moleculeStartValuesPresenter;
      protected ISelectAndEditParameterStartValuesPresenter _parameterStartValuesPresenter;
      protected IFinalOptionsPresenter _finalActionPresenter;
      protected IMoBiContext _context;
      protected IModelConstructor _modelConstructor;
      protected IDimensionValidator _validationVisitor;
      protected IUserSettings _userSettings;
      protected ISimulationFactory _simulationFactory;
      protected IMoBiApplicationController _applicationController;
      protected IBuildConfigurationFactory _buildConfigurationFactory;
      protected IHeavyWorkManager _heavyWorkManager;
      private ISubPresenterItemManager<ISimulationItemPresenter> _subPresenterManager;
      private IDialogCreator _dialogCreator;
      private IForbiddenNamesRetriever _forbiddenNameRetriever;
      protected IMoBiSimulation _simulation;
      protected string _templateId = "Template";
      protected MoBiBuildConfiguration _buildConfiguration;
      protected const string _useId = "ToUse";

      protected override void Context()
      {
         _view = A.Fake<ICreateSimulationView>();
         _subPresenterManager = A.Fake<ISubPresenterItemManager<ISimulationItemPresenter>>();
         _buildConfigurationPresenter = _subPresenterManager.CreateFake(SimulationItems.BuildConfiguration);
         _moleculeStartValuesPresenter = _subPresenterManager.CreateFake(SimulationItems.MoleculeStartValues);
         _parameterStartValuesPresenter = _subPresenterManager.CreateFake(SimulationItems.ParameterStartValues);
         _finalActionPresenter = _subPresenterManager.CreateFake(SimulationItems.FinalOptions);

         A.CallTo(() => _subPresenterManager.AllSubPresenters).Returns(new ISimulationItemPresenter[] {_buildConfigurationPresenter, _moleculeStartValuesPresenter, _parameterStartValuesPresenter, _finalActionPresenter});
         _context = A.Fake<IMoBiContext>();
         _modelConstructor = A.Fake<IModelConstructor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _modelConstructor.CreateModelFrom(A<IBuildConfiguration>._, A<string>._)).Returns(A.Fake<CreationResult>());


         _validationVisitor = A.Fake<IDimensionValidator>();
         _userSettings = A.Fake<IUserSettings>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         _buildConfigurationFactory = A.Fake<IBuildConfigurationFactory>();
         _heavyWorkManager = new HeavyWorkManagerForSpecs();
         _forbiddenNameRetriever = A.Fake<IForbiddenNamesRetriever>();
         sut = new CreateSimulationPresenter(_view, _context, _modelConstructor, _validationVisitor,
            _simulationFactory, _buildConfigurationFactory, _heavyWorkManager, _subPresenterManager, _dialogCreator, 
            _forbiddenNameRetriever);

         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         _buildConfiguration = createBuildConfiguration();
         A.CallTo(() => _simulation.MoBiBuildConfiguration).Returns(_buildConfiguration);
         A.CallTo(() => _moleculeStartValuesPresenter.StartValues).Returns(A.Fake<IMoleculeStartValuesBuildingBlock>().WithId(_useId));
         A.CallTo(() => _parameterStartValuesPresenter.StartValues).Returns(A.Fake<IParameterStartValuesBuildingBlock>().WithId(_useId));
      }

      private MoBiBuildConfiguration createBuildConfiguration()
      {
         var moBiBuildConfiguration = new MoBiBuildConfiguration();
         setBuildingBlockInfo(moBiBuildConfiguration.MoleculeStartValuesInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.MoleculesInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.ParameterStartValuesInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.PassiveTransportsInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.ReactionsInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.SimulationSettingsInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.SpatialStructureInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.ObserversInfo);
         setBuildingBlockInfo(moBiBuildConfiguration.EventGroupsInfo);
         return moBiBuildConfiguration;
      }

      private void setBuildingBlockInfo<T>(BuildingBlockInfo<T> info) where T : class, IBuildingBlock
      {
         info.TemplateBuildingBlock = A.Fake<T>().WithId(_templateId);
         info.BuildingBlock = A.Fake<T>().WithId(ShortGuid.NewGuid());
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
         A.CallTo(() => _buildConfigurationFactory.CreateFromReferencesUsedIn(_buildConfiguration,null)).Returns(_newBuildConfiguration);
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
         A.CallTo(() => _buildConfigurationPresenter.Edit(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_create_model()
      {
         A.CallTo(() => _modelConstructor.CreateModelFrom(_newBuildConfiguration, A<string>._)).MustHaveHappened();
      }


      [Observation]
      public void should_generate_a_correct_buildconfiguration()
      {
         checkInfo(_buildConfiguration.MoleculeStartValuesInfo);
         checkInfo(_buildConfiguration.ParameterStartValuesInfo);
      }

      private void checkInfo<T>(BuildingBlockInfo<T> info) where T : class, IBuildingBlock
      {
         info.TemplateBuildingBlockId.ShouldBeEqualTo(_templateId);
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
         A.CallTo(() => _simulationFactory.CreateFrom(A<IMoBiBuildConfiguration>._, A<IModel>._)).Returns(_simulation);
      }

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void should_mark_start_value_building_block_as_changed()
      {
         var buildConfiguration = _result.MoBiBuildConfiguration;
         buildConfiguration.HasChangedBuildingBlocks().ShouldBeTrue();
         buildConfiguration.MoleculeStartValuesInfo.SimulationHasChanged.ShouldBeTrue();
         buildConfiguration.ParameterStartValuesInfo.SimulationHasChanged.ShouldBeTrue();
      }
   }

   public class When_the_selected_spatial_structure_is_being_changed : concern_for_CreateSimulationPresenter
   {
      protected override void Because()
      {
         _buildConfigurationPresenter.SpatialStructureChangedEvent += Raise.WithEmpty();
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
         _buildConfigurationPresenter.MoleculeBuildingBlockChangedEvent += Raise.WithEmpty();
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