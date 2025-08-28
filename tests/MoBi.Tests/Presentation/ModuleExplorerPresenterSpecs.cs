using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;
using TreeNodeFactory = MoBi.Presentation.Nodes.TreeNodeFactory;

namespace MoBi.Presentation
{
   public abstract class concern_for_ModuleExplorerPresenter : ContextSpecification<ModuleExplorerPresenter>
   {
      protected IModuleExplorerView _view;
      protected IMoBiContext _context;
      protected IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IRegionResolver _regionResolver;
      protected TreeNodeFactory _treeNodeFactory;
      protected IClassificationPresenter _classificationPresenter;
      private IToolTipPartCreator _toolTipPartCreator;
      protected IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      protected IProjectRetriever _projectRetriever;
      protected IEditBuildingBlockStarter _editBuildingBlockStarter;
      protected IInteractionTasksForModule _interactionTaskForModule;
      private IObservedDataRepository _observedDataRepository;
      protected IModulesInExplorerPresenter _modulesInExplorerPresenter;

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
         _editBuildingBlockStarter = A.Fake<IEditBuildingBlockStarter>();
         _treeNodeFactory = new TreeNodeFactory(_observedDataRepository, _toolTipPartCreator);
         _interactionTaskForModule = A.Fake<IInteractionTasksForModule>();
         _modulesInExplorerPresenter = new ModulesInExplorerPresenter(_treeNodeFactory);
         sut = new ModuleExplorerPresenter(_view, _regionResolver, _treeNodeFactory, _viewItemContextMenuFactory, _context,
            _classificationPresenter, _toolTipPartCreator, _observedDataInExplorerPresenter, _multipleTreeNodeContextMenuFactory, _projectRetriever, _editBuildingBlockStarter, _interactionTaskForModule, _modulesInExplorerPresenter);
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
      private MoleculeBuilder _moleculeBuilder;
      private MoleculeBuildingBlock _moleculeBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _moleculeBuilder = A.Fake<MoleculeBuilder>();
         _moleculeBuildingBlock = A.Fake<MoleculeBuildingBlock>();
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
      private ITreeNode<RootNodeType> _moduleNode;
      private InitialConditionsFolderNode _initialConditionsFolderNode;
      private ParameterValuesFolderNode _parameterValuesFolderNode;

      protected override void Context()
      {
         base.Context();

         _spatialStructureA = _treeNodeFactory.CreateFor<SpatialStructure>(new SpatialStructure().WithName("A"));
         _moduleNode = _treeNodeFactory.CreateFor(RootNodeTypes.ModulesFolder);
         var module = new Module();
         _parameterValuesFolderNode = new ParameterValuesFolderNode(new ClassifiableModule
         {
            Subject = module
         });
         _initialConditionsFolderNode = new InitialConditionsFolderNode(new ClassifiableModule
         {
            Subject = module
         });
      }

      [Observation]
      public void module_nodes_are_superior()
      {
         sut.OrderingComparisonFor(_moduleNode, _spatialStructureA).ShouldBeEqualTo(-1);
      }

      [Observation]
      public void the_start_values_folders_are_inferior_to_other_node_types()
      {
         sut.OrderingComparisonFor(_parameterValuesFolderNode, _spatialStructureA).ShouldBeEqualTo(1);
         sut.OrderingComparisonFor(_initialConditionsFolderNode, _spatialStructureA).ShouldBeEqualTo(1);
         sut.OrderingComparisonFor(_parameterValuesFolderNode, _moduleNode).ShouldBeEqualTo(1);
         sut.OrderingComparisonFor(_initialConditionsFolderNode, _moduleNode).ShouldBeEqualTo(1);
         sut.OrderingComparisonFor(_moduleNode, _parameterValuesFolderNode).ShouldBeEqualTo(-1);
         sut.OrderingComparisonFor(_moduleNode, _initialConditionsFolderNode).ShouldBeEqualTo(-1);
         sut.OrderingComparisonFor(_initialConditionsFolderNode, _parameterValuesFolderNode).ShouldBeEqualTo(-1);
         sut.OrderingComparisonFor(_parameterValuesFolderNode, _initialConditionsFolderNode).ShouldBeEqualTo(1);
      }
   }

