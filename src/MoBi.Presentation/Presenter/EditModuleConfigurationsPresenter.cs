using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using MoBi.Presentation.Views;
using NPOI.OpenXmlFormats.Spreadsheet;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
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
      IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollectionFor(ITreeNode selectedNode);
      IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollectionFor(ITreeNode selectedNode);
      int CompareSelectedNodes(ITreeNode node1, ITreeNode node2);
      IReadOnlyList<ModuleConfiguration> ModuleConfigurations { get; }
      void UpdateStartValuesFor(ITreeNode selectedModuleConfigurationNode);
   }

   public class EditModuleConfigurationsPresenter : AbstractSubPresenter<IEditModuleConfigurationsView, IEditModuleConfigurationsPresenter>, IEditModuleConfigurationsPresenter
   {
      private readonly ITreeNodeFactory _treeNodeFactory;
      private readonly IMoBiContext _context;
      private readonly IModuleConfigurationToModuleConfigurationDTOMapper _moduleConfigurationDTOMapper;
      private List<ModuleConfigurationDTO> _moduleConfigurationsDTOs;

      public EditModuleConfigurationsPresenter(IEditModuleConfigurationsView view, ITreeNodeFactory treeNodeFactory, IMoBiContext context,
         IModuleConfigurationToModuleConfigurationDTOMapper moduleConfigurationDTOMapper) : base(view)
      {
         _treeNodeFactory = treeNodeFactory;
         _context = context;
         _moduleConfigurationDTOMapper = moduleConfigurationDTOMapper;
      }

      public void Edit(SimulationConfiguration simulationConfiguration)
      {
         _moduleConfigurationsDTOs = simulationConfiguration.ModuleConfigurations.MapAllUsing(_moduleConfigurationDTOMapper).ToList();

         addUnusedModulesToSelectionView();
         addUsedModuleConfigurationsToSelectedView(_moduleConfigurationsDTOs);
      }

      private void addUsedModuleConfigurationsToSelectedView(List<ModuleConfigurationDTO> simulationConfiguration)
      {
         simulationConfiguration.Each(addModuleConfigurationToSelectedView);
      }

      private void addUnusedModulesToSelectionView()
      {
         _context.CurrentProject.Modules.Where(module => !moduleInUse(module)).Each(addModuleToSelectionView);
      }

      private bool moduleInUse(Module module)
      {
         return _moduleConfigurationsDTOs.Any(x => x.Uses(module));
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
         _moduleConfigurationsDTOs.Add(moduleConfigurationDTO);
         addModuleConfigurationToSelectedView(moduleConfigurationDTO);
         _view.RemoveNodeFromSelectionView(selectedTreeNode);
      }

      private void addModuleConfigurationToSelectedView(ModuleConfigurationDTO moduleConfiguration)
      {
         _view.AddModuleConfigurationNode(_treeNodeFactory.CreateFor(moduleConfiguration));
      }

      public void RemoveModuleConfiguration(ITreeNode selectedTreeNode)
      {
         if (selectedTreeNode == null)
            return;

         var moduleConfigurationToRemove = moduleConfigurationDTOFor(selectedTreeNode);
         _moduleConfigurationsDTOs.Remove(moduleConfigurationToRemove);
         _view.RemoveNodeFromSelectedView(selectedTreeNode);

         addModuleToSelectionView(moduleConfigurationDTOFor(selectedTreeNode).Module);
      }

      public void SelectedModuleConfigurationNodeChanged(ITreeNode selectedTreeNode)
      {
         var dto = moduleConfigurationDTOFor(selectedTreeNode);
         _view.UnbindModuleConfiguration();

         // if the user selects a non-module node we want disable the binding
         if (dto == null)
         {
            _view.EnableRemove = false;
            return;
         }

         _view.EnableRemove = true;
         _view.BindModuleConfiguration(dto);
      }

      public void SelectedModuleNodeChanged(ITreeNode selectedNode)
      {
         var module = projectModuleFor(selectedNode);
         _view.EnableAdd = module != null;
      }

      private Module projectModuleFor(ITreeNode selectedNode) => selectedNode?.TagAsObject as Module;

      private ModuleConfigurationDTO moduleConfigurationDTOFor(ITreeNode treeNode) => treeNode?.TagAsObject as ModuleConfigurationDTO;

      public IReadOnlyList<MoleculeStartValuesBuildingBlock> MoleculeStartValuesCollectionFor(ITreeNode selectedNode)
      {
         if (selectedNode == null)
            return new List<MoleculeStartValuesBuildingBlock>();

         var dto = moduleConfigurationDTOFor(selectedNode);
         return dto.MoleculeStartValuesCollection;
      }

      public IReadOnlyList<ParameterStartValuesBuildingBlock> ParameterStartValuesCollectionFor(ITreeNode selectedNode)
      {
         if (selectedNode == null)
            return new List<ParameterStartValuesBuildingBlock>();

         var dto = moduleConfigurationDTOFor(selectedNode);
         return dto.ParameterStartValuesCollection;
      }

      public int CompareSelectedNodes(ITreeNode node1, ITreeNode node2)
      {
         var dto1 = moduleConfigurationDTOFor(node1);
         var dto2 = moduleConfigurationDTOFor(node2);

         return _moduleConfigurationsDTOs.IndexOf(dto1) - _moduleConfigurationsDTOs.IndexOf(dto2);
      }

      public IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurationsDTOs.Select(mapNewModuleConfiguration).ToList();

      private ModuleConfiguration mapNewModuleConfiguration(ModuleConfigurationDTO dto)
      {
         return new ModuleConfiguration(dto.Module, selectedMoleculeStartValues(dto), selectedParameterStartValues(dto));
      }

      private static ParameterStartValuesBuildingBlock selectedParameterStartValues(ModuleConfigurationDTO x)
      {
         return x.HasParameterStartValues() ? x.SelectedParameterStartValues : null;
      }

      private static MoleculeStartValuesBuildingBlock selectedMoleculeStartValues(ModuleConfigurationDTO x)
      {
         return x.HasMoleculeStartValues() ? x.SelectedMoleculeStartValues : null;
      }

      public void UpdateStartValuesFor(ITreeNode selectedModuleConfigurationNode)
      {
         var dto = moduleConfigurationDTOFor(selectedModuleConfigurationNode);

         updateStartValueIfRequired(selectedModuleConfigurationNode, dto.SelectedParameterStartValues, removeOnly: !dto.HasParameterStartValues());
         updateStartValueIfRequired(selectedModuleConfigurationNode, dto.SelectedMoleculeStartValues, removeOnly: !dto.HasMoleculeStartValues());
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

      public void MoveNode(ITreeNode dragNode, ITreeNode targetNode)
      {
         var movingConfiguration = moduleConfigurationDTOFor(dragNode);
         var targetConfiguration = moduleConfigurationDTOFor(targetNode);
         if (movingConfiguration == null || targetConfiguration == null)
            return;

         _moduleConfigurationsDTOs.Remove(movingConfiguration);

         var targetNodeIndex = _moduleConfigurationsDTOs.IndexOf(targetConfiguration);
         _moduleConfigurationsDTOs.Insert(targetNodeIndex, movingConfiguration);
         _view.SortSelectedModules();
      }
   }
}