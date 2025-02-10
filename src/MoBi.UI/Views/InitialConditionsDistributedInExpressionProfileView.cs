using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public class InitialConditionsDistributedInExpressionProfileView
      : DistributedPathAndValueEntityView<IInitialConditionsDistributedInExpressionProfilePresenter, InitialCondition, InitialConditionDTO, ExpressionProfileBuildingBlock>,
         IInitialConditionsDistributedInExpressionProfileView
   {
      protected override IFormatter<double?> FormatterFor(InitialConditionDTO dto)
      {
         return dto.InitialConditionFormatter();
      }
   }
}