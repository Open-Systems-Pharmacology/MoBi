using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportModuleSnapshotUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModule _interactionTasksForModule;

      public ExportModuleSnapshotUICommand(IInteractionTasksForModule interactionTasksForModule)
      {
         _interactionTasksForModule = interactionTasksForModule;
      }
      protected override void PerformExecute()
      {
         _interactionTasksForModule.ExportModuleSnapshot(Subject);
      }
   }
}