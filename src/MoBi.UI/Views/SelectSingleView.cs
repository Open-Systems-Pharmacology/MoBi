using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class SelectSingleView<T> : BaseUserControl, ISelectSingleView<T>
   {
      public SelectSingleView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         layoutControlItemParameterValues.TextVisible = false;
         cbItems.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
      }

      public void InitializeWith(IEnumerable<ListItemDTO<T>> allItems)
      {
         cbItems.Properties.Items.Clear();
         allItems.Each(item => cbItems.Properties.Items.Add(item));
         cbItems.SelectedIndex = 0;
      }

      public ListItemDTO<T> Selection => cbItems.SelectedItem as ListItemDTO<T>;

      public void SetDescription(string description)
      {
         lblDescription.Text = description;
      }

      public void AttachPresenter(ISelectSinglePresenter<T> presenter)
      {
      }
   }
}