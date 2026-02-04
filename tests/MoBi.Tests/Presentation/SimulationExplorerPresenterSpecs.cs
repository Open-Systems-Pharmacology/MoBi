using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation
{
   public abstract class concern_for_SimulationExplorerPresenter : ContextSpecification<SimulationExplorerPresenter>
   {
      protected ISimulationExplorerView _view;
      protected IRegionResolver _regionResolver;
      protected ITreeNodeFactory _treeNodeFactory;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IMoBiContext _context;
      protected IClassificationPresenter _classificationPresenter;
      protected IToolTipPartCreator _toolTipPartCreator;
      protected IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      protected IProjectRetriever _projectRetriever;
      protected IInteractionTasksForSimulation _interactionTasksForSimulation;
      protected IParameterAnalysablesInExplorerPresenter _parameterAnalysablesPresenter;
      protected IUxTreeView _treeView;

      protected IMoBiSimulation _simulation;
      protected ITreeNode _oldNode;
      protected ITreeNode _newNode;
      protected ITreeNode<IClassification> _parentClassificationNode;
      protected MoBiProject _fakeProject;

      protected override void Context()
      {
         _view = A.Fake<ISimulationExplorerView>();
         _regionResolver = A.Fake<IRegionResolver>();
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _context = A.Fake<IMoBiContext>();
         _classificationPresenter = A.Fake<IClassificationPresenter>();
         _toolTipPartCreator = A.Fake<IToolTipPartCreator>();
         _multipleTreeNodeContextMenuFactory = A.Fake<IMultipleTreeNodeContextMenuFactory>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _interactionTasksForSimulation = A.Fake<IInteractionTasksForSimulation>();
         _parameterAnalysablesPresenter = A.Fake<IParameterAnalysablesInExplorerPresenter>();

         _treeView = A.Fake<IUxTreeView>();
         A.CallTo(() => _view.TreeView).Returns(_treeView);

         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.Id).Returns("sim-1");

         _oldNode = A.Fake<ITreeNode>();
         A.CallTo(() => _oldNode.Id).Returns(_simulation.Id);

         _newNode = A.Fake<ITreeNode>();
         A.CallTo(() => _newNode.Id).Returns(_simulation.Id); // recreated node shares the same id

         _parentClassificationNode = A.Fake<ITreeNode<IClassification>>();
         A.CallTo(() => _oldNode.ParentNode).Returns(_parentClassificationNode);
         A.CallTo(() => _treeView.NodeById(_simulation.Id)).Returns(_oldNode);
         A.CallTo(() => _treeNodeFactory.CreateFor(A<ClassifiableSimulation>._)).Returns(_newNode);

         _fakeProject = new MoBiProject();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_fakeProject);

         sut = new SimulationExplorerPresenter(
            _view,
            _regionResolver,
            _treeNodeFactory,
            _viewItemContextMenuFactory,
            _context,
            _classificationPresenter,
            _toolTipPartCreator,
            _multipleTreeNodeContextMenuFactory,
            _projectRetriever,
            _interactionTasksForSimulation,
            _parameterAnalysablesPresenter);
      }
   }

   public class When_recreating_a_selected_simulation_node : concern_for_SimulationExplorerPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _treeView.IsNodeExpanded(_oldNode)).Returns(true);
         A.CallTo(() => _treeView.SelectedNode).Returns(_oldNode);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationReloadEvent(_simulation));
      }

      [Observation]
      public void should_select_the_recreated_node()
      {
         A.CallTo(() => _treeView.SelectNode(_newNode)).MustHaveHappened();
      }
   }

   public class When_recreating_a_simulation_node_that_was_not_selected : concern_for_SimulationExplorerPresenter
   {
      protected override void Context()
      {
         base.Context();

         var someOtherNode = A.Fake<ITreeNode>();
         A.CallTo(() => someOtherNode.Id).Returns("other");
         A.CallTo(() => _treeView.SelectedNode).Returns(someOtherNode);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationReloadEvent(_simulation));
      }

      [Observation]
      public void should_not_change_selection()
      {
         A.CallTo(() => _treeView.SelectNode(_newNode)).MustNotHaveHappened();
      }
   }
   public class When_simulation_run_finishes_for_selected_node : concern_for_SimulationExplorerPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _treeView.IsNodeExpanded(_oldNode)).Returns(true);
         A.CallTo(() => _treeView.SelectedNode).Returns(_oldNode);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRunFinishedEvent(_simulation, true));
      }

      [Observation]
      public void should_keep_selection_on_the_same_simulation()
      {
         A.CallTo(() => _treeView.SelectNode(_newNode)).MustHaveHappened();
      }
   }

   public class When_simulation_run_finishes_for_a_node_that_was_not_selected : concern_for_SimulationExplorerPresenter
   {
      protected override void Context()
      {
         base.Context();

         var other = A.Fake<ITreeNode>();
         A.CallTo(() => other.Id).Returns("other");
         A.CallTo(() => _treeView.SelectedNode).Returns(other);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRunFinishedEvent(_simulation, false));
      }

      [Observation]
      public void should_not_change_the_selection()
      {
         A.CallTo(() => _treeView.SelectNode(_newNode)).MustNotHaveHappened();
      }
   }
}