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
      private readonly IInteractionTasksForIndividualBuildingBlock _interactionTask;

      protected AddNewFromPKSimCommand(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForIndividualBuildingBlock interactionTask)
      {
         _pkSimStarter = pkSimStarter;
         _context = context;
         _interactionTask = interactionTask;
      }

      protected override void PerformExecute()
      {
         var buildingBlockFromPKSim = createBuildingBlockFromPKSim();

         if (buildingBlockFromPKSim == null)
            return;

         _context.AddToHistory(_interactionTask.AddToProject(buildingBlockFromPKSim));
      }

      protected abstract IBuildingBlock createBuildingBlockFromPKSim();
   }
}