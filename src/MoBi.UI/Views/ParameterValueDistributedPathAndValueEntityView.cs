using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public class ParameterValueDistributedPathAndValueEntityView : DistributedPathAndValueEntityView<IParameterValueDistributedPathAndValueEntityPresenter, ParameterValue, ParameterValueDTO, ParameterValuesBuildingBlock>,
      IParameterValueDistributedPathAndValueEntityView
   {
      protected override IFormatter<double?> FormatterFor(ParameterValueDTO dto)
      {
         return dto.ParameterValueFormatter();
      }
   }
}