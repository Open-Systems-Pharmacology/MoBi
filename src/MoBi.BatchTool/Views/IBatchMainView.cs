using MoBi.BatchTool.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.BatchTool.Views
{
   public interface IBatchMainView : IView<IBatchMainPresenter>
   {
      void Hide();
   }
}