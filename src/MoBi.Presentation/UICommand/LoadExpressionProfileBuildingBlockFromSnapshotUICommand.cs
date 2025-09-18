using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class LoadExpressionProfileBuildingBlockFromSnapshotUICommand : PKSimStarterSnapshotUICommand<ExpressionProfileBuildingBlock>
   {
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTasks;

      public LoadExpressionProfileBuildingBlockFromSnapshotUICommand(
         IMoBiProjectRetriever projectRetriever,
         IHeavyWorkManager heavyWorkManager,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTasks,
         IMoBiContext context) : base(projectRetriever, heavyWorkManager, context)
      {
         _interactionTasks = interactionTasks;
      }

      protected override IMoBiCommand AddToProject(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         return _interactionTasks.AddToProject(expressionProfileBuildingBlock);
      }

      protected override ExpressionProfileBuildingBlock LoadFromSnapshot()
      {
         return _interactionTasks.LoadFromSnapshot(Subject.Snapshot);
      }
   }
}