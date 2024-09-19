using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class ExportBuildingBlockToExcelUICommand<T, TBuilder> : ObjectUICommand<T> where T : class, IBuildingBlock
   {
      private readonly IInteractionTasksForPathAndValueEntity<T, TBuilder> _interactionTasks;

      protected ExportBuildingBlockToExcelUICommand(
         IInteractionTasksForPathAndValueEntity<T, TBuilder> interactionTasks)
      {
         _interactionTasks = interactionTasks;
      }

      protected override void PerformExecute() => _interactionTasks.ExportToExcel(Subject);
   }
}