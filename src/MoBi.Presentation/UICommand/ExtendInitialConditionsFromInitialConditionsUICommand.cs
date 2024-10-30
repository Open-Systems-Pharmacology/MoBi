using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ExtendInitialConditionsFromInitialConditionsUICommand : ExtendBuildingBlockFromPathAndValuesUICommand<InitialConditionsBuildingBlock, Module, InitialConditionsBuildingBlock, InitialCondition>
   {
      public ExtendInitialConditionsFromInitialConditionsUICommand(IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTasksForSource, IMoBiContext context) : base(interactionTasksForSource, interactionTasksForSource, context)
      {
      }

      protected override IReadOnlyList<InitialCondition> MapAll(IReadOnlyList<InitialConditionsBuildingBlock> buildingBlocks)
      {
         return buildingBlocks.SelectMany(x => x).ToList();
      }
   }
}