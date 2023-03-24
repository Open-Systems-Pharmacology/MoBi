using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Presenter
{
   public class StartValueBuildingBlockSelectionPresenter<TBuildingBlock> : BaseBuildingBlockSelectionPresenter<TBuildingBlock>, IStartValuesSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      private readonly List<IBuildingBlock> _startValuesCollection = new List<IBuildingBlock>();

      public StartValueBuildingBlockSelectionPresenter(IBuildingBlockSelectionView view, IInteractionTasksForBuildingBlock<TBuildingBlock> interactionTasks, IMoBiContext context) : base(view, interactionTasks, context)
      {
      }

      public void SetAvailableStartValueBuildingBlocks(IReadOnlyCollection<TBuildingBlock> buildingBlocks)
      {
         _startValuesCollection.Clear();
         _startValuesCollection.AddRange(buildingBlocks);
         _editedBuildingBlock = null;
         Edit(_editedBuildingBlock);
      }

      public override IEnumerable<IBuildingBlock> AllAvailableBlocks => _startValuesCollection;
   }

   public interface IStartValuesSelectionPresenter<TBuildingBlock> : IBuildingBlockSelectionPresenter<TBuildingBlock> where TBuildingBlock : class, IBuildingBlock
   {
      void SetAvailableStartValueBuildingBlocks(IReadOnlyCollection<TBuildingBlock> buildingBlocks);
   }
}