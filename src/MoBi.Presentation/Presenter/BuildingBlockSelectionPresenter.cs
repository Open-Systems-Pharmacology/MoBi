using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IBuildingBlockSelectionPresenter : ICommandCollectorPresenter, IPresenter<IBuildingBlockSelectionView>
   {
      IEnumerable<IBuildingBlock> AllAvailableBlocks { get; }
      void SelectedBuildingBlockChanged();
      void CreateNew();
      string DisplayNameFor(IBuildingBlock buildingBlock);
   }

   public interface IBuildingBlockSelectionPresenter<T> : IBuildingBlockSelectionPresenter where T : class, IBuildingBlock
   {
      bool CanCreateBuildingBlock { set; }
      void Edit(IBuildingBlockInfo<T> buildingBlockInfo);
      event Action SelectionChanged;
      void UpdateBuildingBlock(IBuildingBlock buildingBlock);
   }

   public class BuildingBlockSelectionPresenter<TBuildingBlock> : AbstractCommandCollectorPresenter<IBuildingBlockSelectionView, IBuildingBlockSelectionPresenter>, IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IInteractionTasksForBuildingBlock<TBuildingBlock> _interactionTasks;
      private readonly IMoBiContext _context;
      private BuildingBlockSelectionDTO _buildingBlockSelectionDTO;
      private IBuildingBlockInfo<TBuildingBlock> _buildingBlockInfo;
      private TBuildingBlock _editedBuildingBlock;
      private TBuildingBlock _originalTemplateBuildingBlock;
      private uint _origialSimulationChanges;

      public event Action SelectionChanged = delegate { };

      public BuildingBlockSelectionPresenter(IBuildingBlockSelectionView view, IBuildingBlockRepository buildingBlockRepository,
         IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context)
         : base(view)
      {
         _buildingBlockRepository = buildingBlockRepository;
         _interactionTasks = interactionTasks;
         _context = context;
         _buildingBlockSelectionDTO = new BuildingBlockSelectionDTO();
      }

      public void UpdateBuildingBlock(IBuildingBlock buildingBlock)
      {
         if (!isTemplateBuildingBlock(buildingBlock)) return;
         refreshBuildingBlocks();
         selectBuildingBlock(buildingBlock);
      }

      public bool CanCreateBuildingBlock
      {
         set { View.NewVisible = value; }
      }

      public void Edit(IBuildingBlockInfo<TBuildingBlock> buildingBlockInfo)
      {
         _buildingBlockInfo = buildingBlockInfo;
         _editedBuildingBlock = buildingBlockInfo.BuildingBlock;
         _originalTemplateBuildingBlock = buildingBlockInfo.TemplateBuildingBlock;
         _origialSimulationChanges = buildingBlockInfo.SimulationChanges;
         updateSelectionWithBuildingBlock(_editedBuildingBlock ?? allAvailableTemplateBlocks.FirstOrDefault());
      }

      public IEnumerable<IBuildingBlock> AllAvailableBlocks
      {
         get
         {
            var allTemplateBuildingBlocks = allAvailableTemplateBlocks;

            //not a template building block 
            if (!editedBlockIsTemplate)
               yield return _editedBuildingBlock;

            foreach (var templateBuildingBlock in allTemplateBuildingBlocks.OrderBy(x => x.Name))
            {
               yield return templateBuildingBlock;
            }
         }
      }

      public void CreateNew()
      {
         //Directly add to context as this action should be added to history
         _context.AddToHistory(_interactionTasks.AddNew());
         updateSelectionWithBuildingBlock(allAvailableTemplateBlocks.LastOrDefault());
         SelectedBuildingBlockChanged();
      }

      public string DisplayNameFor(IBuildingBlock buildingBlock)
      {
         if (isTemplateBuildingBlock(buildingBlock))
            return buildingBlock.Name;

         return $"{buildingBlock.Name} - <color=red><B>{AppConstants.Warnings.ThisItNotATemplateBuildingBlock}</B></color>";
      }

      public void SelectedBuildingBlockChanged()
      {
         saveCurrentSelectionAsTemplateInBuildingBlockInfo();
         SelectionChanged();
      }

      private void refreshBuildingBlocks()
      {
         _view.RefreshElementList();
      }

      private void selectBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlockSelectionDTO.BuildingBlock = buildingBlock;
      }

      private TBuildingBlock selection => _buildingBlockSelectionDTO.BuildingBlock.DowncastTo<TBuildingBlock>();

      private void updateSelectionWithBuildingBlock(TBuildingBlock buildingBlock)
      {
         _buildingBlockSelectionDTO = new BuildingBlockSelectionDTO {BuildingBlock = buildingBlock};
         saveCurrentSelectionAsTemplateInBuildingBlockInfo();
         _view.BindTo(_buildingBlockSelectionDTO);
      }

      private IReadOnlyList<TBuildingBlock> allAvailableTemplateBlocks => _buildingBlockRepository.All<TBuildingBlock>().ToList();

      private void saveCurrentSelectionAsTemplateInBuildingBlockInfo()
      {
         if (isTemplateBuildingBlock(selection))
            updateBuildingBlockInfoWithTemplate(selection);
         else
            resetBuildingBlockInfo();

         _buildingBlockInfo.BuildingBlock = selection;
      }

      private void updateBuildingBlockInfoWithTemplate(TBuildingBlock templateBuildingBlock)
      {
         updateBuildingBlockInfo(templateBuildingBlock, simulationChanges:0);
      }

      private void resetBuildingBlockInfo()
      {
         updateBuildingBlockInfo(_originalTemplateBuildingBlock, _origialSimulationChanges);
      }

      private void updateBuildingBlockInfo(TBuildingBlock selectedBuildingBlock, uint simulationChanges)
      {
         _buildingBlockInfo.TemplateBuildingBlock = selectedBuildingBlock;
         _buildingBlockInfo.SimulationChanges = simulationChanges;
      }

      private bool editedBlockIsTemplate => _editedBuildingBlock == null || isTemplateBuildingBlock(_editedBuildingBlock);

      private bool isTemplateBuildingBlock(IBuildingBlock buildingBlock)
      {
         return allAvailableTemplateBlocks.Contains(buildingBlock);
      }
   }
}