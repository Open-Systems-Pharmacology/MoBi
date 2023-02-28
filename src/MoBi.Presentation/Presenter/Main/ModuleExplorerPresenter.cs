using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
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
   public interface IModuleExplorerPresenter : IExplorerPresenter, IPresenter<IModuleExplorerView>
   {
      int OrderingComparisonFor(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2);
   }

   public class ModuleExplorerPresenter : ExplorerPresenter<IModuleExplorerView, IModuleExplorerPresenter>, IModuleExplorerPresenter, IListener<AddedEvent<Module>>
   {
      private readonly IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private readonly IEditBuildingBlockStarter _editBuildingBlockStarter;

      public ModuleExplorerPresenter(IModuleExplorerView view, IRegionResolver regionResolver, ITreeNodeFactory treeNodeFactory,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context, IClassificationPresenter classificationPresenter,
         IToolTipPartCreator toolTipPartCreator, IObservedDataInExplorerPresenter observedDataInExplorerPresenter,
         IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory, IProjectRetriever projectRetriever, IEditBuildingBlockStarter editBuildingBlockStarter) :
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

         return base.ContextMenuFor(treeNode);
      }

      public override void NodeDoubleClicked(ITreeNode node)
      {
         var moleculeBuilder = node.TagAsObject as IMoleculeBuilder;
         if (moleculeBuilder == null)
         {
            base.NodeDoubleClicked(node);
            return;
         }

         var moleculeBuildingBlock = node.ParentNode.TagAsObject.DowncastTo<IMoleculeBuildingBlock>();
         _editBuildingBlockStarter.EditMolecule(moleculeBuildingBlock, moleculeBuilder);
      }

      public void Handle(AddedEvent<Module> eventToHandle)
      {
         addModule(eventToHandle.AddedObject);
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
         if (nodeTagIsModuleRootNode(node1) && nodeTagIsModuleRootNode(node2))
            return rootNodeTypeComparison(node1, node2);

         if (nodeTagIsModuleRootNode(node1) && !nodeTagIsModuleRootNode(node2))
            return -1;

         if (!nodeTagIsModuleRootNode(node1) && nodeTagIsModuleRootNode(node2))
            return 1;

         if (nodeTagIsBuildingBlock(node1) && nodeTagIsBuildingBlock(node2))
            return 0;

         return nameComparison(node1, node2);
      }

      private int rootNodeTypeComparison(ITreeNode<IWithName> node1, ITreeNode<IWithName> node2)
      {
         if (node1.Tag.Equals(MoBiRootNodeTypes.ExtensionModulesFolder))
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
         var rootNodeList = new List<IWithName> { MoBiRootNodeTypes.ExtensionModulesFolder, MoBiRootNodeTypes.PKSimModuleFolder };
         return rootNodeList.Contains(node?.Tag);
      }

      public override IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode)
      {
         return _observedDataInExplorerPresenter.AvailableObservedDataCategoriesIn(parentClassificationNode);
      }

      public override void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category)
      {
         _observedDataInExplorerPresenter.GroupObservedDataByCategory(parentNode, category);
      }

      public override bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode)
      {
         return _observedDataInExplorerPresenter.RemoveObservedDataUnder(classificationNode);
      }

      private void addBuildingBlockToTree<TBuildingBlock>(TBuildingBlock buildingBlock, RootNodeType buildingBlockFolderType) where TBuildingBlock : IBuildingBlock
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

      protected override void AddProjectToTree(IMoBiProject project)
      {
         using (new BatchUpdate(_view))
         {
            _view.DestroyNodes();

            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.PKSimModuleFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ExtensionModulesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ExpressionProfilesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.IndividualsFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(RootNodeTypes.ObservedDataFolder));

            project.Modules.Each(addModule);

            project.ExpressionProfileCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.ExpressionProfilesFolder));
            project.IndividualsCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.IndividualsFolder));

            _observedDataInExplorerPresenter.AddObservedDataToTree(project);
         }
      }

      private void addModule(Module module)
      {
         var moduleNode = _view.AddNode(_treeNodeFactory.CreateFor(module).WithIcon(ApplicationIcons.Module).Under(_view.NodeByType(MoBiRootNodeTypes.ExtensionModulesFolder)));

         addBuildingBlockUnderNode(module.SpatialStructure, moduleNode);
         addBuildingBlockUnderNode(module.Molecule, moduleNode);
         addBuildingBlockUnderNode(module.Reaction, moduleNode);
         addBuildingBlockUnderNode(module.PassiveTransport, moduleNode);
         addBuildingBlockUnderNode(module.Observer, moduleNode);
         addBuildingBlockUnderNode(module.EventGroup, moduleNode);

         var moleculeStartValuesCollectionNode = collectionNodeFor(module.MoleculeStartValuesCollection, MoBiRootNodeTypes.MoleculeStartValuesFolder, moduleNode);
         var parameterStartValuesCollectionNode = collectionNodeFor(module.ParameterStartValuesCollection, MoBiRootNodeTypes.ParameterStartValuesFolder, moduleNode);

         module.MoleculeStartValuesCollection.Each(bb => addBuildingBlockUnderNode(bb, moleculeStartValuesCollectionNode));
         module.ParameterStartValuesCollection.Each(bb => addBuildingBlockUnderNode(bb, parameterStartValuesCollectionNode));
      }

      private ITreeNode collectionNodeFor<T>(IReadOnlyList<IStartValuesBuildingBlock<T>> startValueBlockCollection, RootNodeType rootNodeType, ITreeNode moduleNode) where T : class, IStartValue
      {
         return startValueBlockCollection.Count > 1 ? _view.AddNode(_treeNodeFactory.CreateFor(rootNodeType).Under(moduleNode)) : moduleNode;
      }
   }
}