   public class When_the_module_explorer_presenter_compares_nodes_of_the_same_type : concern_for_ModuleExplorerPresenter
   {
      private ITreeNode<SpatialStructure> _spatialStructureA;
      private ITreeNode<SpatialStructure> _spatialStructureZ;
      private ITreeNode<Module> _moduleNodeA;
      private ITreeNode<Module> _moduleNodeZ;

      protected override void Context()
      {
         base.Context();
         _moduleNodeA = _treeNodeFactory.CreateFor(new Module().WithName("A")) as ITreeNode<Module>;
         _moduleNodeZ = _treeNodeFactory.CreateFor(new Module().WithName("Z")) as ITreeNode<Module>;
         _spatialStructureA = _treeNodeFactory.CreateFor<SpatialStructure>(new SpatialStructure().WithName("A"));
         _spatialStructureZ = _treeNodeFactory.CreateFor<SpatialStructure>(new SpatialStructure().WithName("Z"));
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
   }

   public abstract class When_the_module_explorer_presenter_receives_an_added_event<T> : concern_for_ModuleExplorerPresenter where T : new()
   {
      protected MoBiProject _project;
      protected T _addedObject;

      protected override void Context()
      {
         base.Context();
         _addedObject = new T();
         _project = DomainHelperForSpecs.NewProject();
      }
   }

   public class When_the_module_explorer_presenter_receives_an_added_individual_event : When_the_module_explorer_presenter_receives_an_added_event<IndividualBuildingBlock>
   {
      protected override void Because()
      {
         sut.Handle(new AddedEvent<IndividualBuildingBlock>(_addedObject, _project));
      }

      [Observation]
      public void the_tree_node_should_be_added_to_the_root_node()
      {
         A.CallTo(() => _view.AddNode(A<ITreeNode>.That.Matches(x => x.TagAsObject.Equals(_addedObject)))).MustHaveHappened();
      }
   }

   public class When_the_module_explorer_presenter_receives_an_added_module_event : When_the_module_explorer_presenter_receives_an_added_event<Module>
   {
      private ITreeNode _rootNode;
      private ITreeNode _moduleNode;
      private EventGroupBuildingBlock _eventGroupBuildingBlock;
      private ITreeNode _eventGroupNode;
      private IContextMenu _contextMenu;
      private ClassifiableModule _classifiableModule;

      protected override void Context()
      {
         base.Context();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock().WithId("eventGroupId");
         _rootNode = new RootNode(RootNodeTypes.ModulesFolder);
         _classifiableModule = new ClassifiableModule { Subject = _addedObject };
         _moduleNode = new ModuleNode(_classifiableModule);
         _eventGroupNode = new BuildingBlockNode(_eventGroupBuildingBlock);
         A.CallTo(() => _view.TreeView.NodeById(_rootNode.Id)).Returns(_rootNode);
         A.CallTo(() => _view.TreeView.NodeById(_addedObject.Id)).Returns(_moduleNode);


         _addedObject.Add(_eventGroupBuildingBlock);

         A.CallTo(() => _view.TreeView.NodeById(_eventGroupBuildingBlock.Id)).Returns(_eventGroupNode);

         _contextMenu = A.Fake<IContextMenu>();
         A.CallTo(() => _viewItemContextMenuFactory.CreateFor(A<BuildingBlockViewItem>.That.Matches(x => x.BuildingBlock.Equals(_eventGroupBuildingBlock)), sut)).Returns(_contextMenu);
         A.CallTo(() => _projectRetriever.CurrentProject.GetOrCreateClassifiableFor<ClassifiableModule, Module>(_addedObject)).Returns(_classifiableModule);
      }

      [Observation]
      public void the_tree_node_should_be_added_to_the_root_node()
      {
         A.CallTo(() => _view.AddNode(A<ITreeNode>.That.Matches(x => x.TagAsObject.Equals(_classifiableModule)))).MustHaveHappened();
      }

