using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddExpressionProfileBuildingBlock : ObjectUICommand<ExpressionType>
   {
      protected IPKSimStarter _pkSimStarter;
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTask;

      public AddExpressionProfileBuildingBlock(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTask)
      {
         _pkSimStarter = pkSimStarter;
         _context = context;
         _interactionTask = interactionTask;
      }

      protected override void PerformExecute()
      {
         var expressionFromPKSim = _pkSimStarter.CreateProfileExpression(Subject);

         if (expressionFromPKSim == null)
            return;

         _context.AddToHistory(_interactionTask.AddToProject(expressionFromPKSim));
      }
   }
}