using System.Collections.Generic;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Tasks.Interaction.MoBi.Presentation.Tasks.Interaction;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveMultipleBuildingBlocksUICommand : ObjectUICommand<IReadOnlyList<IBuildingBlock>>
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