      protected override void Because()
      {
         sut.Handle(new AddedEvent<Module>(_addedObject, _project));
      }

      [Observation]
      public void the_single_building_block_should_be_opened_in_the_editor()
      {
         A.CallTo(() => _contextMenu.ActivateFirstMenu()).MustHaveHappened();
      }

      [Observation]
      public void the_modules_root_folder_should_be_expanded()
      {
         A.CallTo(() => _view.TreeView.ExpandNode(_rootNode)).MustHaveHappened();
      }

      [Observation]
      public void the_module_node_should_be_expanded()
      {
         A.CallTo(() => _view.TreeView.ExpandNode(_moduleNode)).MustHaveHappened();
      }
   }

   public class When_the_module_explorer_presenter_receives_an_added_expression_event : When_the_module_explorer_presenter_receives_an_added_event<ExpressionProfileBuildingBlock>
   {
      protected override void Because()
      {
         _addedObject.Type = ExpressionTypes.MetabolizingEnzyme;
         sut.Handle(new AddedEvent<ExpressionProfileBuildingBlock>(_addedObject, _project));
      }

      [Observation]
      public void the_tree_node_should_be_added_to_the_root_node()
      {
         A.CallTo(() => _view.AddNode(A<ITreeNode>.That.Matches(x => x.TagAsObject.Equals(_addedObject)))).MustHaveHappened();
      }
   }

   public class When_the_module_explorer_presenter_is_adding_the_project_to_the_tree_without_initial_conditions : concern_for_ModuleExplorerPresenter
   {
      private List<ITreeNode> _allNodesAdded;
      private MoBiProject _project;
      private Module _module1;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _module1 = new Module
         {
            new MoBiSpatialStructure(),
            new ParameterValuesBuildingBlock().WithId("PSV1"),
            new ParameterValuesBuildingBlock().WithId("PSV2"),
         };

         _allNodesAdded = new List<ITreeNode>();
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x =>
         {
            var treeNode = x.GetArgument<ITreeNode>(0);
            flattenAndAdd(treeNode);
         });

         A.CallTo(() => _view.TreeView.NodeById(A<string>._)).ReturnsLazily(x => getNodeForId(x.Arguments.Get<string>(0)));

         _project.AddModule(_module1);
      }

      private ITreeNode getNodeForId(string id)
      {
         return _allNodesAdded.First(x => x.Id == id);
      }

      private void flattenAndAdd(ITreeNode treeNode)
      {
         treeNode.Children.Each(flattenAndAdd);
         if (_allNodesAdded.Contains(treeNode))
            return;
         _allNodesAdded.Add(treeNode);
      }

      protected override void Because()
      {
         sut.Handle(new ProjectCreatedEvent(_project));
      }

