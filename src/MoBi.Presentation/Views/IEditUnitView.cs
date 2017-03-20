using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditUnitView : IView<IEditUnitPresenter>
   {
      void Show(Unit unit);
   }
}