using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class ExportExpressionProfilesBuildingBlockToExcelUICommand : ExportBuildingBlockToExcelUICommand<ExpressionProfileBuildingBlock>
   {
      public ExportExpressionProfilesBuildingBlockToExcelUICommand(
         IMoBiProjectRetriever projectRetriever,
         IDialogCreator dialogCreator,
         IExpressionProfileBuildingBlockToDataTableMapper mapper)
         : base(projectRetriever, dialogCreator, mapper)
      {
      }
   }
}