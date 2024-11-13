using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class AddNewFromPKSimCommand<T> : ObjectUICommand<T> where T : class
   {
      protected IPKSimStarter _pkSimStarter;
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForProjectBuildingBlock _interactionTask;

      protected AddNewFromPKSimCommand(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForProjectBuildingBlock interactionTask)
      {
         _pkSimStarter = pkSimStarter;
         _context = context;
         _interactionTask = interactionTask;
      }

      // Override this because the base class clears the Subject property after completion
      public override void Execute() => PerformExecute();

      protected override void PerformExecute()
      {
         var buildingBlockFromPKSim = CreateBuildingBlockFromPKSim();

         if (buildingBlockFromPKSim == null)
            return;

         _context.AddToHistory(_interactionTask.AddToProject(buildingBlockFromPKSim));
      }

      protected abstract IBuildingBlock CreateBuildingBlockFromPKSim();
   }
}