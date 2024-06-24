using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
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
using Keys = OSPSuite.Presentation.Core.Keys;

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
      IListener<ModuleStatusChangedEvent>

   {
      private readonly IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private readonly IEditBuildingBlockStarter _editBuildingBlockStarter;
      private readonly IInteractionTasksForModule _interactionTaskForModule;

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

      public override bool CanDrop(ITreeNode targetNode, ITreeNode nodeToDrop)
      {
         var canDrop = base.CanDrop(targetNode, nodeToDrop);
         if (!canDrop)
         {
            //This checks if the node to drop is a building block and the target node is a module
            var buildingBlockSourceNode = targetNode as ITreeNode<IBuildingBlock>;
            var targetModuleNode = nodeToDrop as ModuleNode;

            if (targetModuleNode == null || buildingBlockSourceNode == null)
            {
               return false;
            }

            // This checks if the building block is already in the module, not by type but by ref. Meaning, it is his own module
            if (Equals(buildingBlockSourceNode.ParentNode, targetModuleNode))
               return false;

            // This checks if the building block is already in the module, by type
            if (!targetModuleNode.Tag.CanAdd(buildingBlockSourceNode.Tag))
               return false;

            return true;

         }
         return false;
      }

      public override void DropNode(ITreeNode dragNode, ITreeNode targetNode, DragDropKeyFlags keyState = DragDropKeyFlags.None)
      {
         var buildingBlockSourceNode = dragNode as ITreeNode<IBuildingBlock>;
         var targetModuleNode = targetNode as ModuleNode;

         if (buildingBlockSourceNode == null || targetModuleNode == null)
            return;

         var targetModule = targetModuleNode.Tag;
         var movingBuildingBlock = buildingBlockSourceNode.Tag;
         var sourceModule = movingBuildingBlock.Module;
         if(sourceModule == null)
            return;

         switch (keyState)
         {
            case DragDropKeyFlags.None:
               _interactionTaskForModule.MoveBuildingBlock(movingBuildingBlock, targetModule);
               break;
            case DragDropKeyFlags.CtrlKey:
               _interactionTaskForModule.CopyBuildingBlock(movingBuildingBlock, targetModule);
               break;
         }
         
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

         if(node.IsAnImplementationOf<BuildingBlockNode>())
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

      public void Handle(RemovedEvent eventToHandle)
      {
         RemoveNodesFor(eventToHandle.RemovedObjects);
         if(eventToHandle.Parent is Module module)
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
   }
}