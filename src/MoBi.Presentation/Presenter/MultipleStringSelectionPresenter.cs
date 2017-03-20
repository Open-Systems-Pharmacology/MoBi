using System.Collections.Generic;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IMultipleStringSelectionPresenter : IDisposablePresenter
   {
      IEnumerable<string> Show(string caption, string text, IEnumerable<string> valueList, string addToListToolTip, bool canAdd);
      void AddNameToList(string newItem);
   }

   public class MultipleStringSelectionPresenter : AbstractDisposablePresenter<IMultipleStringSelectionView, IMultipleStringSelectionPresenter>, IMultipleStringSelectionPresenter
   {
      public MultipleStringSelectionPresenter(IMultipleStringSelectionView view) : base(view)
      {
      }

      public IEnumerable<string> Show(string caption, string text, IEnumerable<string> valueList, string addToListToolTip, bool canAdd = true)
      {
         _view.Text = caption;
         _view.ValueList = valueList;
         _view.DisplayText = text;
         _view.NewItem = string.Empty;
         _view.AddToListToolTip = addToListToolTip;
         _view.EnableAddingValues(canAdd);
         _view.Display();
         
         return _view.Canceled ? new string[0] : _view.SelectedItems();
      }

      public void AddNameToList(string newItem)
      {
         _view.AddItem(newItem);
         _view.NewItem = string.Empty;
      }
   }
}