using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class RemoveSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IInteractionTasksForChildren<MoBiProject, IMoBiSimulation> _simulationTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RemoveSimulationUICommand(IMoBiContext context, IInteractionTasksForChildren<MoBiProject, IMoBiSimulation> simulationTasks, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _context = context;
         _simulationTasks = simulationTasks;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_simulationTasks.Remove(Subject, _context.CurrentProject, buildingBlock));
      }
   }
}