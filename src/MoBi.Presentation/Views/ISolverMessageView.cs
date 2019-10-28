using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;
using OSPSuite.SimModel;

namespace MoBi.Presentation.Views
{
   public interface ISolverMessageView : IModalView<ISolverMessagePresenter>
   {
      void BindTo(IEnumerable<SolverWarning> warnings);
   }
}