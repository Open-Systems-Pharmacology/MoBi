using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IMultipleStringSelectionView : IModalView<IMultipleStringSelectionPresenter>
   {
      IEnumerable<string> ValueList { set; }
      string DisplayText { set; }
      string Text { set; get; }
      string NewItem { get; set; }
      string AddToListToolTip { get; set; }
      IEnumerable<string> SelectedItems();
      void AddItem(string newItem);
      void EnableAddingValues(bool canAdd);
   }
}