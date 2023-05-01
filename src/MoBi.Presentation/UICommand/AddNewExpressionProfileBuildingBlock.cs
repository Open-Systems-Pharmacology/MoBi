using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddExpressionProfileBuildingBlock : AddNewFromPKSimCommand<ExpressionType>
   {
      public AddExpressionProfileBuildingBlock(IPKSimStarter pkSimStarter, IMoBiContext context, IInteractionTasksForExpressionProfileBuildingBlock interactionTask) : 
         base(pkSimStarter, context, interactionTask)
      {
      }

      protected override IBuildingBlock createBuildingBlockFromPKSim()
      {
         return _pkSimStarter.CreateProfileExpression(Subject);
      }
   }
}