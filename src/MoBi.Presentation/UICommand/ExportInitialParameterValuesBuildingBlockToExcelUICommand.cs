using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ExportParameterValuesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ParameterValuesBuildingBlock>
   {
      public ExportParameterValuesBuildingBlockToExcelUICommand(
         IMoBiProjectRetriever projectRetriever,
         IDialogCreator dialogCreator,
         IParameterValueBuildingBlockToParameterValuesDataTableMapper mapper)
         : base(projectRetriever, dialogCreator, mapper)
      {
      }
   }
}