using System.Collections.Generic;
using MoBi.Presentation.Tasks.Interaction.MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveMultipleBuildingBlocksUICommand: ObjectUICommand<IReadOnlyList<IBuildingBlock>> 
   {
      private readonly IInteractionTasksForMultipleBuildingBlocks _interactionTasks;

      public RemoveMultipleBuildingBlocksUICommand(IInteractionTasksForMultipleBuildingBlocks interactionTasks)
      {
         _interactionTasks = interactionTasks;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.RemoveBuildingBlocks(Subject);
      }
   }
}