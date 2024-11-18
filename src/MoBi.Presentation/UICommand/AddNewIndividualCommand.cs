using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddNewIndividualCommand : AddNewFromPKSimCommand<IndividualBuildingBlock>
   {
      public AddNewIndividualCommand(IPKSimStarter pkSimStarter, IMoBiContext context, IInteractionTasksForIndividualBuildingBlock interactionTask) : base(pkSimStarter, context, interactionTask)
      {
      }

      protected override IBuildingBlock CreateBuildingBlockFromPKSim()
      {
         return _pkSimStarter.CreateIndividual();
      }
   }
}