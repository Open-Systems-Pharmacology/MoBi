using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class ExpressionProfileExtendUICommand : ActiveObjectUICommand<ExpressionProfileBuildingBlock>
   {
      private readonly IInitialConditionsTask<ExpressionProfileBuildingBlock> _initialConditionsTask;

      public ExpressionProfileExtendUICommand(IActiveSubjectRetriever activeSubjectRetriever, IInitialConditionsTask<ExpressionProfileBuildingBlock> initialConditionsTask) : base(activeSubjectRetriever)
      {
         _initialConditionsTask = initialConditionsTask;
      }

      protected override void PerformExecute()
      {
         _initialConditionsTask.ExtendExpressionProfileInitialConditions(Subject);
      }
   }
}