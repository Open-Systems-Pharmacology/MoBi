using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IModuleExplorerPresenter : IExplorerPresenter, IPresenter<IModuleExplorerView>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>
   {
      int OrderingComparisonFor(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2);
   }

   public class ModuleExplorerPresenter : ExplorerPresenter<IModuleExplorerView, IModuleExplorerPresenter>, IModuleExplorerPresenter,
      IListener<AddedEvent<Module>>,
      IListener<AddedEvent<IndividualBuildingBlock>>,
      IListener<AddedEvent<ExpressionProfileBuildingBlock>>,
      IListener<AddedEvent<MoleculeBuilder>>,
      IListener<ModuleStatusChangedEvent>,
      IListener<BulkUpdateStartedEvent>,
      IListener<BulkUpdateFinishedEvent>

   {
      private readonly IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private readonly IEditBuildingBlockStarter _editBuildingBlockStarter;
      private readonly IInteractionTasksForModule _interactionTaskForModule;
      private bool _editSinglesOnLoad = true;

      public ModuleExplorerPresenter(IModuleExplorerView view, IRegionResolver regionResolver, ITreeNodeFactory treeNodeFactory,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context, IClassificationPresenter classificationPresenter,
         IToolTipPartCreator toolTipPartCreator, IObservedDataInExplorerPresenter observedDataInExplorerPresenter,
         IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory, IProjectRetriever projectRetriever,
         IEditBuildingBlockStarter editBuildingBlockStarter, IInteractionTasksForModule interactionTaskForModule) :
         base(view, regionResolver, treeNodeFactory, viewItemContextMenuFactory, context, RegionNames.ModuleExplorer,
            classificationPresenter, toolTipPartCreator, multipleTreeNodeContextMenuFactory, projectRetriever)
      {
         _observedDataInExplorerPresenter = observedDataInExplorerPresenter;
         _observedDataInExplorerPresenter.InitializeWith(this, classificationPresenter, RootNodeTypes.ObservedDataFolder);
         _editBuildingBlockStarter = editBuildingBlockStarter;
         _interactionTaskForModule = interactionTaskForModule;
      }

      protected override IContextMenu ContextMenuFor(ITreeNode treeNode)
      {
         if (treeNode.TagAsObject is IBuildingBlock buildingBlock)
            return ContextMenuFor(new BuildingBlockViewItem(buildingBlock));

         if (treeNode.TagAsObject is ClassifiableObservedData observedData)
            return ContextMenuFor(new ObservedDataViewItem(observedData.Repository));

         if (treeNode.TagAsObject is Module module)
            return ContextMenuFor(new ModuleViewItem(module));

         return base.ContextMenuFor(treeNode);
      }

      public override bool CanDrop(ITreeNode nodeToDrop, ITreeNode targetNode)
      {
         if (base.CanDrop(nodeToDrop, targetNode))
            return true;

         var (targetModuleNode, buildingBlockSourceNode) = convertToExpectedTypes(nodeToDrop, targetNode);

         if (targetModuleNode == null || buildingBlockSourceNode == null)
            return false;

         if (moduleAlreadyContainsNode(buildingBlockSourceNode, targetModuleNode))
            return false;

         if (!isPossibleToAddBuildingBlockToModule(targetModuleNode, buildingBlockSourceNode))
            return false;

         return true;
      }

      private static bool isPossibleToAddBuildingBlockToModule(ModuleNode targetModuleNode, ITreeNode<IBuildingBlock> buildingBlockSourceNode) =>
         targetModuleNode.Tag.CanAdd(buildingBlockSourceNode.Tag);

      private static (ModuleNode moduleNode, ITreeNode<IBuildingBlock> buildingBlockSourceNode) convertToExpectedTypes(ITreeNode nodeToDrop, ITreeNode targetNode) =>
         (targetNode as ModuleNode, nodeToDrop as ITreeNode<IBuildingBlock>);

      private static bool moduleAlreadyContainsNode(ITreeNode<IBuildingBlock> buildingBlockSourceNode, ModuleNode targetModuleNode) =>
         Equals(buildingBlockSourceNode.ParentNode, targetModuleNode);

      public override void DropNode(ITreeNode nodeToDrop, ITreeNode targetNode, DragDropKeyFlags keyState = DragDropKeyFlags.None)
      {
         if (!handleDropNodeForBuildingBlock(nodeToDrop, targetNode, keyState))
            base.DropNode(nodeToDrop, targetNode, keyState);
      }

      private bool handleDropNodeForBuildingBlock(ITreeNode nodeToDrop, ITreeNode targetNode, DragDropKeyFlags keyState)
      {
         var (targetModuleNode, buildingBlockSourceNode) = convertToExpectedTypes(nodeToDrop, targetNode);

         if (buildingBlockSourceNode == null || targetModuleNode == null)
            return false;

         var movingBuildingBlock = buildingBlockSourceNode.Tag;
         if (movingBuildingBlock.Module == null)
            return false;

         var targetModule = targetModuleNode.Tag;
         switch (keyState)
         {
            case DragDropKeyFlags.None:
               _interactionTaskForModule.MoveBuildingBlock(movingBuildingBlock, targetModule);
               return true;
            case DragDropKeyFlags.CtrlKey:
               _interactionTaskForModule.CopyBuildingBlock(movingBuildingBlock, targetModule);
               return true;
         }

         return false;
      }

      protected override bool IsExpandable(ITreeNode node)
      {
         return base.IsExpandable(node) || node.IsAnImplementationOf<ParameterValuesFolderNode>() || node.IsAnImplementationOf<InitialConditionsFolderNode>();
      }

      public override void NodeDoubleClicked(ITreeNode node)
      {
         var moleculeBuilder = node.TagAsObject as MoleculeBuilder;
         if (moleculeBuilder == null)
         {
            base.NodeDoubleClicked(node);
            return;
         }

         var moleculeBuildingBlock = node.ParentNode.TagAsObject.DowncastTo<MoleculeBuildingBlock>();
         _editBuildingBlockStarter.EditMolecule(moleculeBuildingBlock, moleculeBuilder);
      }

      private void editSingleBuildingBlockModule(Module module)
      {
         if (module.BuildingBlocks.Count != 1)
            return;

         var buildingBlock = module.BuildingBlocks.First();
         NodeDoubleClicked(_view.NodeById(buildingBlock.Id));
      }

      private void expandNodes(IReadOnlyList<ITreeNode> treeNodes)
      {
         treeNodes.Each(_view.ExpandNode);
      }

      public void Handle(AddedEvent<Module> eventToHandle)
      {
         var moduleToAdd = eventToHandle.AddedObject;
         var addedNode = addModule(moduleToAdd);

         expandNodes(new List<ITreeNode>
         {
            addedNode,
            _view.NodeByType(MoBiRootNodeTypes.ModulesFolder)
         });

         if (_editSinglesOnLoad)
            editSingleBuildingBlockModule(moduleToAdd);
      }

      public void Handle(AddedEvent<IndividualBuildingBlock> eventToHandle)
      {
         addBuildingBlockToTree(eventToHandle.AddedObject, MoBiRootNodeTypes.IndividualsFolder);
      }

      public void Handle(AddedEvent<ExpressionProfileBuildingBlock> eventToHandle)
      {
         addBuildingBlockToTree(eventToHandle.AddedObject, MoBiRootNodeTypes.ExpressionProfilesFolder);
      }

      public override bool CanDrag(ITreeNode node)
      {
         if (node == null)
            return false;

         if (node.IsAnImplementationOf<ClassificationNode>())
            return true;

         if (node.IsAnImplementationOf<BuildingBlockNode>())
            return true;

         return _observedDataInExplorerPresenter.CanDrag(node);
      }

      public int OrderingComparisonFor(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2)
      {
         if (nodeIsStartValueFolderNode(node1) && nodeIsStartValueFolderNode(node2))
            return nodeIsInitialConditionsNode(node1) ? -1 : 1;

         if (nodeIsStartValueFolderNode(node1))
            return 1;

         if (nodeIsStartValueFolderNode(node2))
            return -1;

         if (nodeTagIsModuleRootNode(node1) && nodeTagIsModuleRootNode(node2))
            return rootNodeTypeComparison(node1);

         if (nodeTagIsModuleRootNode(node1) && !nodeTagIsModuleRootNode(node2))
            return -1;

         if (!nodeTagIsModuleRootNode(node1) && nodeTagIsModuleRootNode(node2))
            return 1;

         if (nodeTagIsBuildingBlock(node1) && nodeTagIsBuildingBlock(node2))
            return 0;

         return nameComparison(node1, node2);
      }

      private bool nodeIsStartValueFolderNode(ITreeNode<IWithName> node1)
      {
         return nodeIsParameterValuesNode(node1) || nodeIsInitialConditionsNode(node1);
      }

      private static bool nodeIsInitialConditionsNode(ITreeNode<IWithName> node1)
      {
         return node1 is InitialConditionsFolderNode;
      }

      private static bool nodeIsParameterValuesNode(ITreeNode<IWithName> node1)
      {
         return node1 is ParameterValuesFolderNode;
      }

      private int rootNodeTypeComparison(ITreeNode<IWithName> node1)
      {
         if (node1.Tag.Equals(MoBiRootNodeTypes.ModulesFolder))
            return 1;

         return -1;
      }

      private bool nodeTagIsBuildingBlock(ITreeNode<IWithName> node1)
      {
         return node1?.Tag is BuildingBlock;
      }

      private int nameComparison(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2)
      {
         if (node1 != null && node2 != null)
            return string.Compare(node1.Tag.Name, node2.Tag.Name, StringComparison.InvariantCultureIgnoreCase);

         return 0;
      }

      private static bool nodeTagIsModuleRootNode(ITreeNode<IWithName> node)
      {
         return Equals(node?.Tag, MoBiRootNodeTypes.ModulesFolder);
      }

      public override IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode) => _observedDataInExplorerPresenter.AvailableObservedDataCategoriesIn(parentClassificationNode);

      public override void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category)
      {
         _observedDataInExplorerPresenter.GroupObservedDataByCategory(parentNode, category);
      }

      public override bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode) => _observedDataInExplorerPresenter.RemoveObservedDataUnder(classificationNode);

      private void addBuildingBlockToTree<TBuildingBlock>(TBuildingBlock buildingBlock, RootNodeType buildingBlockFolderType)
         where TBuildingBlock : IBuildingBlock
      {
         var buildingBockFolderNode = _view.NodeByType(buildingBlockFolderType);

         addBuildingBlockUnderNode(buildingBlock, buildingBockFolderNode);
      }

      private void addBuildingBlockUnderNode<TBuildingBlock>(TBuildingBlock buildingBlock, ITreeNode folderNode) where TBuildingBlock : IBuildingBlock
      {
         if (buildingBlock == null)
            return;

         _view.AddNode(_treeNodeFactory.CreateFor(buildingBlock)
            .Under(folderNode));
      }

      protected override void AddProjectToTree(MoBiProject project)
      {
         using (new BatchUpdate(_view))
         {
            _view.DestroyNodes();

            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ModulesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ExpressionProfilesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.IndividualsFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(RootNodeTypes.ObservedDataFolder));

            project.Modules.Each(x => addModule(x));

            project.ExpressionProfileCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.ExpressionProfilesFolder));
            project.IndividualsCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.IndividualsFolder));

            _observedDataInExplorerPresenter.AddObservedDataToTree(project);
         }
      }

      private void addBuildingBlockToModule(IBuildingBlock buildingBlock, Module module)
      {
         var moduleNode = folderNodeForBuildingBlock(buildingBlock, module);

         addBuildingBlockUnderNode(buildingBlock, moduleNode);
      }

      private ITreeNode folderNodeForBuildingBlock(IBuildingBlock buildingBlock, Module module)
      {
         var nodeById = _view.TreeView.NodeById(module.Id);

         if (!(nodeById is ModuleNode moduleNode))
            return nodeById;

         switch (buildingBlock)
         {
            case ParameterValuesBuildingBlock _:
               return moduleNode.Children.OfType<ParameterValuesFolderNode>().FirstOrDefault() ?? _view.AddNode(_treeNodeFactory.ParameterValuesFolderNodeForModuleUnder(moduleNode));
            case InitialConditionsBuildingBlock _:
               return moduleNode.Children.OfType<InitialConditionsFolderNode>().FirstOrDefault() ?? _view.AddNode(_treeNodeFactory.InitialConditionsFolderNodeForModuleUnder(moduleNode));
         }

         return nodeById;
      }

      private ITreeNode addModule(Module module)
      {
         return _view.AddNode(_treeNodeFactory.CreateFor(module).Under(_view.NodeByType(MoBiRootNodeTypes.ModulesFolder)));
      }

      public void Handle(AddedEvent eventToHandle)
      {
         switch (eventToHandle.AddedObject)
         {
            case IBuildingBlock buildingBlock:
               var module = eventToHandle.Parent as Module;
               addBuildingBlockToModule(buildingBlock, module);
               refreshModuleIcon(module);
               break;
         }
      }

      public override void RemoveNodeFor(IWithId objectWithId)
      {
         var parentNode = NodeFor(objectWithId)?.ParentNode;
         base.RemoveNodeFor(objectWithId);
         switch (parentNode)
         {
            case InitialConditionsFolderNode initialConditionsFolderNode:
               {
                  if (!initialConditionsFolderNode.HasChildren)
                     RemoveNode(initialConditionsFolderNode);
                  break;
               }
            case ParameterValuesFolderNode parameterValuesFolderNode:
               {
                  if (!parameterValuesFolderNode.HasChildren)
                     RemoveNode(parameterValuesFolderNode);
                  break;
               }
         }
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         RemoveNodesFor(eventToHandle.RemovedObjects);
         if (eventToHandle.Parent is Module module)
            refreshModuleIcon(module);
      }

      public void Handle(ModuleStatusChangedEvent eventToHandle)
      {
         refreshModuleIcon(eventToHandle.Module);
      }

      private void refreshModuleIcon(Module module)
      {
         _view.NodeById(module.Id).Icon = ApplicationIcons.IconByName(module.Icon);
      }

      public void Handle(BulkUpdateStartedEvent eventToHandle)
      {
         _editSinglesOnLoad = false;
      }

      public void Handle(BulkUpdateFinishedEvent eventToHandle)
      {
         _editSinglesOnLoad = true;
      }

      private void addMoleculeBuilder(MoleculeBuilder moleculeBuilder, MoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculeBuildingBlockNode = _view.NodeById(moleculeBuildingBlock.Id);
         _view.AddNode(_treeNodeFactory.CreateFor(moleculeBuilder).Under(moleculeBuildingBlockNode));
      }


      public void Handle(AddedEvent<MoleculeBuilder> eventToHandle)
      {
         addMoleculeBuilder(eventToHandle.AddedObject, eventToHandle.Parent.DowncastTo<MoleculeBuildingBlock>());
      }
   }
}