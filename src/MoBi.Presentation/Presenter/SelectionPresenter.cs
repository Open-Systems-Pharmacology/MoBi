using System.Collections.Generic;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectionPresenter
   {
      object Select();
      ISelectionView GetView();
      void SetDefault();
   }

   public interface ISelectionPresenter<T> : ISelectionPresenter,IPresenter
   {
      IEnumerable<T> SelectionList { get; set; }
      T Selection { set; get; }
   }
}