using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditContainerPresenter : ContextSpecification<IEditContainerPresenter>
   {
      protected IEditContainerView _view;
      private IContainerToContainerDTOMapper _containerMapper;
      private IEditTaskForContainer _editTasks;
      protected IEditParametersInContainerPresenter _parametersInContainerPresenter;
      private IMoBiContext _context;
      private ITagsPresenter _tagsPresenter;
      protected IApplicationController _applicationController;
      protected ICommandCollector _commandCollector;

      protected override void Context()
      {
         _view = A.Fake<IEditContainerView>();
         _containerMapper = A.Fake<IContainerToContainerDTOMapper>();
         _editTasks = A.Fake<IEditTaskForContainer>();
         _parametersInContainerPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _context = A.Fake<IMoBiContext>();
         _tagsPresenter = A.Fake<ITagsPresenter>();
         _applicationController = A.Fake<IApplicationController>();
         sut = new EditContainerPresenter(_view, _containerMapper, _editTasks, _parametersInContainerPresenter, _context, _tagsPresenter, _applicationController);
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
         _muscle = new Container {ParentPath = new ObjectPath("A", "B")}.WithName("Muscle");
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

      protected override void Context()
      {
         base.Context();
         _container = new Container {ParentPath = new ObjectPath("A", "B")};
         _buildingBlock = new SpatialStructure();
         sut.Edit(_container);
         sut.BuildingBlock = _buildingBlock;
         _selectContainerPresenter = A.Fake<ISelectContainerPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectContainerPresenter>()).Returns(_selectContainerPresenter);
      }

      [Observation]
      public void should_update_the_parent_path_if_a_selection_was_made()
      {
         A.CallTo(() => _selectContainerPresenter.Select()).Returns(new ObjectPath("A", "B", "C"));
         sut.UpdateParentPath();
         _container.ParentPath.PathAsString.ShouldBeEqualTo("A|B|C");
      }

      [Observation]
      public void should_not_update_the_parent_path_if_the_user_canceled_the_action()
      {
         A.CallTo(() => _selectContainerPresenter.Select()).Returns(null);
         sut.UpdateParentPath();
         _container.ParentPath.PathAsString.ShouldBeEqualTo("A|B");
      }
   }
}