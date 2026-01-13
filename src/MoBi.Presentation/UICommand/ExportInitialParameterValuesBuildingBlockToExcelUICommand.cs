using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class ExportParameterValuesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      public ExportParameterValuesBuildingBlockToExcelUICommand(
         IParameterValuesTask interactionTask)
         : base(interactionTask)
      {
      }
   }
}