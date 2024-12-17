using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
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

      protected override void AddProjectToTree(MoBiProject project)
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

      public override bool CopyAllowed() => false;

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

      private void addChartTreeNode(CurveChart chart) => _view.AddNode(_treeNodeFactory.CreateFor(chart));

      public void Handle(SimulationAddedEvent eventToHandle) => addSimulationToTree(eventToHandle.Simulation);

      private void addSimulationToTree(IMoBiSimulation simulation)
      {
         AddSubjectToClassifyToTree<IMoBiSimulation, ClassifiableSimulation>(simulation, addClassifiableSimulationToSimulationRootFolder);
      }

      public void Handle(SimulationRemovedEvent eventToHandle) => RemoveNodeFor(eventToHandle.Simulation);

      public override void Handle(SimulationRunFinishedEvent eventToHandle)
      {
         base.Handle(eventToHandle);
         reCreateSimulationNode(eventToHandle.Simulation);
      }

      public void Handle(RemovedDataEvent eventToHandle) => RemoveNodeFor(eventToHandle.Repository);

      public void Handle(ChartAddedEvent eventToHandle) => addChartTreeNode(eventToHandle.Chart);

      public void Handle(ChartDeletedEvent eventToHandle) => RemoveNodeFor(eventToHandle.Chart);
      
      public void Handle(SimulationStatusChangedEvent eventToHandle) => refreshDisplayedSimulation(eventToHandle.Simulation);

      protected override IContextMenu ContextMenuFor(ITreeNode treeNode)
      {
         if (treeNode.TagAsObject is ClassifiableSimulation simulation)
            return ContextMenuFor(new SimulationViewItem(simulation.Simulation));

         // Order is important here because SimulationSettings is also an IBuildingBlock
         if(treeNode.TagAsObject is SimulationSettingsDTO settingsDTO)
            return ContextMenuFor(settingsDTO);

         if (treeNode.TagAsObject is IBuildingBlock buildingBlock)
            return ContextMenuFor(new SimulationBuildingBlockViewItem(buildingBlock));

         return base.ContextMenuFor(treeNode);
      }

      public void Handle(SimulationReloadEvent eventToHandle) => reCreateSimulationNode(eventToHandle.Simulation);

      private void reCreateSimulationNode(IMoBiSimulation simulation)
      {
         var simulationNode = _view.NodeById(simulation.Id);

         // In case of a cloned simulation, the reload event will be published by the command
         // before the simulation is added to the project
         if (simulationNode == null)
            return;

         bool simulationNodeExpanded = _view.IsNodeExpanded(simulationNode);
         var parentNode = simulationNode.ParentNode.DowncastTo<ITreeNode<IClassification>>();
         RemoveNodeFor(simulation);
         var classifiableSimulation = _projectRetriever.CurrentProject.GetOrCreateClassifiableFor<ClassifiableSimulation, IMoBiSimulation>(simulation);
         simulationNode = addClassifiableSimulationToTree(parentNode, classifiableSimulation);
         _view.ExpandNodeIfRequired(simulationNode, simulationNodeExpanded);
      }

      private void refreshDisplayedSimulation(IMoBiSimulation simulation)
      {
         var simulationNode = _view.NodeById(simulation.Id);

         var changedTemplateBuildingBlocks = _interactionTasksForSimulation.FindChangedBuildingBlocks(simulation).ToList();
         var changedModules = _interactionTasksForSimulation.FindChangedModules(simulation).ToList();

         // Set simulation to red if there are any module, building block, or quantity changes
         simulationNode.Icon = changedModules.Any() || changedTemplateBuildingBlocks.Any() || simulation.OriginalQuantityValues.Any() ? ApplicationIcons.SimulationRed : ApplicationIcons.SimulationGreen;

         updateModuleIcons(changedModules, simulationNode.AllNodes.OfType<ModuleConfigurationNode>());
         updateBuildingBlockIcons(changedTemplateBuildingBlocks, simulationNode.AllNodes.OfType<BuildingBlockNode>());
      }

      private void updateModuleIcons(IEnumerable<Module> changedModules, IEnumerable<ModuleConfigurationNode> simulationNodeChildren)
      {
         simulationNodeChildren.Each(x => updateModuleIcon(x, changedModules));
      }

      private void updateBuildingBlockIcons(IEnumerable<IBuildingBlock> changedSimulationBuildingBlocks, IEnumerable<BuildingBlockNode> simulationNodeChildren)
      {
         simulationNodeChildren.Each(x => updateBuildingBlockIcon(x, changedSimulationBuildingBlocks));
      }

      private void updateModuleIcon(ModuleConfigurationNode moduleConfigurationNode, IEnumerable<Module> changedModules)
      {
         moduleConfigurationNode.Icon = changedModules.AllNames().Contains(moduleConfigurationNode.ModuleName) ? ApplicationIcons.RedOverlayFor(moduleConfigurationNode.BaseIcon) : ApplicationIcons.GreenOverlayFor(moduleConfigurationNode.BaseIcon);
      }

      private static void updateBuildingBlockIcon(BuildingBlockNode buildingBlock, IEnumerable<IBuildingBlock> changedSimulationBuildingBlocks)
      {
         buildingBlock.Icon = changedSimulationBuildingBlocks.Contains(buildingBlock.Tag) ? ApplicationIcons.RedOverlayFor(buildingBlock.BaseIcon) : ApplicationIcons.GreenOverlayFor(buildingBlock.BaseIcon);
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