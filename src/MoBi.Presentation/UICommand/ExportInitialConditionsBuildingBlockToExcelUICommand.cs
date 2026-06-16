using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ExportInitialConditionsBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<InitialConditionsBuildingBlock, InitialCondition>
   {
      public ExportInitialConditionsBuildingBlockToExcelUICommand(
         IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTask)
         : base(interactionTask)
      {
      }
   }
}