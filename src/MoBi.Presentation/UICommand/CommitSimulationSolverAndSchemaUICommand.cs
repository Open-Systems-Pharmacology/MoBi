using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationSolverAndSchemaUICommand : ActiveObjectUICommand<SimulationSettings>
   {
      private readonly IInteractionTasksForSimulationSettings _simulationSettingsTask;
      private readonly IMoBiContext _context;

      public CommitSimulationSolverAndSchemaUICommand(
         IInteractionTasksForSimulationSettings simulationSettingsTask,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever)
      {
         _simulationSettingsTask = simulationSettingsTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var clonedSimulationSettings = _context.Clone(Subject);
         _simulationSettingsTask.UpdateDefaultSimulationSettingsInProject(clonedSimulationSettings.Solver, clonedSimulationSettings.OutputSchema);
      }
   }
}