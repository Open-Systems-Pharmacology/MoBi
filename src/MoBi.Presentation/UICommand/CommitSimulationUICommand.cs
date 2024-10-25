using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly IMoBiContext _context;
      private readonly ISimulationCommitTask _simulationCommitTask;

      public CommitSimulationUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, ISimulationCommitTask simulationCommitTask) : base(activeSubjectRetriever)
      {
         _context = context;
         _simulationCommitTask = simulationCommitTask;
      }

      protected override void PerformExecute()
      {
         var command = _simulationCommitTask.CommitSimulationChanges(Subject);
         if(command != null)
            _context.AddToHistory(command);
      }
   }
}