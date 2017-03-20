using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;
using SimModelNET;

namespace MoBi.Presentation.Views
{
   public interface ISolverMessageView : IModalView<ISolverMessagePresenter>
   {
      void BindTo(IEnumerable<ISolverWarning> warnings);
   }
}