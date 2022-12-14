using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter.Main
{
   public interface IBuildingBlockExplorerPresenter : IExplorerPresenter, IPresenter<IBuildingBlockExplorerView>,
      IListener<AddedEvent>,
      IListener<RemovedEvent>
   {
   }

   public class BuildingBlockExplorerPresenter : ExplorerPresenter<IBuildingBlockExplorerView, IBuildingBlockExplorerPresenter>, IBuildingBlockExplorerPresenter
   {
      private readonly IObservedDataInExplorerPresenter _observedDataInExplorerPresenter;
      private readonly IEditBuildingBlockStarter _editBuildingBlockStarter;

      public BuildingBlockExplorerPresenter(IBuildingBlockExplorerView view, IRegionResolver regionResolver, ITreeNodeFactory treeNodeFactory,
         IViewItemContextMenuFactory viewItemContextMenuFactory, IMoBiContext context, IClassificationPresenter classificationPresenter,
         IToolTipPartCreator toolTipPartCreator, IObservedDataInExplorerPresenter observedDataInExplorerPresenter,
         IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory, IProjectRetriever projectRetriever, IEditBuildingBlockStarter editBuildingBlockStarter)
         : base(view, regionResolver, treeNodeFactory, viewItemContextMenuFactory, context, RegionNames.BuildingBlockExplorer,
            classificationPresenter, toolTipPartCreator, multipleTreeNodeContextMenuFactory, projectRetriever)
      {
         _observedDataInExplorerPresenter = observedDataInExplorerPresenter;
         _editBuildingBlockStarter = editBuildingBlockStarter;
         _observedDataInExplorerPresenter.InitializeWith(this, classificationPresenter, RootNodeTypes.ObservedDataFolder);
      }

      protected override void AddProjectToTree(IMoBiProject project)
      {
         using (new BatchUpdate(_view))
         {
            _view.DestroyNodes();

            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.SpatialStructureFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.MoleculeFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ReactionFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.PassiveTransportFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ObserverFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.EventFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.SimulationSettingsFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.MoleculeStartValuesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ParameterStartValuesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.ExpressionProfilesFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(MoBiRootNodeTypes.IndividualsFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(RootNodeTypes.ObservedDataFolder));

            project.MoleculeBlockCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.MoleculeFolder));
            project.SpatialStructureCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.SpatialStructureFolder));
            project.PassiveTransportCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.PassiveTransportFolder));
            project.ReactionBlockCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.ReactionFolder));
            project.ObserverBlockCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.ObserverFolder));
            project.EventBlockCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.EventFolder));
            project.MoleculeStartValueBlockCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.MoleculeStartValuesFolder));
            project.ParametersStartValueBlockCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.ParameterStartValuesFolder));
            project.ExpressionProfileCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.ExpressionProfilesFolder));
            project.IndividualsCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.IndividualsFolder));

            project.SimulationSettingsCollection.Each(bb => addBuildingBlockToTree(bb, MoBiRootNodeTypes.SimulationSettingsFolder));

            _observedDataInExplorerPresenter.AddObservedDataToTree(project);
         }
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

      protected override IContextMenu ContextMenuFor(ITreeNode treeNode)
      {
         var buildingBlock = treeNode.TagAsObject as IBuildingBlock;
         if (buildingBlock != null)
            return ContextMenuFor(new BuildingBlockViewItem(buildingBlock));

         var observedData = treeNode.TagAsObject as ClassifiableObservedData;
         if (observedData != null)
            return ContextMenuFor(new ObservedDataViewItem(observedData.Repository));

         return base.ContextMenuFor(treeNode);
      }

      public override bool CanDrag(ITreeNode node)
      {
         if (node == null)
            return false;

         if (node.IsAnImplementationOf<ClassificationNode>())
            return true;

         return _observedDataInExplorerPresenter.CanDrag(node);
      }

      public void Handle(AddedEvent eventToHandle)
      {
         switch (eventToHandle.AddedObject)
         {
            case IBuildingBlock buildingBlock:
               addBuildingBlock(buildingBlock);
               break;
            case IMoleculeBuilder moleculeBuilder:
               addMoleculeBuilder(moleculeBuilder, eventToHandle.Parent.DowncastTo<IMoleculeBuildingBlock>());
               break;
         }
      }

      private ITreeNode addMoleculeBuilder(IMoleculeBuilder moleculeBuilder, IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         var moleculeBuildingBlockNode = _view.NodeById(moleculeBuildingBlock.Id);
         return _view.AddNode(_treeNodeFactory.CreateFor(moleculeBuilder)
            .Under(moleculeBuildingBlockNode));
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

      private ITreeNode addBuildingBlock(IBuildingBlock buildingBlock)
      {
         if (buildingBlock.IsAnImplementationOf<MoleculeBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.MoleculeFolder);

         if (buildingBlock.IsAnImplementationOf<SpatialStructure>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.SpatialStructureFolder);

         if (buildingBlock.IsAnImplementationOf<PassiveTransportBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.PassiveTransportFolder);

         if (buildingBlock.IsAnImplementationOf<ReactionBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.ReactionFolder);

         if (buildingBlock.IsAnImplementationOf<ObserverBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.ObserverFolder);

         if (buildingBlock.IsAnImplementationOf<EventGroupBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.EventFolder);

         if (buildingBlock.IsAnImplementationOf<MoleculeStartValuesBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.MoleculeStartValuesFolder);

         if (buildingBlock.IsAnImplementationOf<ParameterStartValuesBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.ParameterStartValuesFolder);

         if (buildingBlock.IsAnImplementationOf<SimulationSettings>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.SimulationSettingsFolder);

         if (buildingBlock.IsAnImplementationOf<ExpressionProfileBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.ExpressionProfilesFolder);

         if (buildingBlock.IsAnImplementationOf<IndividualBuildingBlock>())
            return addBuildingBlockToTree(buildingBlock, MoBiRootNodeTypes.IndividualsFolder);
         
         throw new ArgumentOutOfRangeException();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         RemoveNodesFor(eventToHandle.RemovedObjects);
      }

      private ITreeNode addBuildingBlockToTree<TBuildingBlock>(TBuildingBlock buildingBlock, RootNodeType buildingBlockFolderType) where TBuildingBlock : IBuildingBlock
      {
         var buildingBockFolderNode = _view.NodeByType(buildingBlockFolderType);
         return _view.AddNode(_treeNodeFactory.CreateFor(buildingBlock)
            .Under(buildingBockFolderNode));
      }
   }
}