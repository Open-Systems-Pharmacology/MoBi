using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Core.Domain.Repositories;

namespace MoBi.Presentation
{
   public abstract class concern_for_ModuleExplorerPresenter : ContextSpecification<ModuleExplorerPresenter>
   {
      protected IModuleExplorerView _view;
      protected IMoBiContext _context;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IRegionResolver _regionResolver;
      protected Nodes.TreeNodeFactory _treeNodeFactory;
      private IClassificationPresenter _classificationPresenter;
      private IToolTipPartCreator _toolTipPartCreator;
      protected IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      private IProjectRetriever _projectRetriever;
      protected IEditBuildingBlockStarter _editBuildingBlockStarter;

      protected ITreeNode<RootNodeType> _nodeObservedDataFolder;
      private IObservedDataRepository _observedDataRepository;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _view = A.Fake<IModuleExplorerView>();
         A.CallTo(() => _view.TreeView).Returns(A.Fake<IUxTreeView>());
         _context = A.Fake<IMoBiContext>();
         _regionResolver = A.Fake<IRegionResolver>();
         _observedDataRepository = A.Fake<IObservedDataRepository>();

         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _classificationPresenter = A.Fake<IClassificationPresenter>();
         _toolTipPartCreator = A.Fake<IToolTipPartCreator>();
         _observedDataInExplorerPresenter = A.Fake<IObservedDataInExplorerPresenter>();
         _multipleTreeNodeContextMenuFactory = A.Fake<IMultipleTreeNodeContextMenuFactory>();
         _editBuildingBlockStarter = A.Fake<IEditBuildingBlockStarter>();
         _treeNodeFactory = new Nodes.TreeNodeFactory(_observedDataRepository, _toolTipPartCreator);
         
