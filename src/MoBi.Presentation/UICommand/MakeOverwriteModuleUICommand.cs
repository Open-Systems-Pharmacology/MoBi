using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class MakeOverwriteModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModule _tasks;

      public MakeOverwriteModuleUICommand(IInteractionTasksForModule tasks)
      {
         _tasks = tasks;
      }

      protected override void PerformExecute() => _tasks.MakeOverwriteModule(Subject);
   }
}