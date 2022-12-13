using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class AddExpressionProfileBuildingBlock : ObjectUICommand<AddExpressionProfileBuildingBlock>
   {
      protected IPKSimStarter _pkSimStarter;
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTasksForExpressionProfileBuildingBlock;

      protected AddExpressionProfileBuildingBlock(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTasksForExpressionProfileBuildingBlock)
      {
         _pkSimStarter = pkSimStarter;
         _context = context;
         _interactionTasksForExpressionProfileBuildingBlock = interactionTasksForExpressionProfileBuildingBlock;
      }

      protected override void PerformExecute()
      {
         var expressionFromPKSim = CreateExpressionFromPKSim();
         if (expressionFromPKSim != null)
         {
            _context.AddToHistory(_interactionTasksForExpressionProfileBuildingBlock.AddToProject(expressionFromPKSim));
         }
      }

      protected abstract ExpressionProfileBuildingBlock CreateExpressionFromPKSim();
   }

   public class AddNewIndividualTransporterBuildingBlock : AddExpressionProfileBuildingBlock
   {
      public AddNewIndividualTransporterBuildingBlock(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTask) : base(pkSimStarter, context, interactionTask)
      {
      }

      protected override ExpressionProfileBuildingBlock CreateExpressionFromPKSim()
      {
         return _pkSimStarter.CreateTransporterExpression();
      }
   }

   public class AddNewBindingPartnerBuildingBlock : AddExpressionProfileBuildingBlock
   {
      public AddNewBindingPartnerBuildingBlock(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTask) : base(pkSimStarter, context, interactionTask)
      {
      }

      protected override ExpressionProfileBuildingBlock CreateExpressionFromPKSim()
      {
         return _pkSimStarter.CreateBindingPartnerExpression();
      }
   }

   public class AddNewMetabolizingEnzymeBuildingBlock : AddExpressionProfileBuildingBlock
   {
      public AddNewMetabolizingEnzymeBuildingBlock(IPKSimStarter pkSimStarter, IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTask) : base(pkSimStarter, context, interactionTask)
      {
      }

      protected override ExpressionProfileBuildingBlock CreateExpressionFromPKSim()
      {
         return _pkSimStarter.CreateMetabolizingEnzymeExpression();
      }
   }
}