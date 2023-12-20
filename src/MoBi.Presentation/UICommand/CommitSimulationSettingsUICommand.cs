using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationSettingsUICommand : ActiveObjectUICommand<SimulationSettings>
   {
      private readonly IInteractionTasksForSimulationSettings _simulationSettingsTask;
      private readonly IMoBiContext _context;

      public CommitSimulationSettingsUICommand(
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
         _simulationSettingsTask.UpdateDefaultSimulationSettingsInProject(_context.Clone(Subject));
      }
   }
}