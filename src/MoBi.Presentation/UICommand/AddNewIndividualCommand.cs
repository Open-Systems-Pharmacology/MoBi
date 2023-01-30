using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddNewIndividualCommand : ObjectUICommand<IndividualBuildingBlock>
   {
      protected IPKSimStarter _pkSimStarter;
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForIndividualBuildingBlock _interactionTask;

      public AddNewIndividualCommand(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForIndividualBuildingBlock interactionTask)
      {
         _pkSimStarter = pkSimStarter;
         _context = context;
         _interactionTask = interactionTask;
      }

      protected override void PerformExecute()
      {
         var individualFromPKSim = _pkSimStarter.CreateIndividual();

         if (individualFromPKSim == null)
            return;

         _context.AddToHistory(_interactionTask.AddToProject(individualFromPKSim));
      }
   }
}