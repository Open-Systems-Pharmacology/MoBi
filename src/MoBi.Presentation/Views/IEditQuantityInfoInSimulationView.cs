using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditQuantityInfoInSimulationView : IView<IEditQuantityInfoInSimulationPresenter>
   {
      void BindTo(QuantityDTO quantityDTO);
      void BindTo(IReadOnlyCollection<UsedCalculationMethod> dtoUsedCalculationMethods);
   }
}