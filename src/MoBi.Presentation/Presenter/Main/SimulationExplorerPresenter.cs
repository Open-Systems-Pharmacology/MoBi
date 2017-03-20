using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;
using SimulationStatusChangedEvent = MoBi.Core.Events.SimulationStatusChangedEvent;

namespace MoBi.Presentation.Presenter.Main
{
   public interface ISimulationExplorerPresenter : IPresenter<ISimulationExplorerView>,
      IExplorerPresenter,
      IListener<SimulationAddedEvent>,
      IListener<SimulationRemovedEvent>,
      IListener<RemovedDataEvent>,
      IListener<ChartAddedEvent>,
      IListener<ChartDeletedEvent>,
      IListener<SimulationStatusChangedEvent>,
      IListener<SimulationReloadEvent>
   {
   }

   public class SimulationExplorerPresenter : ExplorerPresenter<ISimulationExplorerView, ISimulationExplorerPresenter>, ISimulationExplorerPresenter
   {
      private readonly IInteractionTasksForSimulation _interactionTasksForSimulation;
      private readonly IParameterAnalysablesInExplorerPresenter _parameterAnalysablesInExplorerPresenter;

      public SimulationExplorerPresenter(ISimulationExplorerView view, IRegionResolver regionResolver, ITreeNodeFactory treeNodeFactory, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IMoBiContext context, IClassificationPresenter classificationPresenter,
         IToolTipPartCreator toolTipPartCreator, IMultipleTreeNodeContextMenuFactory multipleTreeNodeContextMenuFactory,
         IProjectRetriever projectRetriever, IInteractionTasksForSimulation interactionTasksForSimulation, IParameterAnalysablesInExplorerPresenter parameterAnalysablesInExplorerPresenter) :
            base(view, regionResolver, treeNodeFactory, viewItemContextMenuFactory, context, RegionNames.SimulationExplorer,
               classificationPresenter, toolTipPartCreator, multipleTreeNodeContextMenuFactory, projectRetriever)
      {
         _interactionTasksForSimulation = interactionTasksForSimulation;
         _parameterAnalysablesInExplorerPresenter = parameterAnalysablesInExplorerPresenter;
         _parameterAnalysablesInExplorerPresenter.InitializeWith(this, classificationPresenter);
      }

      protected override void AddProjectToTree(IMoBiProject project)
      {
         using (new BatchUpdate(_view))
         {
            _view.DestroyNodes();

            //root nodes
            _view.AddNode(_treeNodeFactory.CreateFor(RootNodeTypes.SimulationFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(RootNodeTypes.ParameterIdentificationFolder));
            _view.AddNode(_treeNodeFactory.CreateFor(RootNodeTypes.SensitivityAnalysisFolder));

            //classifications
            _classificationPresenter.AddClassificationsToTree(project.AllClassificationsByType(ClassificationType.Simulation));

            //classifiables
            project.AllClassifiablesByType<ClassifiableSimulation>().Each(x => addClassifiableSimulationToSimulationRootFolder(x));

            _parameterAnalysablesInExplorerPresenter.AddParameterAnalysablesToTree(project);

            project.Charts.Each(addChartTreeNode);
         }
      }

      public override bool CanDrag(ITreeNode node)
      {
         if (node == null)
            return false;

         var tag = node.TagAsObject;
         if (tag.IsAnImplementationOf<DataRepository>())
            return true;

         return node.IsAnImplementationOf<SimulationNode>() ||
                node.IsAnImplementationOf<ClassificationNode>() ||
                _parameterAnalysablesInExplorerPresenter.CanDrag(node);
      }

      public override void NodeDoubleClicked(ITreeNode node)
      {
         if (node.IsAnImplementationOf<BuildingBlockInfoNode>())
            return;

         base.NodeDoubleClicked(node);
      }

      private ITreeNode addClassifiableSimulationToSimulationRootFolder(ClassifiableSimulation classifiableSimulation)
      {
         return AddClassifiableToTree(classifiableSimulation, RootNodeTypes.SimulationFolder, addClassifiableSimulationToTree);
      }

      private ITreeNode addClassifiableSimulationToTree(ITreeNode<IClassification> classificationNode, ClassifiableSimulation classifiableSimulation)
      {
         var simulationNode = _treeNodeFactory.CreateFor(classifiableSimulation);
         AddClassifiableNodeToView(simulationNode, classificationNode);
         refreshDisplayedSimulation(classifiableSimulation.Simulation);
         return simulationNode;
      }

      private void addChartTreeNode(ICurveChart chart)
      {
         _view.AddNode(_treeNodeFactory.CreateFor(chart));
      }

      public void Handle(SimulationAddedEvent eventToHandle)
      {
         addSimulationToTree(eventToHandle.Simulation);
      }

      private ITreeNode addSimulationToTree(IMoBiSimulation simulation)
      {
         return AddSubjectToClassifyToTree<IMoBiSimulation, ClassifiableSimulation>(simulation, addClassifiableSimulationToSimulationRootFolder);
      }

      public void Handle(SimulationRemovedEvent eventToHandle)
      {
         RemoveNodeFor(eventToHandle.Simulation);
      }

      public override void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         base.Handle(eventToHandle);
         reCreateSimulationNode(eventToHandle.Simulation);
      }

