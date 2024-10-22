using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CommitSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly IMoBiContext _context;
      private readonly ISimulationCommitTask _simulationCommitTask;
      private readonly IInteractionTaskContext _interactionTaskContext;

      public CommitSimulationUICommand(IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever, ISimulationCommitTask simulationCommitTask, IInteractionTaskContext interactionTaskContext) : base(activeSubjectRetriever)
      {
         _context = context;
         _simulationCommitTask = simulationCommitTask;
         _interactionTaskContext = interactionTaskContext;
      }

      protected override void PerformExecute()
      {
         var changes = _simulationCommitTask.ShowChanges(Subject);
         if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(changes) != ViewResult.Yes)
            return;

         _context.AddToHistory(_simulationCommitTask.CommitSimulationChanges(Subject));
      }
   }
}