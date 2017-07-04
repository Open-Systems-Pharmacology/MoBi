using System.Collections.Generic;
using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
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
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using System;

namespace MoBi.Presentation
{
   public abstract class concern_for_BuildingBlockExplorerPresenter : ContextSpecification<IBuildingBlockExplorerPresenter>
   {
      protected IBuildingBlockExplorerView _view;
      protected IMoBiContext _context;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IRegionResolver _regionResolver;
      protected ITreeNodeFactory _treeNodeFactory;
      private IClassificationPresenter _classificationPresenter;
      private IToolTipPartCreator _toolTipPartCreator;
      protected IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      private IProjectRetriever _projectRetriever;
      protected IEditBuildingBlockStarter _editBuildingBlockStarter;

      protected ITreeNode<RootNodeType> _nodeSpatialStructureFolder;
      protected ITreeNode<RootNodeType> _nodeMoleculeFolder;
      protected ITreeNode<RootNodeType> _nodeReactionFolder;
      protected ITreeNode<RootNodeType> _nodePassiveTransportFolder;
      protected ITreeNode<RootNodeType> _nodeObserverFolder;
      protected ITreeNode<RootNodeType> _nodeEventFolder;
      protected ITreeNode<RootNodeType> _nodeSimulationSettingsFolder;
      protected ITreeNode<RootNodeType> _nodeMoleculeStartValuesFolder;
      protected ITreeNode<RootNodeType> _nodeParameterStartValuesFolder;
      protected ITreeNode<RootNodeType> _nodeObservedDataFolder;

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

         _nodeSpatialStructureFolder = setupNode(MoBiRootNodeTypes.SpatialStructureFolder);
         _nodeMoleculeFolder = setupNode(MoBiRootNodeTypes.MoleculeFolder);
         _nodeReactionFolder = setupNode(MoBiRootNodeTypes.ReactionFolder);
         _nodePassiveTransportFolder = setupNode(MoBiRootNodeTypes.PassiveTransportFolder);
         _nodeObserverFolder = setupNode(MoBiRootNodeTypes.ObserverFolder);
         _nodeEventFolder = setupNode(MoBiRootNodeTypes.EventFolder);
         _nodeSimulationSettingsFolder = setupNode(MoBiRootNodeTypes.SimulationSettingsFolder);
         _nodeMoleculeStartValuesFolder = setupNode(MoBiRootNodeTypes.MoleculeStartValuesFolder);
         _nodeParameterStartValuesFolder = setupNode(MoBiRootNodeTypes.ParameterStartValuesFolder);
         _nodeObservedDataFolder = setupNode(RootNodeTypes.ObservedDataFolder);
      }