      public void Handle(RemovedDataEvent eventToHandle)
      {
         RemoveNodeFor(eventToHandle.Repository);
      }

      public void Handle(ChartAddedEvent eventToHandle)
      {
         addChartTreeNode(eventToHandle.Chart);
      }

      public void Handle(ChartDeletedEvent eventToHandle)
      {
         RemoveNodeFor(eventToHandle.Chart);
      }

      public void Handle(SimulationStatusChangedEvent eventToHandle)
      {
         refreshDisplayedSimulation(eventToHandle.Simulation);
      }

      protected override IContextMenu ContextMenuFor(ITreeNode treeNode)
      {
         var simulation = treeNode.TagAsObject as ClassifiableSimulation;
         if (simulation != null)
            return ContextMenuFor(new SimulationViewItem(simulation.Simulation));

         var buildingBlockInfo = treeNode.TagAsObject as IBuildingBlockInfo;
         if (buildingBlockInfo != null)
         {
            var simulationNode = parentSimulationNodeFor(treeNode);
            return ContextMenuFor(new BuildingBlockInfoViewItem(buildingBlockInfo, simulationNode.Simulation));
         }

         return base.ContextMenuFor(treeNode);
      }

      private SimulationNode parentSimulationNodeFor(ITreeNode treeNode)
      {
         var simulationNode = treeNode as SimulationNode;
         if (simulationNode != null)
            return simulationNode;

         return parentSimulationNodeFor(treeNode.ParentNode);
      }

      public void Handle(SimulationReloadEvent eventToHandle)
      {
         reCreateSimulationNode(eventToHandle.Simulation);
      }

      private void reCreateSimulationNode(IMoBiSimulation simulation)
      {
         var simulationNode = _view.NodeById(simulation.Id);
         var configurationNode = simulationConfigurationNodeUnder(simulationNode);
         bool simulationNodeExpanded = _view.IsNodeExpanded(simulationNode);
         bool configurationNodeExpanded = _view.IsNodeExpanded(configurationNode);
         var parentNode = simulationNode.ParentNode.DowncastTo<ITreeNode<IClassification>>();
         RemoveNodeFor(simulation);
         var classifiableSimulation = _projectRetriever.CurrentProject.GetOrCreateClassifiableFor<ClassifiableSimulation, IMoBiSimulation>(simulation);
         simulationNode = addClassifiableSimulationToTree(parentNode, classifiableSimulation);
         configurationNode = simulationConfigurationNodeUnder(simulationNode);
         _view.ExpandNodeIfRequired(simulationNode, simulationNodeExpanded);
         _view.ExpandNodeIfRequired(configurationNode, configurationNodeExpanded);
      }

      private void refreshDisplayedSimulation(IMoBiSimulation simulation)
      {
         var simulationNode = _view.NodeById(simulation.Id);

         //Update Simulation Icon
         var isChangedSimulation = simulation.MoBiBuildConfiguration.HasChangedBuildingBlocks();
         simulationNode.Icon = isChangedSimulation ? ApplicationIcons.SimulationRed : ApplicationIcons.SimulationGreen;

         // Update Building block
         simulationConfigurationNodeUnder(simulationNode).Children.Each(refreshDisplayedBuildingBlock);
      }

      private ITreeNode simulationConfigurationNodeUnder(ITreeNode simulationNode)
      {
         return simulationNode.Children<BuildConfigurationNode>().First();
      }

      private void refreshDisplayedBuildingBlock(ITreeNode treeNode)
      {
         var buildingBlockInfo = treeNode.TagAsObject as IBuildingBlockInfo;
         if (buildingBlockInfo == null) return;

         //Update TreeNode Icon
         var icon = buildingBlockInfo.BuildingBlockChanged ? ApplicationIcons.RedOverlayFor(buildingBlockInfo.IconName) : ApplicationIcons.GreenOverlayFor(buildingBlockInfo.IconName);
         treeNode.Icon = icon;
      }

      public override IEnumerable<ClassificationTemplate> AvailableClassificationCategories(ITreeNode<IClassification> parentClassificationNode)
      {
         return Enumerable.Empty<ClassificationTemplate>();
      }

      public override void AddToClassificationTree(ITreeNode<IClassification> parentNode, string category)
      {
         /*no category for now*/
      }

      public override bool RemoveDataUnderClassification(ITreeNode<IClassification> classificationNode)
      {
         if (classificationNode.Tag.ClassificationType == ClassificationType.Simulation)
         {
            IReadOnlyList<IMoBiSimulation> allSimulations = classificationNode.AllNodes<SimulationNode>().Select(x => x.Tag.Simulation).ToList();
            var command = _interactionTasksForSimulation.RemoveMultipleSimulations(allSimulations);
            _context.AddToHistory(command);
            return !command.IsEmpty();
         }

         return _parameterAnalysablesInExplorerPresenter.RemoveDataUnderClassification(classificationNode);
      }
   }
}