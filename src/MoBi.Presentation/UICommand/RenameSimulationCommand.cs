using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RenameSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTaskFor<IMoBiSimulation> _simulationTasks;
      private readonly IMoBiContext _context;

      public RenameSimulationUICommand(IEditTaskFor<IMoBiSimulation> simulationTasks, IMoBiContext context)
      {
         _simulationTasks = simulationTasks;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _simulationTasks.Rename(Subject, _context.CurrentProject.Simulations, null);
      }
   }
}