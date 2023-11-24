using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public class IndividualDistributedPathAndValueEntityView : DistributedPathAndValueEntityView<IIndividualDistributedPathAndValueEntityPresenter, IndividualParameter, IndividualParameterDTO, IndividualBuildingBlock>, 
      IIndividualDistributedPathAndValueEntityView
   {
      protected override IFormatter<double?> FormatterFor(IndividualParameterDTO dto)
      {
         return dto.IndividualParameterFormatter();
      }
   }
}