using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationSettingsUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly ISimulationCommitTask _simulationCommitTask;
      private readonly IMoBiContext _context;

      public CommitSimulationSettingsUICommand(
         ISimulationCommitTask simulationCommitTask,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever)
      {
         _simulationCommitTask = simulationCommitTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_simulationCommitTask.CommitSimulationSettings(Subject));
      }
   }
}