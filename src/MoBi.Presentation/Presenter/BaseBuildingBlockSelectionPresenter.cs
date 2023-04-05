using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public abstract class BaseBuildingBlockSelectionPresenter<TBuildingBlock> : AbstractCommandCollectorPresenter<IBuildingBlockSelectionView, IBuildingBlockSelectionPresenter>,
      IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly IInteractionTasksForBuildingBlock<TBuildingBlock> _interactionTasks;
      private readonly IMoBiContext _context;
      private BuildingBlockSelectionDTO _buildingBlockSelectionDTO;
      protected TBuildingBlock _editedBuildingBlock;

      public event Action SelectionChanged = delegate { };

      protected BaseBuildingBlockSelectionPresenter(IBuildingBlockSelectionView view,
         IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context)
         : base(view)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _buildingBlockSelectionDTO = new BuildingBlockSelectionDTO();
      }

      public void UpdateBuildingBlock(IBuildingBlock buildingBlock)
      {
         refreshBuildingBlocks();
         selectBuildingBlock(buildingBlock);
      }

      public bool CanCreateBuildingBlock
      {
         set => View.NewVisible = value;
      }

      public TBuildingBlock SelectedBuildingBlock => _buildingBlockSelectionDTO.SelectedObject.DowncastTo<TBuildingBlock>();

      public void Edit(TBuildingBlock buildingBlock)
      {
         _editedBuildingBlock = buildingBlock;
         updateSelectionWithBuildingBlock(_editedBuildingBlock ?? AllAvailableBlocks.FirstOrDefault());
      }

      public abstract IEnumerable<IBuildingBlock> AllAvailableBlocks { get; }

      public void CreateNew()
      {
         //Directly add to context as this action should be added to history
         _context.AddToHistory(_interactionTasks.AddNew());
         updateSelectionWithBuildingBlock(AllAvailableBlocks.LastOrDefault());
         SelectedBuildingBlockChanged();
      }

      public string DisplayNameFor(IBuildingBlock buildingBlock)
      {
         return buildingBlock.Name;
      }

      public void SelectedBuildingBlockChanged()
      {
         SelectionChanged();
      }

      private void refreshBuildingBlocks()
      {
         _view.RefreshElementList();
      }

      private void selectBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlockSelectionDTO.SelectedObject = buildingBlock;
      }

      private void updateSelectionWithBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlockSelectionDTO = new BuildingBlockSelectionDTO { SelectedObject = buildingBlock };
         _view.BindTo(_buildingBlockSelectionDTO);
      }
   }
}