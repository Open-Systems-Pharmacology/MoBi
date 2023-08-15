using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class ReconfigureSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      protected ISimulationUpdateTask _simulationUpdateTask;
      protected IMoBiContext _context;

      protected ReconfigureSimulationUICommand(IActiveSubjectRetriever activeSubjectRetriever, ISimulationUpdateTask simulationUpdateTask, IMoBiContext context) : base(activeSubjectRetriever)
      {
         _simulationUpdateTask = simulationUpdateTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(PerformReconfigure());
      }

      protected abstract ICommand PerformReconfigure();
   }
}