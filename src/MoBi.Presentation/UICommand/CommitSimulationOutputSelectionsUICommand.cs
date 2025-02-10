using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationOutputSelectionsUICommand : ActiveObjectUICommand<SimulationSettings>
   {
      private readonly IInteractionTasksForSimulationSettings _simulationSettingsTask;

      public CommitSimulationOutputSelectionsUICommand(
         IInteractionTasksForSimulationSettings simulationSettingsTask,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever)
      {
         _simulationSettingsTask = simulationSettingsTask;
      }

      protected override void PerformExecute()
      {
         _simulationSettingsTask.UpdateDefaultOutputSelectionsInProject(Subject.OutputSelections.ToList());
      }
   }
}