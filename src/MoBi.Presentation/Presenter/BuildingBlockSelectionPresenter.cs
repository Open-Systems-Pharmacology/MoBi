using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
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
      T SelectedBuildingBlock { get; }
      void Edit(T buildingBlock);
      event Action SelectionChanged;
      void UpdateBuildingBlock(IBuildingBlock buildingBlock);
   }

   public class BuildingBlockSelectionPresenter<TBuildingBlock> : BaseBuildingBlockSelectionPresenter<TBuildingBlock>, IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public BuildingBlockSelectionPresenter(IBuildingBlockSelectionView view, IBuildingBlockRepository buildingBlockRepository,
         IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context) : base(view, interactionTasks, context)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public override IEnumerable<IBuildingBlock> AllAvailableBlocks => _buildingBlockRepository.All<TBuildingBlock>();
   }
}