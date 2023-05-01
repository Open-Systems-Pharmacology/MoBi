using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class CloneProjectBuildingBlockUICommand<T> : ObjectUICommand<T> where T : class, IBuildingBlock
   {
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForProjectBuildingBlock<T> _tasks;

      public CloneProjectBuildingBlockUICommand(IMoBiContext context, IInteractionTasksForProjectBuildingBlock<T> tasks)
      {
         _context = context;
         _tasks = tasks;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_tasks.Clone(Subject));
      }
   }
}