using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ICreationPresenter<T> : IPresenter
   {
      T GetNew();
   }
}