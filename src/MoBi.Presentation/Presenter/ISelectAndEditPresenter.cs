using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectAndEditPresenter : IPresenter<ISelectAndEditContainerView>,
                                              ISubPresenter

   {
      void AddToProject();
      void ValidateStartValues();
   }
}