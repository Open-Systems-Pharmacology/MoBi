using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.UICommand
{
   public class ShowAboutUICommand : IUICommand
   {
      private readonly IMoBiApplicationController _applicationController;

      public ShowAboutUICommand(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public void Execute()
      {
         using (_applicationController.Start<IAboutPresenter>())
         {
            
         }
      }
   }
}