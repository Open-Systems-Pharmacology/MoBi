using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IEditModuleConfigurationsPresenter : ISimulationConfigurationItemPresenter, ICanDragDropPresenter
   {
      void AddModuleConfiguration(ITreeNode selectedNode);
      void RemoveModuleConfiguration(ITreeNode selectedTreeNode);
      void SelectedModuleConfigurationNodeChanged(ITreeNode selectedTreeNode);
      void SelectedModuleNodeChanged(ITreeNode selectedNode);
      IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollectionFor(ITreeNode selectedNode);
      IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollectionFor(ITreeNode selectedNode);
      int CompareSelectedNodes(ITreeNode node1, ITreeNode node2);
      IReadOnlyList<ModuleConfigurationDTO> ModuleConfigurationDTOs { get; }
      void UpdateStartValuesFor(ITreeNode selectedModuleConfigurationNode);
      void MoveUp(ITreeNode selectedNode);
      void MoveDown(ITreeNode selectedNode);
   }

   public class EditModuleConfigurationsPresenter : AbstractSubPresenter<IEditModuleConfigurationsView, IEditModuleConfigurationsPresenter>, IEditModuleConfigurationsPresenter
   {
      private readonly ITreeNodeFactory _treeNodeFactory;
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IModuleConfigurationToModuleConfigurationDTOMapper _moduleConfigurationDTOMapper;
      private List<ModuleConfigurationDTO> _moduleConfigurationDTOs;

      public EditModuleConfigurationsPresenter(IEditModuleConfigurationsView view, ITreeNodeFactory treeNodeFactory, IMoBiProjectRetriever projectRetriever,
         IModuleConfigurationToModuleConfigurationDTOMapper moduleConfigurationDTOMapper) : base(view)
      {
         _treeNodeFactory = treeNodeFactory;
         _projectRetriever = projectRetriever;
         _moduleConfigurationDTOMapper = moduleConfigurationDTOMapper;
      }

      public override bool CanClose => ModuleConfigurationDTOs.Any();

      public void Edit(SimulationConfiguration simulationConfiguration)
      {
         _moduleConfigurationDTOs = simulationConfiguration.ModuleConfigurations.MapAllUsing(_moduleConfigurationDTOMapper).ToList();

         addUnusedModulesToSelectionView();
         addUsedModuleConfigurationsToSelectedView(_moduleConfigurationDTOs);
      }

      private void addUsedModuleConfigurationsToSelectedView(List<ModuleConfigurationDTO> moduleConfigurations)
      {
         moduleConfigurations.Each(addModuleConfigurationToSelectedView);
      }

      private void addUnusedModulesToSelectionView()
      {
         _projectRetriever.Current.Modules.Where(module => !moduleInUse(module)).Each(addModuleToSelectionView);
      }

      private bool moduleInUse(Module module)
      {
         return _moduleConfigurationDTOs.Any(x => x.Uses(module));
      }

      private void addModuleToSelectionView(Module module)
      {
         _view.AddModuleNode(_treeNodeFactory.CreateFor(module));
      }

      public void AddModuleConfiguration(ITreeNode selectedTreeNode)
      {
         var selectedModule = projectModuleFor(selectedTreeNode);
         if (selectedModule == null)
            return;

         var moduleConfiguration = new ModuleConfiguration(selectedModule);
         var moduleConfigurationDTO = _moduleConfigurationDTOMapper.MapFrom(moduleConfiguration);
         _moduleConfigurationDTOs.Add(moduleConfigurationDTO);
         addModuleConfigurationToSelectedView(moduleConfigurationDTO);
         _view.RemoveNodeFromSelectionView(selectedTreeNode);

         // We are informing the parent presenter that something has changed in the view
         // In this case, the addition/removal of configuration means we could/or could not advance
         // to the next step
         ViewChanged();
      }

      private void addModuleConfigurationToSelectedView(ModuleConfigurationDTO moduleConfiguration)
      {
         var nodeToAdd = _treeNodeFactory.CreateFor(moduleConfiguration);
         _view.AddModuleConfigurationNode(nodeToAdd);
      }

      public void RemoveModuleConfiguration(ITreeNode selectedTreeNode)
      {
         if (selectedTreeNode == null)
            return;

         var moduleConfigurationToRemove = moduleConfigurationDTOFor(selectedTreeNode);
         _moduleConfigurationDTOs.Remove(moduleConfigurationToRemove);
         _view.RemoveNodeFromSelectedView(selectedTreeNode);

         addModuleToSelectionView(moduleConfigurationDTOFor(selectedTreeNode).Module);

         // We are informing the parent presenter that something has changed in the view
         // In this case, the addition/removal of configuration means we could/or could not advance
         // to the next step
         ViewChanged();
      }

      public void SelectedModuleConfigurationNodeChanged(ITreeNode selectedTreeNode)
      {
         var dto = moduleConfigurationDTOFor(selectedTreeNode);
         _view.UnbindModuleConfiguration();

         // if the user selects a non-module node we want disable the binding
         if (dto == null)
         {
            _view.EnableRemove = false;
            _view.EnableUp = false;
            _view.EnableDown = false;
            return;
         }

         enableUpDownArrows(selectedTreeNode);

         _view.EnableRemove = true;
         _view.BindModuleConfiguration(dto);
      }

      private void enableUpDownArrows(ITreeNode selectedTreeNode)
      {
         _view.EnableUp = canMoveConfigurationUpByOne(selectedTreeNode);
         _view.EnableDown = canMoveConfigurationDownByOne(selectedTreeNode);
      }

      public void SelectedModuleNodeChanged(ITreeNode selectedNode)
      {
         var module = projectModuleFor(selectedNode);
         _view.EnableAdd = module != null;
      }

      private Module projectModuleFor(ITreeNode selectedNode) => selectedNode?.TagAsObject as Module;

      private ModuleConfigurationDTO moduleConfigurationDTOFor(ITreeNode treeNode) => treeNode?.TagAsObject as ModuleConfigurationDTO;

      public IReadOnlyList<InitialConditionsBuildingBlock> InitialConditionsCollectionFor(ITreeNode selectedNode)
      {
         if (selectedNode == null)
            return new List<InitialConditionsBuildingBlock>();

         var dto = moduleConfigurationDTOFor(selectedNode);
         return dto.InitialConditionsCollection;
      }

      public IReadOnlyList<ParameterValuesBuildingBlock> ParameterValuesCollectionFor(ITreeNode selectedNode)
      {
         if (selectedNode == null)
            return new List<ParameterValuesBuildingBlock>();

         var dto = moduleConfigurationDTOFor(selectedNode);
         return dto.ParameterValuesCollection;
      }

      public int CompareSelectedNodes(ITreeNode node1, ITreeNode node2)
      {
         var dto1 = moduleConfigurationDTOFor(node1);
         var dto2 = moduleConfigurationDTOFor(node2);

         return _moduleConfigurationDTOs.IndexOf(dto1) - _moduleConfigurationDTOs.IndexOf(dto2);
      }

      public IReadOnlyList<ModuleConfigurationDTO> ModuleConfigurationDTOs => _moduleConfigurationDTOs;

      public void UpdateStartValuesFor(ITreeNode selectedModuleConfigurationNode)
      {
         var dto = moduleConfigurationDTOFor(selectedModuleConfigurationNode);

         updateStartValueIfRequired(selectedModuleConfigurationNode, dto.SelectedParameterValues, removeOnly: !dto.HasParameterValues);
         updateStartValueIfRequired(selectedModuleConfigurationNode, dto.SelectedInitialConditions, removeOnly: !dto.HasInitialConditions);
      }

      private void updateStartValueIfRequired<TBuildingBlock>(ITreeNode selectedModuleConfigurationNode, TBuildingBlock buildingBlock, bool removeOnly) where TBuildingBlock : class, IBuildingBlock
      {
         var alreadyHasNode = selectedModuleConfigurationNode.AllLeafNodes.Any(x => Equals(x.TagAsObject, buildingBlock));
         if (alreadyHasNode)
            return;

         selectedModuleConfigurationNode.AllLeafNodes.Where(x => x.TagAsObject is TBuildingBlock).Each(x => removeNode(selectedModuleConfigurationNode, x));

         if (!removeOnly)
            _view.AddNodeToSelectedModuleConfigurations(_treeNodeFactory.CreateFor(buildingBlock).Under(selectedModuleConfigurationNode));
      }

      private void removeNode(ITreeNode parentNode, ITreeNode nodeToRemove)
      {
         parentNode.RemoveChild(nodeToRemove);
         _view.RemoveNodeFromSelectedView(nodeToRemove);
      }

      public bool CanDrag(ITreeNode node) => nodeIsModuleConfiguration(node);

      private bool nodeIsModuleConfiguration(ITreeNode node) => node?.TagAsObject is ModuleConfigurationDTO;

      public bool CanDrop(ITreeNode dragNode, ITreeNode targetNode) => nodeIsModuleConfiguration(targetNode) && !Equals(dragNode, targetNode);
      public void DropNode(ITreeNode dragNode, ITreeNode targetNode, DragDropKeyFlags keyState = DragDropKeyFlags.None)
      {
         var movingConfiguration = moduleConfigurationDTOFor(dragNode);
         var targetConfiguration = moduleConfigurationDTOFor(targetNode);
         if (movingConfiguration == null || targetConfiguration == null)
            return;

         moveConfiguration(movingConfiguration, targetConfiguration);
         refreshSelectedModulesView(dragNode);
      }

      public void MoveUp(ITreeNode selectedNode)
      {
         if (!canMoveConfigurationUpByOne(selectedNode))
            return;

         var selectedConfiguration = moduleConfigurationDTOFor(selectedNode);
         // -1 because we want to change the index by that amount
         var targetIndex = _moduleConfigurationDTOs.IndexOf(selectedConfiguration) - 1;
         _moduleConfigurationDTOs.Remove(selectedConfiguration);
         _moduleConfigurationDTOs.Insert(targetIndex, selectedConfiguration);
         refreshSelectedModulesView(selectedNode);
      }

      private bool canMoveConfigurationDownByOne(ITreeNode selectedNode)
      {
         return _moduleConfigurationDTOs.IndexOf(moduleConfigurationDTOFor(selectedNode)) < _moduleConfigurationDTOs.Count - 1;
      }

      private bool canMoveConfigurationUpByOne(ITreeNode selectedNode)
      {
         return _moduleConfigurationDTOs.IndexOf(moduleConfigurationDTOFor(selectedNode)) > 0;
      }

      public void MoveDown(ITreeNode selectedNode)
      {
         if (!canMoveConfigurationDownByOne(selectedNode))
            return;

         var selectedConfiguration = moduleConfigurationDTOFor(selectedNode);

         // +1 because we want to change the index by that amount
         var targetIndex = _moduleConfigurationDTOs.IndexOf(selectedConfiguration) + 1;
         _moduleConfigurationDTOs.Remove(selectedConfiguration);
         _moduleConfigurationDTOs.Insert(targetIndex, selectedConfiguration);
         refreshSelectedModulesView(selectedNode);
      }

      private void refreshSelectedModulesView(ITreeNode selectedNode)
      {
         _view.SortSelectedModules();
         enableUpDownArrows(selectedNode);
      }

      private void moveConfiguration(ModuleConfigurationDTO movingConfiguration, ModuleConfigurationDTO targetConfiguration)
      {
         _moduleConfigurationDTOs.Remove(movingConfiguration);

         var targetNodeIndex = _moduleConfigurationDTOs.IndexOf(targetConfiguration);
         _moduleConfigurationDTOs.Insert(targetNodeIndex, movingConfiguration);
      }
   }
}