using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ExportParameterValuesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      public ExportParameterValuesBuildingBlockToExcelUICommand(
         IParameterValuesTask interactionTask,
         IParameterValueBuildingBlockToParameterValuesDataTableMapper mapper)
         : base(interactionTask, mapper)
      {
      }
   }
}