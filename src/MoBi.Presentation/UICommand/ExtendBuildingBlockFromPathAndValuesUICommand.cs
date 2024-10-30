using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class ExtendBuildingBlockFromPathAndValuesUICommand<TSource, TSourceParent, TTarget, TTargetBuilder> : ObjectUICommand<TTarget> where TSource : BuildingBlock where TTarget : PathAndValueEntityBuildingBlock<TTargetBuilder> where TTargetBuilder : PathAndValueEntity where TSourceParent : class
   {
      private readonly IInteractionTasksForBuildingBlock<TSourceParent, TSource> _interactionTasksForSource;
      private readonly IInteractionTasksForExtendablePathAndValueEntity<TTarget, TTargetBuilder> _taskForExtendingTarget;
      private readonly IMoBiContext _context;

      protected ExtendBuildingBlockFromPathAndValuesUICommand(
         IInteractionTasksForBuildingBlock<TSourceParent, TSource> interactionTasksForSource,
         IInteractionTasksForExtendablePathAndValueEntity<TTarget, TTargetBuilder> taskForExtendingTarget,
         IMoBiContext context)
      {
         _interactionTasksForSource = interactionTasksForSource;
         _taskForExtendingTarget = taskForExtendingTarget;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var buildingBlocks = _interactionTasksForSource.LoadFromPKML();
         _context.AddToHistory(_taskForExtendingTarget.Extend(MapAll(buildingBlocks), Subject));
      }

      protected abstract IReadOnlyList<TTargetBuilder> MapAll(IReadOnlyList<TSource> buildingBlocks);
   }
}