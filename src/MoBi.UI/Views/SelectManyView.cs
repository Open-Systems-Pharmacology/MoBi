using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;

using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.UI.Views
{
   public partial class SelectManyView<T> : ListBoxControl, ISelectManyView<T>
   {
      private ISelectManyPresenter<T> _presenter;

      public SelectManyView()
      {
         InitializeComponent();
         SelectionMode = SelectionMode.MultiExtended;
      }

      public void InitializeBinding()
      {
         /*nothing to do */
      }

      public void InitializeResources()
      {
         /*nothing to do */
      }

      public void AttachPresenter(IPresenter presenter)
      {
         /*nothing to do */
      }

      public string Caption { get; set; }

      public bool HasError
      {
         get { return false; }
      }

      public ApplicationIcon ApplicationIcon { get; set; }

      public event EventHandler CaptionChanged = delegate { };

      public void InitializeWith(IEnumerable<ListItemDTO<T>> allItems)
      {
         Items.Clear();
         allItems.Each(item => Items.Add(item));
      }

      public IEnumerable<ListItemDTO<T>> Selections
      {
         get { return SelectedItems.Cast<ListItemDTO<T>>(); }
      }

      public void AttachPresenter(ISelectManyPresenter<T> presenter)
      {
         _presenter = presenter;
      }
   }
}