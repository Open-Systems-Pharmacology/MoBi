using OSPSuite.Core.Domain;
using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditOutputSelectionsView : IView<IEditOutputSelectionsPresenter>
   {
      void BindTo(IEnumerable<QuantitySelection> allOutputs);
   }
}
