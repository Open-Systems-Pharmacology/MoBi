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

      public CommitSimulationSolverAndSchemaUICommand(
         IInteractionTasksForSimulationSettings simulationSettingsTask, 
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever)
      {
         _simulationSettingsTask = simulationSettingsTask;
      }

      protected override void PerformExecute()
      {
         _simulationSettingsTask.UpdateDefaultSimulationSettingsInProject(Subject.OutputSchema, Subject.Solver);
      }
   }
}