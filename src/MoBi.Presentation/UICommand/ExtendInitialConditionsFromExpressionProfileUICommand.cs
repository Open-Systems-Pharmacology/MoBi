using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ExtendInitialConditionsFromExpressionProfileUICommand : ExtendBuildingBlockFromPathAndValuesUICommand<ExpressionProfileBuildingBlock, Module, InitialConditionsBuildingBlock, InitialCondition>
   {
      public ExtendInitialConditionsFromExpressionProfileUICommand(
         IInitialConditionsTask<ExpressionProfileBuildingBlock> interactionTasksForSource, 
         IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTasksForTarget, 
         IMoBiContext context) : base(interactionTasksForSource, interactionTasksForTarget, context)
      {
      }

      protected override IReadOnlyList<InitialCondition> MapAll(IReadOnlyList<ExpressionProfileBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.SelectMany(x => x.InitialConditions).ToList();
      }
   }
}