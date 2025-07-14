using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditContainerPresenter : ContextSpecification<EditContainerPresenter>
   {
      protected IEditContainerView _view;
      protected IContainerToContainerDTOMapper _containerMapper;
      protected IEditTaskForContainer _editTasks;
      protected IEditParametersInContainerPresenter _parametersInContainerPresenter;
      protected IMoBiContext _context;
      private ITagsPresenter _tagsPresenter;
      protected IApplicationController _applicationController;
      protected ICommandCollector _commandCollector;
      private IObjectPathFactory _objectPathFactory;
      protected IDialogCreator _dialogCreator;
      protected ISourceReferenceNavigator _sourceReferenceNavigator;

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _view = A.Fake<IEditContainerView>();
         _containerMapper = A.Fake<IContainerToContainerDTOMapper>();
         _editTasks = A.Fake<IEditTaskForContainer>();
         _parametersInContainerPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _context = A.Fake<IMoBiContext>();
         _tagsPresenter = A.Fake<ITagsPresenter>();
         _applicationController = A.Fake<IApplicationController>();
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         _sourceReferenceNavigator = A.Fake<ISourceReferenceNavigator>();
         sut = new EditContainerPresenter(_view, _containerMapper, _editTasks, _parametersInContainerPresenter, _context, _tagsPresenter, _applicationController, _objectPathFactory, _dialogCreator, _sourceReferenceNavigator);
         _commandCollector = new MoBiMacroCommand();
         sut.InitializeWith(_commandCollector);
      }
   }

   internal class When_a_parameter_of_the_edited_container_is_selected : concern_for_EditContainerPresenter
   {
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
      }

      protected override void Because()
      {
         sut.SelectParameter(_parameter);
      }

      [Observation]
      public void should_tell_view_to_show_parameters()
      {
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened();
      }

      [Observation]
      public void should_tell_parameter_presenter_to_select_parameter()
      {
         A.CallTo(() => _parametersInContainerPresenter.Select(_parameter)).MustHaveHappened();
      }

      [Observation]
      public void the_parameter_presenter_should_be_set_to_allow_changes_to_localisation()
      {
         _parametersInContainerPresenter.ChangeLocalisationAllowed.ShouldBeEqualTo(true);
      }
   }

   public class When_the_parent_path_of_a_container_is_being_set : concern_for_EditContainerPresenter
   {
      private string _parentPath;
      private IContainer _muscle;
      private SpatialStructure _spatialStructure;
      private NeighborhoodBuilder _neighborhoodReferencingContainer;
      private Container _anotherTopContainer;
      private Parameter _parameterInOtherContainerReferencingMuscle;
      private Parameter _parameterInContainerReferencingMuscle;
      private Parameter _parameterInNeighborhoodReferencingMuscle;
      private FormulaUsablePath _muscleInterstitialPath;
      private Parameter _parameterInContainerNotReferencingMuscle;

      protected override void Context()
      {
         base.Context();
         _parentPath = "NewPath";
         _muscleInterstitialPath = new FormulaUsablePath("A", "B", "Muscle", "Interstitial");
         _muscle = new Container { ParentPath = new ObjectPath("A", "B") }.WithName("Muscle");
         _neighborhoodReferencingContainer = new NeighborhoodBuilder
         {
            FirstNeighborPath = _muscleInterstitialPath,
            SecondNeighborPath = new ObjectPath("Organism", "Liver", "Interstitial"),
         };

         _anotherTopContainer = new Container().WithName("Another");
         _parameterInOtherContainerReferencingMuscle = new Parameter().WithName("ParameterOtherContainer").Under(_anotherTopContainer);
         _parameterInOtherContainerReferencingMuscle.Formula = new ExplicitFormula("A");
         _parameterInOtherContainerReferencingMuscle.Formula.AddObjectPath(_muscleInterstitialPath.Clone<FormulaUsablePath>().AndAdd("P"));

         _parameterInContainerReferencingMuscle = new Parameter().WithName("ParameterSubContainer").Under(_muscle);
         _parameterInContainerReferencingMuscle.Formula = new ExplicitFormula("A");
         _parameterInContainerReferencingMuscle.Formula.AddObjectPath(_muscleInterstitialPath.Clone<FormulaUsablePath>().AndAdd("P"));


         _parameterInContainerNotReferencingMuscle = new Parameter().WithName("AnotherParameterNotReferencing").Under(_muscle);
         _parameterInContainerNotReferencingMuscle.Formula = new ExplicitFormula("A");
         _parameterInContainerNotReferencingMuscle.Formula.AddObjectPath(new FormulaUsablePath("A", "B", "AnotherContainer").AndAdd("P"));


         _parameterInNeighborhoodReferencingMuscle = new Parameter().WithName("ParameterNeighborhood").Under(_neighborhoodReferencingContainer);
         _parameterInNeighborhoodReferencingMuscle.Formula = new ExplicitFormula("A");
         _parameterInNeighborhoodReferencingMuscle.Formula.AddObjectPath(_muscleInterstitialPath.Clone<FormulaUsablePath>().AndAdd("P"));


         _spatialStructure = new SpatialStructure
         {
            NeighborhoodsContainer = new Container()
         };
         _spatialStructure.AddTopContainer(_muscle);
         _spatialStructure.AddTopContainer(_anotherTopContainer);


         _spatialStructure.AddNeighborhood(_neighborhoodReferencingContainer);
         sut.Edit(_muscle);
         sut.BuildingBlock = _spatialStructure;
      }

      protected override void Because()
      {
         sut.SetParentPath(_parentPath);
      }

      [Observation]
      public void should_update_the_path_of_the_underlying_container()
      {
         _muscle.ParentPath.PathAsString.ShouldBeEqualTo(_parentPath);
      }

      [Observation]
      public void should_update_the_path_of_neighbors_in_neighborhoods_referencing_this_container()
      {
         _neighborhoodReferencingContainer.FirstNeighborPath.PathAsString.ShouldBeEqualTo("NewPath|Muscle|Interstitial");
      }

      [Observation]
      public void should_update_the_reference_to_this_container_in_all_formulas_of_the_spatial_structure()
      {
         var expectedPath = "NewPath|Muscle|Interstitial|P";

         _parameterInOtherContainerReferencingMuscle.Formula.ObjectPaths[0].PathAsString.ShouldBeEqualTo(expectedPath);
         _parameterInContainerReferencingMuscle.Formula.ObjectPaths[0].PathAsString.ShouldBeEqualTo(expectedPath);
         _parameterInNeighborhoodReferencingMuscle.Formula.ObjectPaths[0].PathAsString.ShouldBeEqualTo(expectedPath);
      }

      [Observation]
      public void should_not_update_references_of_formula_not_referencing_the_container()
      {
         _parameterInContainerNotReferencingMuscle.Formula.ObjectPaths[0].PathAsString.ShouldBeEqualTo("A|B|AnotherContainer|P");
      }

      [Observation]
      public void should_have_added_the_command_to_the_history()
      {
         _commandCollector.All().Any(x => x.IsAnImplementationOf<SetParentPathCommand>()).ShouldBeTrue();
      }
   }

   public class When_the_edit_container_presenter_is_updating_the_parent_path : concern_for_EditContainerPresenter
   {
      private IContainer _container;
      private SpatialStructure _buildingBlock;
      private ISelectContainerPresenter _selectContainerPresenter;
      private ObjectPath _excludedObjectPath;

      protected override void Context()
      {
         base.Context();
         _container = new Container { ParentPath = new ObjectPath("A", "B") };
         _buildingBlock = new SpatialStructure();
         _excludedObjectPath = new ObjectPath();
         sut.Edit(_container);
         sut.BuildingBlock = _buildingBlock;
         _selectContainerPresenter = A.Fake<ISelectContainerPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectContainerPresenter>()).Returns(_selectContainerPresenter);
         A.CallTo(() => _selectContainerPresenter.Select(_excludedObjectPath)).Returns(new ObjectPath("A", "B", "C"));
      }

      protected override void Because()
      {
         sut.UpdateParentPath();
      }

      [Observation]
      public void should_update_the_parent_path()
      {
         _container.ParentPath.PathAsString.ShouldBeEqualTo("A|B|C");
      }
   }

   public class When_the_edit_container_presenter_is_updating_the_parent_path_and_user_cancels : concern_for_EditContainerPresenter
   {
      private IContainer _container;
      private SpatialStructure _buildingBlock;
      private ISelectContainerPresenter _selectContainerPresenter;
      private ObjectPath _excludedObjectPath;

      protected override void Context()
      {
         base.Context();
         _container = new Container { ParentPath = new ObjectPath("A", "B") };
         _buildingBlock = new SpatialStructure();
         _excludedObjectPath = new ObjectPath();
         sut.Edit(_container);
         sut.BuildingBlock = _buildingBlock;
         _selectContainerPresenter = A.Fake<ISelectContainerPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectContainerPresenter>()).Returns(_selectContainerPresenter);
         A.CallTo(() => _selectContainerPresenter.Select(_excludedObjectPath)).Returns(null);
      }

      protected override void Because()
      {
         sut.UpdateParentPath();
      }

      [Observation]
      public void should_not_update_the_parent_path()
      {
         _container.ParentPath.PathAsString.ShouldBeEqualTo("A|B");
      }
   }

   internal class When_setting_container_mode_to_new_container : concern_for_EditContainerPresenter
   {
      private IContainer _muscle;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _muscle = new Container { ParentPath = new ObjectPath("A", "B") };
         _spatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container(),
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>()
         };
         _spatialStructure.AddTopContainer(_muscle);
         _muscle.Mode = ContainerMode.Physical;
         var moleculeProperties = _context.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);
         _muscle.Add(moleculeProperties);
         sut.Edit(_muscle);
         sut.BuildingBlock = _spatialStructure;
      }

      protected override void Because()
      {
         sut.ConfirmAndSetContainerMode(ContainerMode.Logical);
      }

      [Observation]
      public void should_change_mode()
      {
         _muscle.Mode.ShouldBeEqualTo(ContainerMode.Logical);
      }
   }

   internal class When_setting_container_mode_physical_to_existing_container_without_molecule_properties : concern_for_EditContainerPresenter
   {
      private IContainer _muscle;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _muscle = new Container { ParentPath = new ObjectPath("A", "B") };
         _spatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container(),
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>()
         };
         _spatialStructure.AddTopContainer(_muscle);
         _muscle.Mode = ContainerMode.Logical;

         sut.BuildingBlock = _spatialStructure;
         _muscle.Name = "Muscle";
         sut.Edit(_muscle);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>.Ignored, A<ViewResult>.Ignored)).Returns(ViewResult.Yes);
         A.CallTo(() => _editTasks.GetMoleculeProperties(_muscle)).Returns(null);
         A.CallTo(() => _editTasks.SetContainerMode(A<IBuildingBlock>.Ignored, _muscle, ContainerMode.Physical)).Returns(new SetContainerModeCommand(new MoBiSpatialStructure(), _muscle, ContainerMode.Logical));
      }

      protected override void Because()
      {
         sut.ConfirmAndSetContainerMode(ContainerMode.Physical);
      }

      [Observation]
      public void should_add_molecule_properties_when_not_present_already()
      {
         _commandCollector.All().Any(x => x.IsAnImplementationOf<MoBiMacroCommand>()).ShouldBeTrue();
         var macroCommand = _commandCollector.All().FirstOrDefault() as MoBiMacroCommand;
         macroCommand.All().Any(x => x.IsAnImplementationOf<AddContainerToSpatialStructureCommand>()).ShouldBeTrue();
      }

      [Observation]
      public void should_create_the_macro_command_for_changing_mode()
      {
         _commandCollector.All().Any(x => x.IsAnImplementationOf<MoBiMacroCommand>()).ShouldBeTrue();
         var macroCommand = (_commandCollector.All().FirstOrDefault() as MoBiMacroCommand);
         macroCommand.All().Any(x => x.IsAnImplementationOf<SetContainerModeCommand>()).ShouldBeTrue();
      }
   }

   internal class When_setting_container_mode_physical_to_existing_container_with_molecule_properties : concern_for_EditContainerPresenter
   {
      private IContainer _muscle;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _muscle = new Container { ParentPath = new ObjectPath("A", "B") };
         _spatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container(),
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>()
         };
         _spatialStructure.AddTopContainer(_muscle);
         _muscle.Mode = ContainerMode.Logical;
         var moleculeProperties = _context.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);
         _muscle.Add(moleculeProperties);
         sut.BuildingBlock = _spatialStructure;
         _muscle.Name = "Muscle";
         sut.Edit(_muscle);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>.Ignored, A<ViewResult>.Ignored)).Returns(ViewResult.Yes);
         A.CallTo(() => _editTasks.GetMoleculeProperties(_muscle)).Returns(moleculeProperties);
         A.CallTo(() => _editTasks.SetContainerMode(A<IBuildingBlock>.Ignored, _muscle, ContainerMode.Physical)).Returns(new SetContainerModeCommand(new MoBiSpatialStructure(), _muscle, ContainerMode.Logical));
      }

      protected override void Because()
      {
         sut.ConfirmAndSetContainerMode(ContainerMode.Physical);
      }

      [Observation]
      public void should_not_add_molecule_properties_when_present_already()
      {
         _commandCollector.All().Any(x => x.IsAnImplementationOf<MoBiMacroCommand>()).ShouldBeTrue();
         var macroCommand = _commandCollector.All().FirstOrDefault() as MoBiMacroCommand;
         macroCommand.All().Any(x => x.IsAnImplementationOf<AddContainerToSpatialStructureCommand>()).ShouldBeFalse();
      }

      [Observation]
      public void should_create_the_macro_command_for_changing_mode()
      {
         _commandCollector.All().Any(x => x.IsAnImplementationOf<MoBiMacroCommand>()).ShouldBeTrue();
         var macroCommand = (_commandCollector.All().FirstOrDefault() as MoBiMacroCommand);
         macroCommand.All().Any(x => x.IsAnImplementationOf<SetContainerModeCommand>()).ShouldBeTrue();
      }
   }

   internal class When_setting_container_mode_logical_to_existing_container : concern_for_EditContainerPresenter
   {
      private IContainer _muscle;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _muscle = new Container { ParentPath = new ObjectPath("A", "B") };
         _spatialStructure = new MoBiSpatialStructure
         {
            NeighborhoodsContainer = new Container(),
            DiagramManager = A.Fake<IDiagramManager<MoBiSpatialStructure>>()
         };
         _spatialStructure.AddTopContainer(_muscle);
         _muscle.Mode = ContainerMode.Physical;
         var moleculeProperties = _context.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical);
         _muscle.Add(moleculeProperties);
         sut.BuildingBlock = _spatialStructure;
         _muscle.Name = "Muscle";
         sut.Edit(_muscle);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>.Ignored, A<ViewResult>.Ignored)).Returns(ViewResult.Yes);
         A.CallTo(() => _editTasks.GetMoleculeProperties(_muscle)).Returns(moleculeProperties);
         A.CallTo(() => _editTasks.SetContainerMode(A<IBuildingBlock>.Ignored, _muscle,ContainerMode.Logical)).Returns(new SetContainerModeCommand(new MoBiSpatialStructure(),_muscle, ContainerMode.Logical));
         
      }

      protected override void Because()
      {
         sut.ConfirmAndSetContainerMode(ContainerMode.Logical);
      }

      [Observation]
      public void should_show_confirm_dialog()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>.Ignored, A<ViewResult>.Ignored)).MustHaveHappened();
      }

      [Observation]
      public void should_create_the_macro_command_for_changing_mode()
      {
         _commandCollector.All().Any(x => x.IsAnImplementationOf<MoBiMacroCommand>()).ShouldBeTrue();
         var macroCommand = (_commandCollector.All().FirstOrDefault() as MoBiMacroCommand);
         macroCommand.All().Any(x => x.IsAnImplementationOf<SetContainerModeCommand>()).ShouldBeTrue();
         macroCommand.All().Any(x => x.IsAnImplementationOf<RemoveContainerFromSpatialStructureCommand>()).ShouldBeTrue();
      }
   }

   public class When_enabling_individual_preview : concern_for_EditContainerPresenter
   {
      protected override void Because()
      {
         sut.EnableIndividualPreview();
      }

      [Observation]
      public void the_sub_presenter_should_be_called_to_enable_preview()
      {
         A.CallTo(() => _parametersInContainerPresenter.ShowIndividualSelection()).MustHaveHappened();
      }
   }

   public class When_navigating_to_a_source_of_container : concern_for_EditContainerPresenter
   {
      private TrackableSimulation _trackableSimulation;
      private IContainer _container;
      private SimulationEntitySourceReference _sourceRef;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         _trackableSimulation = new TrackableSimulation(null, new SimulationEntitySourceReferenceCache());
         _sourceRef = new SimulationEntitySourceReference(null, null, null, _container);
         _trackableSimulation.ReferenceCache.Add(_container, _sourceRef);
         sut.EnableSimulationTracking(_trackableSimulation);
         A.CallTo(() => _containerMapper.MapFrom(_container, _trackableSimulation)).Returns(new ContainerDTO(_container) { SourceReference = _sourceRef });
         sut.Edit(_container);
      }

      protected override void Because()
      {
         sut.NavigateToSource();
      }

      [Observation]
      public void the_navigator_should_be_used()
      {
         A.CallTo(() => _sourceReferenceNavigator.GoTo(_sourceRef)).MustHaveHappened();
      }
   }

   public class When_editing_a_container_within_tracking_enabled : concern_for_EditContainerPresenter
   {
      private TrackableSimulation _trackableSimulation;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container();
         _trackableSimulation = new TrackableSimulation(null, new SimulationEntitySourceReferenceCache());
         sut.EnableSimulationTracking(_trackableSimulation);
      }

      protected override void Because()
      {
         sut.Edit(_container);
      }

      [Observation]
      public void the_mapper_must_include_tracking_in_dto()
      {
         A.CallTo(() => _containerMapper.MapFrom(_container, _trackableSimulation)).MustHaveHappened();
      }
   }

   public class When_enabling_simulation_tracking : concern_for_EditContainerPresenter
   {
      private TrackableSimulation _trackableSimulation;

      protected override void Context()
      {
         base.Context();
         _trackableSimulation = new TrackableSimulation(null, new SimulationEntitySourceReferenceCache());
      }

      protected override void Because()
      {
         sut.EnableSimulationTracking(_trackableSimulation);
      }

      [Observation]
      public void the_parameters_presenter_must_also_enable_tracking()
      {
         A.CallTo(() => _parametersInContainerPresenter.EnableSimulationTracking(_trackableSimulation)).MustHaveHappened();
      }
   }
}