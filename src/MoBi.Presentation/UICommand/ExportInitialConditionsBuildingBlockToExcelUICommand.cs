using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ExportInitialConditionsBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<InitialConditionsBuildingBlock, InitialCondition>
   {
      public ExportInitialConditionsBuildingBlockToExcelUICommand(
         IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTask,
         IInitialConditionsBuildingBlockToDataTableMapper mapper)
         : base(interactionTask, mapper)
      {
      }
   }
}