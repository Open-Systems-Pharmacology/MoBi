using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RefreshSimulationOutputSelectionsUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly ISimulationUpdateTask _simulationUpdateTask;
      private readonly IMoBiContext _context;

      public RefreshSimulationOutputSelectionsUICommand(
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
         _context.AddToHistory(_simulationUpdateTask.UpdateSimulationOutputSelections(Subject));
      }
   }
}