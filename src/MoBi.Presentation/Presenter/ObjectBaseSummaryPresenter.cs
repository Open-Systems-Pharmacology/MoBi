using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IObjectBaseSummaryPresenter : IPresenter
   {
      /// <summary>
      /// Binds an object to the presenter
      /// </summary>
      /// <param name="objectBase"></param>
      void BindTo(ObjectBaseSummaryDTO objectBase);
   }

   class ObjectBaseSummaryPresenter : AbstractPresenter<IObjectBaseSummaryView, IObjectBaseSummaryPresenter>, IObjectBaseSummaryPresenter
   {
      public ObjectBaseSummaryPresenter(IObjectBaseSummaryView view) : base(view)
      {
      }

      public void BindTo(ObjectBaseSummaryDTO objectBase)
      {
         _view.BindTo(objectBase);
      }
   }
}
