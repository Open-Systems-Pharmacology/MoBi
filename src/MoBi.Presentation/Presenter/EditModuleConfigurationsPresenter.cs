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
      private List<ModuleConfigurationDTO> _moduleConfigurations;

      public EditModuleConfigurationsPresenter(IEditModuleConfigurationsView view, ITreeNodeFactory treeNodeFactory, IMoBiContext context,
         IModuleConfigurationToModuleConfigurationDTOMapper moduleConfigurationDTOMapper) : base(view)
      {
         _treeNodeFactory = treeNodeFactory;
         _context = context;
         _moduleConfigurationDTOMapper = moduleConfigurationDTOMapper;
      }

      public void Edit(SimulationConfiguration simulationConfiguration)
      {
         _moduleConfigurations = simulationConfiguration.ModuleConfigurations.Select(_moduleConfigurationDTOMapper.MapFrom).ToList();

         addUnusedModulesToSelectionView();
         addUsedModuleConfigurationsToSelectedView(simulationConfiguration);
      }

      private void addUsedModuleConfigurationsToSelectedView(SimulationConfiguration simulationConfiguration)
      {
         simulationConfiguration.ModuleConfigurations.Each(addModuleConfigurationToSelectedView);
      }

      private void addUnusedModulesToSelectionView()
      {
         _context.CurrentProject.Modules.Where(module => !moduleInUse(module)).Each(addModuleToSelectionView);
      }

      private bool moduleInUse(Module module)
      {
         return _moduleConfigurations.Any(x => x.Uses(module));
      }

      private ModuleConfigurationDTO dtoFor(ModuleConfiguration moduleConfiguration)
      {
         return _moduleConfigurations.FirstOrDefault(x => x.Uses(moduleConfiguration));
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
         _moduleConfigurations.Add(moduleConfigurationDTO);
         addModuleConfigurationToSelectedView(moduleConfiguration);
         _view.RemoveNodeFromSelectionView(selectedTreeNode);
      }

      private void addModuleConfigurationToSelectedView(ModuleConfiguration moduleConfiguration)
      {
         _view.AddModuleConfigurationNode(_treeNodeFactory.CreateFor(moduleConfiguration));
      }

      public void RemoveModuleConfiguration(ITreeNode selectedTreeNode)
      {
         if (selectedTreeNode == null)
            return;

         var moduleConfigurationToRemove = moduleConfigurationDTOFor(selectedTreeNode);
         _moduleConfigurations.Remove(moduleConfigurationToRemove);
         _view.RemoveNodeFromSelectedView(selectedTreeNode);
         addModuleToSelectionView(moduleConfigurationFor(selectedTreeNode).Module);
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

      private ModuleConfiguration moduleConfigurationFor(ITreeNode treeNode) => treeNode?.TagAsObject as ModuleConfiguration;

      private ModuleConfigurationDTO moduleConfigurationDTOFor(ITreeNode treeNode)
      {
         var moduleConfiguration = moduleConfigurationFor(treeNode);
         return dtoFor(moduleConfiguration);
      }

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

         return _moduleConfigurations.IndexOf(dto1) - _moduleConfigurations.IndexOf(dto2);
      }

      public IReadOnlyList<ModuleConfiguration> ModuleConfigurations => _moduleConfigurations.Select(x => x.ModuleConfiguration).ToList();
      
      public void UpdateStartValuesFor(ITreeNode selectedModuleConfigurationNode)
      {
         selectedModuleConfigurationNode.AllLeafNodes.Where(x => x.TagAsObject is ParameterStartValuesBuildingBlock).Each(x => removeNode(selectedModuleConfigurationNode, x));
         selectedModuleConfigurationNode.AllLeafNodes.Where(x => x.TagAsObject is MoleculeStartValuesBuildingBlock).Each(x => removeNode(selectedModuleConfigurationNode, x));

         _view.AddNodeToSelectedModuleConfigurations(_treeNodeFactory.CreateFor(moduleConfigurationDTOFor(selectedModuleConfigurationNode).SelectedMoleculeStartValues).Under(selectedModuleConfigurationNode));
         _view.AddNodeToSelectedModuleConfigurations(_treeNodeFactory.CreateFor(moduleConfigurationDTOFor(selectedModuleConfigurationNode).SelectedParameterStartValues).Under(selectedModuleConfigurationNode));
      }

      private void removeNode(ITreeNode parentNode, ITreeNode nodeToRemove)
      {
         parentNode.RemoveChild(nodeToRemove);
         _view.RemoveNodeFromSelectedView(nodeToRemove);
      }

      public bool CanDrag(ITreeNode node) => nodeIsModuleConfiguration(node);

      private bool nodeIsModuleConfiguration(ITreeNode node) => moduleConfigurationFor(node) != null;

      public bool CanDrop(ITreeNode dragNode, ITreeNode targetNode) => nodeIsModuleConfiguration(targetNode);

      public void MoveNode(ITreeNode dragNode, ITreeNode targetNode)
      {
         var movingConfiguration = moduleConfigurationDTOFor(dragNode);
         var targetConfiguration = moduleConfigurationDTOFor(targetNode);
         if (movingConfiguration == null || targetConfiguration == null)
            return;

         _moduleConfigurations.Remove(movingConfiguration);

         var targetNodeIndex = _moduleConfigurations.IndexOf(targetConfiguration);
         _moduleConfigurations.Insert(targetNodeIndex, movingConfiguration);
         _view.SortSelectedModules();
      }
   }
}