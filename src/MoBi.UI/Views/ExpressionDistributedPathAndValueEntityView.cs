using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public class ExpressionDistributedPathAndValueEntityView : DistributedPathAndValueEntityView<IExpressionDistributedPathAndValueEntityPresenter, ExpressionParameter, ExpressionParameterDTO, ExpressionProfileBuildingBlock>, 
      IExpressionDistributedPathAndValueEntityView
   {
      protected override IFormatter<double?> FormatterFor(ExpressionParameterDTO dto)
      {
         return dto.ExpressionParameterFormatter();
      }
   }
}