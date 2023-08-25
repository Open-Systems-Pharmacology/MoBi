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

      private void addChartTreeNode(CurveChart chart)
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
         if (treeNode.TagAsObject is ClassifiableSimulation simulation)
            return ContextMenuFor(new SimulationViewItem(simulation.Simulation));

         if (treeNode.TagAsObject is IBuildingBlock buildingBlock)
            return ContextMenuFor(new SimulationBuildingBlockViewItem(buildingBlock));
         
         return base.ContextMenuFor(treeNode);
      }

      public void Handle(SimulationReloadEvent eventToHandle)
      {
         reCreateSimulationNode(eventToHandle.Simulation);
      }

      private void reCreateSimulationNode(IMoBiSimulation simulation)
      {
         var simulationNode = _view.NodeById(simulation.Id);
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
         
         var changedTemplateBuildingBlocks = findChangedTemplateBuildingBlocks(simulation).ToList();
         var changedModules = findChangedTemplateModules(simulation).ToList();

         var isChangedSimulation = changedTemplateBuildingBlocks.Any() || simulation.OriginalQuantityValues.Any();
         simulationNode.Icon = isChangedSimulation ? ApplicationIcons.SimulationRed : ApplicationIcons.SimulationGreen;

         updateModuleIcons(changedModules.Select(x => x.simulationModule), simulationNode.AllNodes.OfType<ModuleConfigurationNode>());
         updateBuildingBlockIcons(changedTemplateBuildingBlocks.Select(x => x.simulationBuildingBlock), simulationNode.AllNodes.OfType<BuildingBlockNode>());
      }

      private IEnumerable<(Module templateModule, Module simulationModule)> findChangedTemplateModules(IMoBiSimulation simulation)
      {
         return simulation.Configuration.ModuleConfigurations.Select(x => (templateModule: _interactionTasksForSimulation.TemplateModuleFor(x.Module), moduleConfiguration: x)).
            Where(match => !versionMatch(match)).Select(x => (templateModule: x.templateModule, simulationModule: x.moduleConfiguration.Module));
      }

      private bool versionMatch((Module templateModule, ModuleConfiguration moduleConfiguration) match)
      {
         var moduleConfiguration = match.moduleConfiguration;
         var simulationModule = moduleConfiguration.Module;
         var templateInitialConditions = moduleConfiguration.SelectedInitialConditions == null ? null : _interactionTasksForSimulation.TemplateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions);
         var templateParameterValues = moduleConfiguration.SelectedParameterValues == null ? null : _interactionTasksForSimulation.TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);

         return string.Equals(
            match.templateModule.VersionWith(templateParameterValues, templateInitialConditions), 
            simulationModule.VersionWith(moduleConfiguration.SelectedParameterValues, moduleConfiguration.SelectedInitialConditions));
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
         moduleConfigurationNode.Icon = changedModules.AllNames().Contains(moduleConfigurationNode.Text) ? ApplicationIcons.RedOverlayFor(moduleConfigurationNode.BaseIcon) : ApplicationIcons.GreenOverlayFor(moduleConfigurationNode.BaseIcon);
      }

      private static void updateBuildingBlockIcon(BuildingBlockNode buildingBlock, IEnumerable<IBuildingBlock> changedSimulationBuildingBlocks)
      {
         buildingBlock.Icon = changedSimulationBuildingBlocks.Contains(buildingBlock.Tag) ? ApplicationIcons.RedOverlayFor(buildingBlock.BaseIcon) : ApplicationIcons.GreenOverlayFor(buildingBlock.BaseIcon);
      }

      private IEnumerable<(IBuildingBlock templateBuildingBlock, IBuildingBlock simulationBuildingBlock)> findChangedTemplateBuildingBlocks(IMoBiSimulation simulation)
      {
         return simulation.BuildingBlocks().Select(x => (templateBuildingBlock: _interactionTasksForSimulation.TemplateBuildingBlockFor(x), simulationBuildingBlock:x)).
            Where(match => match.templateBuildingBlock.Version != match.simulationBuildingBlock.Version);
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