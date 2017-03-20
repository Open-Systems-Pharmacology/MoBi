using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class CloneBuildingBlockUICommand<T> : ObjectUICommand<T> where T : class, IBuildingBlock
   {
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForBuildingBlock<T> _tasks;

      public CloneBuildingBlockUICommand(IMoBiContext context, IInteractionTasksForBuildingBlock<T> tasks)
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