using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RefreshSimulationSettingsUICommand : ActiveObjectUICommand<SimulationSettings>
   {
      private readonly ISimulationUpdateTask _simulationUpdateTask;
      private readonly IMoBiContext _context;

      public RefreshSimulationSettingsUICommand(
         ISimulationUpdateTask simulationUpdateTask,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever)
      {
         _simulationUpdateTask = simulationUpdateTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var simulation = _context.CurrentProject.Simulations.FirstOrDefault(x => x.Settings.Equals(Subject));
         if (simulation == null)
            return;

         _context.AddToHistory(_simulationUpdateTask.UpdateSimulationSettings(simulation));
      }
   }
}