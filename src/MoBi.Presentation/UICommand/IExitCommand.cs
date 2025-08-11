using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public interface IExitCommand : IUICommand
   {
      bool Canceled { get; }
   }
}