      private ITreeNode<RootNodeType> setupNode(RootNodeType rootNodeType)
      {
         var nodeFolder = A.Fake<ITreeNode<RootNodeType>>();
         A.CallTo(() => _treeNodeFactory.CreateFor(rootNodeType)).Returns(nodeFolder);
         A.CallTo(() => _view.TreeView.NodeById(rootNodeType.Id)).Returns(nodeFolder);
         return nodeFolder;
      }
   }

   public class When_the_building_block_explorer_presenter_is_notified_that_the_project_is_being_closed : concern_for_BuildingBlockExplorerPresenter
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

   public class When_the_building_block_explorer_presenter_is_told_to_show_a_context_menu : concern_for_BuildingBlockExplorerPresenter
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

   public class When_double_clicking_on_a_node_that_is_not_a_molecule_builder_node : concern_for_BuildingBlockExplorerPresenter
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

   public class When_double_clicking_on_a_node_that_is_a_molecule_builder_node : concern_for_BuildingBlockExplorerPresenter
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

   public class When_the_building_block_explorer_presenter_is_adding_the_project_to_the_tree : concern_for_BuildingBlockExplorerPresenter
   {
      private List<ITreeNode> _allNodesAdded;
      private IMoBiProject _project;
      private ObserverBuildingBlock _observerBuildingBlock;
      private SimulationSettings _simulationSettingsBuildingBlock;
      private ITreeNode _simulationSettingsNode;
      private ITreeNode _observerBuildingBlockNode;

      protected override void Context()
      {
         base.Context();
         _project= new MoBiProject();
         _observerBuildingBlock = new ObserverBuildingBlock().WithName("OBSERVERS");
         _simulationSettingsBuildingBlock = new SimulationSettings().WithName("SIMULATION_SETTINGS");
         _simulationSettingsNode= A.Fake<ITreeNode>();
         _observerBuildingBlockNode= A.Fake<ITreeNode>();

         _allNodesAdded =new List<ITreeNode>();
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x =>
         {
            _allNodesAdded.Add(x.GetArgument<ITreeNode>(0));
         });

         _project.AddBuildingBlock(_observerBuildingBlock);
         _project.AddBuildingBlock(_simulationSettingsBuildingBlock);

         A.CallTo(() => _treeNodeFactory.CreateFor(_simulationSettingsBuildingBlock)).Returns(_simulationSettingsNode);
         A.CallTo(() => _treeNodeFactory.CreateFor(_observerBuildingBlock)).Returns(_observerBuildingBlockNode);

      }

      protected override void Because()
      {
         sut.Handle(new ProjectCreatedEvent(_project));
      }

      [Observation]
      public void should_add_a_folder_node_for_all_building_block_types()
      {
         _allNodesAdded.ShouldContain(_nodeSpatialStructureFolder, _nodeMoleculeFolder, _nodeReactionFolder, _nodePassiveTransportFolder,
            _nodeObserverFolder, _nodeEventFolder, _nodeMoleculeFolder, _nodeSimulationSettingsFolder, _nodeMoleculeStartValuesFolder, _nodeParameterStartValuesFolder);
      }

      [Observation]
      public void should_add_a_folder_node_for_observed_data()
      {
         _allNodesAdded.ShouldContain(_nodeObservedDataFolder);         
      }

      [Observation]
      public void should_add_a_node_for_each_building_block_under_the_expected_folder()
      {
         _allNodesAdded.ShouldContain(_simulationSettingsNode, _observerBuildingBlockNode);

         A.CallTo(() => _nodeSimulationSettingsFolder.AddChild(_simulationSettingsNode)).MustHaveHappened();
         A.CallTo(() => _nodeObserverFolder.AddChild(_observerBuildingBlockNode)).MustHaveHappened();
      }
   }

   public abstract class When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<T> : concern_for_BuildingBlockExplorerPresenter where T: IBuildingBlock, new()
   {
      private T _buildingBlock;
      private ITreeNode _buildingBlockNode;
      private ITreeNode<RootNodeType> _folderNode;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new T();
         _buildingBlockNode = A.Fake<ITreeNode>();
         A.CallTo(() => _treeNodeFactory.CreateFor(_buildingBlock)).Returns(_buildingBlockNode);
         _folderNode = RetrieveFolderNode();

      }

      protected abstract ITreeNode<RootNodeType> RetrieveFolderNode();

      protected override void Because()
      {
         sut.Handle(new AddedEvent<T>(_buildingBlock, null));
      }

      [Observation]
      public void should_add_a_node_corresponding_to_the_given_building_block()
      {
         A.CallTo(() => _view.AddNode(_buildingBlockNode)).MustHaveHappened();
      }

      [Observation]
      public void should_have_added_the_node_under_the_expected_parent_node()
      {
         A.CallTo(() => _folderNode.AddChild(_buildingBlockNode)).MustHaveHappened();
      }
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_an_observer_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<ObserverBuildingBlock>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeObserverFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_an_simulation_settings_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<SimulationSettings>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeSimulationSettingsFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_a_spatial_structure_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<SpatialStructure>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeSpatialStructureFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_a_reaction_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<ReactionBuildingBlock>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeReactionFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_an_event_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<EventGroupBuildingBlock>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeEventFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_a_molecule_start_value_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<MoleculeStartValuesBuildingBlock>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeMoleculeStartValuesFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_a_parameter_start_value_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<ParameterStartValuesBuildingBlock>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodeParameterStartValuesFolder;
   }

   public class When_the_building_block_explorer_presenter_is_being_notified_that_a_passive_transport_building_block_was_added : When_the_building_block_explorer_presenter_is_being_notified_that_a_building_block_was_added<PassiveTransportBuildingBlock>
   {
      protected override ITreeNode<RootNodeType> RetrieveFolderNode() => _nodePassiveTransportFolder;
   }

   public class When_the_building_block_explorer_presenter_is_notified_that_a_molecule_was_added : concern_for_BuildingBlockExplorerPresenter
   {
      private IMoleculeBuilder _moleculeBuilder;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private ITreeNode _moleculeBuildingBlockNode;
      private ITreeNode _moleculeNode;

      protected override void Context()
      {
         base.Context();
         _moleculeNode= A.Fake<ITreeNode>();
         _moleculeBuildingBlockNode = A.Fake<ITreeNode>();
         _moleculeBuildingBlock = A.Fake<IMoleculeBuildingBlock>().WithId("Id");
         _moleculeBuilder= A.Fake<IMoleculeBuilder>();

         A.CallTo(() => _view.TreeView.NodeById(_moleculeBuildingBlock.Id)).Returns(_moleculeBuildingBlockNode);

         A.CallTo(() => _treeNodeFactory.CreateFor(_moleculeBuilder)).Returns(_moleculeNode);
      }

      protected override void Because()
      {
         sut.Handle(new AddedEvent<IMoleculeBuilder>(_moleculeBuilder,_moleculeBuildingBlock));
      }

      [Observation]
      public void should_add_a_node_for_the_added_molecule_under_the_molecule_building_block_node()
      {
         A.CallTo(() => _moleculeBuildingBlockNode.AddChild(_moleculeNode)).MustHaveHappened();

         A.CallTo(() => _view.AddNode(_moleculeNode)).MustHaveHappened();
      }
   }

   public class When_the_building_block_explorer_presenter_is_asked_if_a_node_can_be_dragged : concern_for_BuildingBlockExplorerPresenter
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