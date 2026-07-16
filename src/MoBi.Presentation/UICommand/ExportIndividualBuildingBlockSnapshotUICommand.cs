using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportIndividualBuildingBlockSnapshotUICommand : ObjectUICommand<IndividualBuildingBlock>
   {
      private readonly IInteractionTasksForIndividualBuildingBlock _interactionTasksForIndividualBuildingBlock;

      public ExportIndividualBuildingBlockSnapshotUICommand(IInteractionTasksForIndividualBuildingBlock interactionTasksForIndividualBuildingBlock)
      {
         _interactionTasksForIndividualBuildingBlock = interactionTasksForIndividualBuildingBlock;
      }

      protected override void PerformExecute() =>
         _interactionTasksForIndividualBuildingBlock.ExportBuildingBlockSnapshot(Subject);
   }
}