         sut = new ModuleExplorerPresenter(_view, _regionResolver, _treeNodeFactory, _viewItemContextMenuFactory, _context,
            _classificationPresenter, _toolTipPartCreator, _observedDataInExplorerPresenter, _multipleTreeNodeContextMenuFactory, _projectRetriever, _editBuildingBlockStarter);
      }
   }

   public class When_the_module_explorer_presenter_is_notified_that_the_project_is_being_closed : concern_for_ModuleExplorerPresenter
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

   public class When_the_module_explorer_presenter_is_told_to_show_a_context_menu : concern_for_ModuleExplorerPresenter
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

   public class When_double_clicking_on_a_node_that_is_not_a_molecule_builder_node_in_module_explorer : concern_for_ModuleExplorerPresenter
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
      public void should_invoke_the_first_element_of_the_context_menu()
      {
         A.CallTo(() => _contextMenu.ActivateFirstMenu()).MustHaveHappened();
      }
   }

   public class When_double_clicking_on_a_node_that_is_a_molecule_builder_node_in_module_explorer : concern_for_ModuleExplorerPresenter
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
      public void should_invoke_the_first_element_of_the_context_menu()
      {
         A.CallTo(() => _editBuildingBlockStarter.EditMolecule(_moleculeBuildingBlock, _moleculeBuilder)).MustHaveHappened();
      }
   }

   public class When_the_module_explorer_presenter_compares_nodes_of_the_different_types : concern_for_ModuleExplorerPresenter
   {
      private ITreeNode<SpatialStructure> _spatialStructureA;
      private ITreeNode<RootNodeType> _spatialStructureRootNode;
      private ITreeNode<RootNodeType> _moduleNode;

      protected override void Context()
      {
         base.Context();
         _moduleNode = _treeNodeFactory.CreateFor(MoBiRootNodeTypes.PKSimModuleFolder);
         _spatialStructureA = _treeNodeFactory.CreateFor<SpatialStructure>(new SpatialStructure().WithName("A"));
         _spatialStructureRootNode = _treeNodeFactory.CreateFor(MoBiRootNodeTypes.SpatialStructureFolder);
      }

      [Observation]
      public void module_nodes_are_superior()
      {
         sut.OrderingComparisonFor(_moduleNode, _spatialStructureRootNode).ShouldBeEqualTo(-1);
         sut.OrderingComparisonFor(_moduleNode, _spatialStructureA).ShouldBeEqualTo(-1);
      }
   }

   public class When_the_module_explorer_presenter_compares_nodes_of_the_same_type : concern_for_ModuleExplorerPresenter
   {
      private ITreeNode<SpatialStructure> _spatialStructureA;
      private ITreeNode<RootNodeType> _spatialStructureRootNode;
      private ITreeNode<RootNodeType> _eventGroupRootNode;
      private ITreeNode<SpatialStructure> _spatialStructureZ;
      private ITreeNode<Module> _moduleNodeA;
      private ITreeNode<Module> _moduleNodeZ;

      protected override void Context()
      {
         base.Context();
         _moduleNodeA = _treeNodeFactory.CreateFor(new Module().WithName("A"));
         _moduleNodeZ = _treeNodeFactory.CreateFor(new Module().WithName("Z"));
         _spatialStructureA = _treeNodeFactory.CreateFor<SpatialStructure>(new SpatialStructure().WithName("A"));
         _spatialStructureZ = _treeNodeFactory.CreateFor<SpatialStructure>(new SpatialStructure().WithName("Z"));
         _spatialStructureRootNode = _treeNodeFactory.CreateFor(MoBiRootNodeTypes.SpatialStructureFolder);
         _eventGroupRootNode = _treeNodeFactory.CreateFor(MoBiRootNodeTypes.EventFolder);
      }

      [Observation]
      public void should_not_compare_for_building_block_nodes()
      {
         sut.OrderingComparisonFor(_spatialStructureA, _spatialStructureZ).ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_compare_names_for_module_nodes()
      {
         sut.OrderingComparisonFor(_moduleNodeA, _moduleNodeZ).ShouldBeEqualTo(-1);
      }


      [Observation]
      public void should_compare_names_for_root_nodes()
      {
         sut.OrderingComparisonFor(_spatialStructureRootNode, _eventGroupRootNode).ShouldBeEqualTo(1);
      }
   }

   public class When_the_module_explorer_presenter_is_adding_the_project_to_the_tree : concern_for_ModuleExplorerPresenter
   {
      private List<ITreeNode> _allNodesAdded;
      private IMoBiProject _project;
      private ObserverBuildingBlock _observerBuildingBlock;
      private SimulationSettings _simulationSettingsBuildingBlock;
      private Module _module1;

      protected override void Context()
      {
         base.Context();
         _project = new MoBiProject();
         _observerBuildingBlock = new ObserverBuildingBlock().WithName("OBSERVERS");
         _simulationSettingsBuildingBlock = new SimulationSettings().WithName("SIMULATION_SETTINGS");
         _module1 = new Module
         {
            SpatialStructure = new MoBiSpatialStructure()
         };

         _module1.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
         _module1.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
         
         _module1.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());

         _allNodesAdded = new List<ITreeNode>();
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x =>
         {
            _allNodesAdded.Add(x.GetArgument<ITreeNode>(0));
         });

         _project.AddBuildingBlock(_observerBuildingBlock);
         _project.AddBuildingBlock(_simulationSettingsBuildingBlock);
         _project.AddModule(_module1);
      }

      protected override void Because()
      {
         sut.Handle(new ProjectCreatedEvent(_project));
      }

      [Observation]
      public void should_add_a_folder_node_for_all_building_block_types()
      {
         _allNodesAdded.Count(x => x.TagAsObject.Equals(MoBiRootNodeTypes.ExpressionProfilesFolder)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => x.TagAsObject.Equals(MoBiRootNodeTypes.IndividualsFolder)).ShouldBeEqualTo(1);

         // We have a module with two parameter start values building blocks so a root node should be created
         _allNodesAdded.Count(x => x.TagAsObject.Equals(MoBiRootNodeTypes.ParameterStartValuesFolder)).ShouldBeEqualTo(1);

         // There is only one molecule start values building block so a root node should not be created
         _allNodesAdded.Count(x => x.TagAsObject.Equals(MoBiRootNodeTypes.MoleculeStartValuesFolder)).ShouldBeEqualTo(0);

         _allNodesAdded.Count(x => x.TagAsObject.Equals(RootNodeTypes.ObservedDataFolder)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => x.TagAsObject.Equals(_module1)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => x.TagAsObject.Equals(_module1.SpatialStructure)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => x.TagAsObject.Equals(_module1.ParameterStartValuesCollection.ElementAt(0))).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => x.TagAsObject.Equals(_module1.ParameterStartValuesCollection.ElementAt(1))).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => x.TagAsObject.Equals(_module1.MoleculeStartValuesCollection.ElementAt(0))).ShouldBeEqualTo(1);
         
         // Make sure nodes have not been added for null items
         _allNodesAdded.Count.ShouldBeEqualTo(11);
      }
   }

   public class When_the_module_explorer_presenter_is_asked_if_a_node_can_be_dragged : concern_for_ModuleExplorerPresenter
   {
      [Observation]
      public void should_return_false_if_the_node_is_null()
      {
         sut.CanDrag(null).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_if_the_node_is_a_classification_node()
      {
         sut.CanDrag(new ClassificationNode(A.Fake<IClassification>())).ShouldBeTrue();
      }

      [Observation]
      public void should_delegate_to_the_observed_data_presenter_otherwise()
      {
         var node = A.Fake<ITreeNode>();
         A.CallTo(() => _observedDataInExplorerPresenter.CanDrag(node)).Returns(true);
         sut.CanDrag(node).ShouldBeTrue();

         A.CallTo(() => _observedDataInExplorerPresenter.CanDrag(node)).Returns(false);
         sut.CanDrag(node).ShouldBeFalse();
      }
   }
}