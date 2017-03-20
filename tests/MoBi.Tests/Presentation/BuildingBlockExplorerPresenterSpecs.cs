using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation
{
   public abstract class concern_for_BuildingBlockExplorerPresenter : ContextSpecification<IBuildingBlockExplorerPresenter>
   {
      protected IBuildingBlockExplorerView _view;
      protected IMoBiContext _context;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IRegionResolver _regionResolver;
      private ITreeNodeFactory _treeNodeFactory;
      private IClassificationPresenter _classificationPresenter;
      private IToolTipPartCreator _toolTipPartCreator;
      private IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      private IProjectRetriever _projectRetriever;
      protected IEditBuildingBlockStarter _editBuildingBlockStarter;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _view = A.Fake<IBuildingBlockExplorerView>();
         A.CallTo(() => _view.TreeView).Returns(A.Fake<IUxTreeView>());
         _context = A.Fake<IMoBiContext>();
         _regionResolver = A.Fake<IRegionResolver>();
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _classificationPresenter = A.Fake<IClassificationPresenter>();
         _toolTipPartCreator = A.Fake<IToolTipPartCreator>();
         _observedDataInExplorerPresenter = A.Fake<IObservedDataInExplorerPresenter>();
         _multipleTreeNodeContextMenuFactory = A.Fake<IMultipleTreeNodeContextMenuFactory>();
         _editBuildingBlockStarter = A.Fake<IEditBuildingBlockStarter>();
         sut = new BuildingBlockExplorerPresenter(_view, _regionResolver, _treeNodeFactory, _viewItemContextMenuFactory, _context,
            _classificationPresenter, _toolTipPartCreator, _observedDataInExplorerPresenter, _multipleTreeNodeContextMenuFactory, _projectRetriever, _editBuildingBlockStarter);
      }
   }

   public class When_handling_a_CloseProjectEvent : concern_for_BuildingBlockExplorerPresenter
   {
      private readonly ProjectClosedEvent _closeProjectEvent = new ProjectClosedEvent();

      protected override void Because()
      {
         sut.Handle(_closeProjectEvent);
      }

      [Observation]
      public void should_tell_view_to_clear()
      {
         A.CallTo(() => _view.TreeView.DestroyAllNodes()).MustHaveHappened();
      }
   }

   public class When_told_to_show_a_context_menu : concern_for_BuildingBlockExplorerPresenter
   {
      private IViewItem _viewItem;
      private Point _point;
      private IContextMenu _menu;

      protected override void Because()
      {
         sut.ShowContextMenu(_viewItem, _point);
      }

      protected override void Context()
      {
         base.Context();
         _viewItem = A.Fake<IViewItem>();
         _point = new Point(1, 2);
         _menu = A.Fake<IContextMenu>();
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(_viewItem, sut)).Returns(_menu);
      }

      [Observation]
      public void should_ask_context_menu_factory_for_the_right_context_menu()
      {
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(_viewItem, sut)).MustHaveHappened();
      }

      [Observation]
      public void should_tell_view_to_show_the_menu_at_point()
      {
         A.CallTo(() => _menu.Show(_view, _point)).MustHaveHappened();
      }
   }

   public class When_double_clikcing_on_a_node_that_is_not_a_molecule_builder_node : concern_for_BuildingBlockExplorerPresenter
   {
      private ITreeNode _node;
      private IViewItem _viewItem;
      private IContextMenu _contextMenu;

      protected override void Context()
      {
         base.Context();
         _node = A.Fake<ITreeNode>();
         _viewItem = A.Fake<IViewItem>();
         _contextMenu = A.Fake<IContextMenu>();
         A.CallTo(() => _node.TagAsObject).Returns(_viewItem);
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(_viewItem, sut)).Returns(_contextMenu);
      }

      protected override void Because()
      {
         sut.NodeDoubleClicked(_node);
      }

      [Observation]
      public void should_invoke_the_first_elementof_the_context_menu()
      {
         A.CallTo(() => _contextMenu.ActivateFirstMenu()).MustHaveHappened();
      }
   }

   public class When_double_clikcing_on_a_node_that_is_a_molecule_builder_node : concern_for_BuildingBlockExplorerPresenter
   {
      private ITreeNode _node;
      private IMoleculeBuilder _moleculeBuilder;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = A.Fake<IMoleculeBuilder>();
         _moleculeBuildingBlock = A.Fake<IMoleculeBuildingBlock>();
         var moleculeBuildingBlockNode = A.Fake<ITreeNode>();
         A.CallTo(() => moleculeBuildingBlockNode.TagAsObject).Returns(_moleculeBuildingBlock);
         _node = A.Fake<ITreeNode>();
         _node.ParentNode = moleculeBuildingBlockNode;
         A.CallTo(() => _node.TagAsObject).Returns(_moleculeBuilder);
      }

      protected override void Because()
      {
         sut.NodeDoubleClicked(_node);
      }

      [Observation]
      public void should_invoke_the_first_elementof_the_context_menu()
      {
         A.CallTo(() => _editBuildingBlockStarter.EditMolecule(_moleculeBuildingBlock, _moleculeBuilder)).MustHaveHappened();
      }
   }
}