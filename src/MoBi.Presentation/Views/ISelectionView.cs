using System;
using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectionView:IView
   {
      void SetError(string message);
   }

   public interface ISelectionView<T> : ISelectionView
   {
      IEnumerable<T> SelectionList { get; set; }
      T Selection { get; set; }
      void AttachPresenter(ISelectionPresenter<T> presenter);
      void Initialise(Func<T, string> toString);
   }
}