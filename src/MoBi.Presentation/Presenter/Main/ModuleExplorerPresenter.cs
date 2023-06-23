using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Views;
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
      IListener<AddedEvent<ExpressionProfileBuildingBlock>>
   {
      private readonly IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private readonly IEditBuildingBlockStarter _editBuildingBlockStarter;

      public ModuleExplorerPresenter(IModuleExplorerView view, IRegionResolver regionResolver, ITreeNodeFactory treeNodeFactory,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context, IClassificationPresenter classificationPresenter,
         IToolTipPartCreator toolTipPartCreator, IObservedDataInExplorerPresenter observedDataInExplorerPresenter,
         IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory, IProjectRetriever projectRetriever,
         IEditBuildingBlockStarter editBuildingBlockStarter) :
         base(view, regionResolver, treeNodeFactory, viewItemContextMenuFactory, context, RegionNames.ModuleExplorer,
            classificationPresenter, toolTipPartCreator, multipleTreeNodeContextMenuFactory, projectRetriever)
      {
         _observedDataInExplorerPresenter = observedDataInExplorerPresenter;
         _observedDataInExplorerPresenter.InitializeWith(this, classificationPresenter, RootNodeTypes.ObservedDataFolder);
         _editBuildingBlockStarter = editBuildingBlockStarter;
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

      public void Handle(AddedEvent<Module> eventToHandle)
      {
         addModule(eventToHandle.AddedObject);
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

            project.Modules.Each(addModule);

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
         var moduleNode = _view.TreeView.NodeById(module.Id);

         if (buildingBlock is ParameterValuesBuildingBlock)
            return moduleNode.Children.OfType<ParameterValuesFolderNode>().FirstOrDefault();

         if (buildingBlock is InitialConditionsBuildingBlock)
            return moduleNode.Children.OfType<InitialConditionsFolderNode>().FirstOrDefault();

         return moduleNode;
      }

      private void addModule(Module module)
      {
         _view.AddNode(_treeNodeFactory.CreateFor(module).Under(_view.NodeByType(MoBiRootNodeTypes.ModulesFolder)));
      }

      public void Handle(AddedEvent eventToHandle)
      {
         switch (eventToHandle.AddedObject)
         {
            case IBuildingBlock buildingBlock:
               addBuildingBlockToModule(buildingBlock, eventToHandle.Parent as Module);
               break;
         }
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         RemoveNodesFor(eventToHandle.RemovedObjects);
      }
   }
}