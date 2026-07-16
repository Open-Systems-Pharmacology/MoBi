using OSPSuite.Presentation.Views;

namespace MoBi.BatchTool.Views
{
   public interface IBatchView : IView
   {
      void Display();
      void AddLogView(IView view);
      bool CalculateEnabled { set; }
   }
}