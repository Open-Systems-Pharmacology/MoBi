using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditViewWithParameters : IView
   {
      void AddParameterView(IView view);
      void ShowParameters();
   }
}