      [Observation]
      public void should_add_a_folder_node_for_all_building_block_types_except_initial_conditions()
      {
         _allNodesAdded.Count(x => Equals(MoBiRootNodeTypes.ExpressionProfilesFolder, x.TagAsObject)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => Equals(MoBiRootNodeTypes.IndividualsFolder, x.TagAsObject)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => Equals(RootNodeTypes.ObservedDataFolder, x.TagAsObject)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => Equals(RootNodeTypes.ModulesFolder, x.TagAsObject)).ShouldBeEqualTo(1);

         // The 4 nodes above + Spatial Struct, PSV Folder, 2x PSV
         _allNodesAdded.Count.ShouldBeEqualTo(8);
      }
   }

   public class When_the_module_explorer_presenter_is_adding_the_project_to_the_tree : concern_for_ModuleExplorerPresenter
   {
      private List<ITreeNode> _allNodesAdded;
      private MoBiProject _project;
      private Module _module1;
      private ITreeNode _rootNode;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _module1 = new Module
         {
            new MoBiSpatialStructure(),
            new ParameterValuesBuildingBlock().WithId("PSV1"),
            new ParameterValuesBuildingBlock().WithId("PSV2"),
            new InitialConditionsBuildingBlock().WithId("MSV")
         };
         _rootNode = new RootNode(RootNodeTypes.ModulesFolder);

         _allNodesAdded = new List<ITreeNode>();
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x =>
         {
            var treeNode = x.GetArgument<ITreeNode>(0);
            flattenAndAdd(treeNode);
         });

         _project.AddModule(_module1);
         A.CallTo(() => _view.TreeView.NodeById(_rootNode.Id)).Returns(_rootNode);
      }

      private void flattenAndAdd(ITreeNode treeNode)
      {
         treeNode.Children.Each(flattenAndAdd);
         if (_allNodesAdded.Contains(treeNode))
            return;
         _allNodesAdded.Add(treeNode);
      }

      protected override void Because()
      {
         sut.Handle(new ProjectCreatedEvent(_project));
      }

      [Observation]
      public void the_modules_root_folder_should_not_be_expanded()
      {
         A.CallTo(() => _view.TreeView.ExpandNode(_rootNode)).MustNotHaveHappened();
      }

      [Observation]
      public void should_add_a_folder_node_for_all_building_block_types_not_in_modules()
      {
         _allNodesAdded.Count(x => Equals(MoBiRootNodeTypes.ExpressionProfilesFolder, x.TagAsObject)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => Equals(MoBiRootNodeTypes.IndividualsFolder, x.TagAsObject)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => Equals(RootNodeTypes.ObservedDataFolder, x.TagAsObject)).ShouldBeEqualTo(1);
         _allNodesAdded.Count(x => Equals(RootNodeTypes.ModulesFolder, x.TagAsObject)).ShouldBeEqualTo(1);

         // The 4 nodes above + Spatial Struct, PSV Folder, MSV Folder, 2x PSV and 1x MSV
         _allNodesAdded.Count.ShouldBeEqualTo(10);
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

   public class When_the_last_parameter_values_is_removed_from_a_module : When_the_last_folder_subnode_is_removed_from_a_module<ParameterValuesFolderNode, ParameterValuesBuildingBlock>
   {
      protected override void Because()
      {
         sut.Handle(new RemovedEvent(new List<IObjectBase> { _module1.ParameterValuesCollection[0], _module1.ParameterValuesCollection[1] }));
      }
   }

   public class When_the_last_initial_conditions_is_removed_from_a_module : When_the_last_folder_subnode_is_removed_from_a_module<InitialConditionsFolderNode, InitialConditionsBuildingBlock>
   {
      protected override void Because()
      {
         sut.Handle(new RemovedEvent(new List<IObjectBase> { _module1.InitialConditionsCollection[0], _module1.InitialConditionsCollection[1] }));
      }
   }

   public abstract class When_the_last_folder_subnode_is_removed_from_a_module<TFolderNode, TBuildingBlock> : concern_for_ModuleExplorerPresenter where TFolderNode : ITreeNode where TBuildingBlock : BuildingBlock, new()
   {
      private List<ITreeNode> _allNodesAdded;
      private MoBiProject _project;
      protected Module _module1;
      private TFolderNode _folderNode;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _module1 = new Module
         {
            new MoBiSpatialStructure(),
            new TBuildingBlock().WithId("PSV1"),
            new TBuildingBlock().WithId("PSV2"),
         };

         _allNodesAdded = new List<ITreeNode>();
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x =>
         {
            var treeNode = x.GetArgument<ITreeNode>(0);
            flattenAndAdd(treeNode);
         });

         A.CallTo(() => _view.TreeView.NodeById(A<string>._)).ReturnsLazily(x => getNodeForId(x.Arguments.Get<string>(0)));
         A.CallTo(() => _view.TreeView.DestroyNode(A<ITreeNode>._)).Invokes(x => removeNode(x.Arguments.Get<ITreeNode>(0)));
         _project.AddModule(_module1);
         sut.Handle(new ProjectCreatedEvent(_project));
      }

      private void removeNode(ITreeNode treeNode)
      {
         _allNodesAdded.Remove(treeNode);
         treeNode.ParentNode.RemoveChild(treeNode);
      }

      private ITreeNode getNodeForId(string id)
      {
         return _allNodesAdded.First(x => x.Id == id);
      }

      private void flattenAndAdd(ITreeNode treeNode)
      {
         treeNode.Children.Each(flattenAndAdd);
         _allNodesAdded.Add(treeNode);
         if (treeNode is TFolderNode pvFolderNode)
            _folderNode = pvFolderNode;
      }

      [Observation]
      public void should_remove_the_folder_node()
      {
         A.CallTo(() => _view.TreeView.RemoveNode(_folderNode)).MustHaveHappened();
      }
   }

   public class When_handling_add_molecule_builder_event : concern_for_ModuleExplorerPresenter
   {
      private MoleculeBuildingBlock _buildingBlock;
      private ITreeNode _moleculeBuildingBlockNode;
      private MoBiProject _project;
      private Module _module;
      private readonly List<ITreeNode> _allNodesAdded = new List<ITreeNode>();

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoleculeBuildingBlock();
         _project = new MoBiProject();
         _module = new Module { _buildingBlock };
         _project.AddModule(_module);

         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x => flattenAndAdd(x.Arguments.Get<ITreeNode>(0)));
         A.CallTo(() => _view.TreeView.DestroyNode(A<ITreeNode>._)).Invokes(x => removeNode(x.Arguments.Get<ITreeNode>(0)));
         A.CallTo(() => _view.TreeView.NodeById(A<string>._)).ReturnsLazily(x => getNodeForId(x.Arguments.Get<string>(0)));

         sut.Handle(new ProjectCreatedEvent(_project));
      }

      private void removeNode(ITreeNode treeNode)
      {
         _allNodesAdded.Remove(treeNode);
         treeNode.ParentNode.RemoveChild(treeNode);
      }

      private ITreeNode getNodeForId(string id)
      {
         return _allNodesAdded.First(x => x.Id == id);
      }

      private void flattenAndAdd(ITreeNode treeNode)
      {
         treeNode.Children.Each(flattenAndAdd);
         _allNodesAdded.Add(treeNode);
         if (isModuleNode(treeNode))
            _moleculeBuildingBlockNode = treeNode.Children.First(child => ReferenceEquals(child.TagAsObject, _buildingBlock));
      }

      private bool isModuleNode(ITreeNode treeNode)
      {
         if (treeNode.TagAsObject is ClassifiableModule classifiable)
            return classifiable.Subject.Equals(_module);

         return false;
      }

      protected override void Because()
      {
         sut.Handle(new AddedEvent<MoleculeBuilder>(new MoleculeBuilder(), _buildingBlock));
      }

      [Observation]
      public void the_molecule_should_be_inserted_under_the_building_block()
      {
         _moleculeBuildingBlockNode.Children.Count().ShouldBeEqualTo(1);
      }
   }

   public abstract class When_dragging_and_dropping_nodes : concern_for_ModuleExplorerPresenter
   {
      protected bool _result;
      protected ITreeNode _dragNode;
      protected ITreeNode _targetNode;
      protected IBuildingBlock _dragBuildingBlock;
      private Module _targetModule;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _dragBuildingBlock = new MoBiSpatialStructure();
         _targetModule = new Module();
         _dragNode = new BuildingBlockNode(_dragBuildingBlock);
         _targetNode = new ModuleNode(new ClassifiableModule { Subject = _targetModule });
         _simulation = A.Fake<IMoBiSimulation>();
      }
   }

   public abstract class When_dragging_and_dropping_nodes_not_used_in_simulation : When_dragging_and_dropping_nodes
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.Uses(_dragBuildingBlock)).Returns(false);
         _context.CurrentProject.AddSimulation(_simulation);
      }

      protected override void Because()
      {
         _result = sut.CanDrop(_dragNode, _targetNode, GetKeyFlags());
      }

      protected abstract DragDropKeyFlags GetKeyFlags();
   }

   public abstract class When_dragging_and_dropping_nodes_used_in_simulation : When_dragging_and_dropping_nodes
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _simulation.Uses(_dragBuildingBlock)).Returns(true);
         _context.CurrentProject.AddSimulation(_simulation);
      }

      protected override void Because()
      {
         _result = sut.CanDrop(_dragNode, _targetNode, GetKeyFlags());
      }

      protected abstract DragDropKeyFlags GetKeyFlags();
   }

   public class When_copying_a_node_not_used_in_simulation : When_dragging_and_dropping_nodes_not_used_in_simulation
   {
      protected override DragDropKeyFlags GetKeyFlags()
      {
         return DragDropKeyFlags.LeftMouseButton | DragDropKeyFlags.CtrlKey;
      }

      [Observation]
      public void the_drag_drop_should_not_be_allowed()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_moving_a_node_not_used_in_simulation : When_dragging_and_dropping_nodes_not_used_in_simulation
   {
      protected override DragDropKeyFlags GetKeyFlags()
      {
         return DragDropKeyFlags.LeftMouseButton;
      }

      [Observation]
      public void the_drag_drop_should_not_be_allowed()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_copying_a_node_used_in_simulation : When_dragging_and_dropping_nodes_used_in_simulation
   {
      protected override DragDropKeyFlags GetKeyFlags()
      {
         return DragDropKeyFlags.LeftMouseButton | DragDropKeyFlags.CtrlKey;
      }

      [Observation]
      public void the_drag_drop_should_not_be_allowed()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_moving_a_node_used_in_simulation : When_dragging_and_dropping_nodes_used_in_simulation
   {
      protected override DragDropKeyFlags GetKeyFlags()
      {
         return DragDropKeyFlags.LeftMouseButton;
      }

      [Observation]
      public void the_drag_drop_should_not_be_allowed()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_removing_module_classification_nodes_and_data : concern_for_ModuleExplorerPresenter
   {
      private ITreeNode<IClassification> _classificationNode;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         var classifiableModule = new ClassifiableModule
         {
            Subject = _module
         };
         var classification = new Classification
         {
            ClassificationType = ClassificationType.Module
         };
         _classificationNode = new ClassificationNode(classification);
         _classificationNode.AddChild(new ModuleNode(classifiableModule));
      }

      protected override void Because()
      {
         sut.RemoveChildrenClassifications(_classificationNode, removeParent: true, removeData: true);
      }

      [Observation]
      public void the_interaction_task_is_used_to_remove_child_modules()
      {
         A.CallTo(() => _interactionTaskForModule.Remove(A<IReadOnlyList<Module>>.That.Contains(_module))).MustHaveHappened();
      }
   }

   public class When_removing_module_classification_without_module_children : concern_for_ModuleExplorerPresenter
   {
      private ITreeNode<IClassification> _classificationNode;

      protected override void Context()
      {
         base.Context();

         var classification = new Classification
         {
            ClassificationType = ClassificationType.Module
         };
         _classificationNode = new ClassificationNode(classification);
      }

      protected override void Because()
      {
         sut.RemoveChildrenClassifications(_classificationNode, removeParent: true, removeData: true);
      }

      [Observation]
      public void the_interaction_task_is_used_to_remove_child_modules()
      {
         A.CallTo(() => _interactionTaskForModule.Remove(A<IReadOnlyList<Module>>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_classification_should_be_removed()
      {
         A.CallTo(() => _classificationPresenter.RemoveClassification(_classificationNode)).MustHaveHappened();
      }
   }

   public class When_removing_module_classification_nodes_and_not_data : concern_for_ModuleExplorerPresenter
   {
      private ITreeNode<IClassification> _classificationNode;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module();
         var classifiableModule = new ClassifiableModule
         {
            Subject = _module
         };
         var classification = new Classification
         {
            ClassificationType = ClassificationType.Module
         };
         _classificationNode = new ClassificationNode(classification);
         _classificationNode.AddChild(new ModuleNode(classifiableModule));
      }

      protected override void Because()
      {
         sut.RemoveChildrenClassifications(_classificationNode, removeParent: true, removeData: false);
      }

      [Observation]
      public void the_interaction_task_is_used_to_remove_child_modules()
      {
         A.CallTo(() => _interactionTaskForModule.Remove(A<IReadOnlyList<Module>>.That.Contains(_module))).MustNotHaveHappened();
      }
   }
}