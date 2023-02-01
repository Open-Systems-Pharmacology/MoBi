using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.UI.Extensions;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class MultipleStringSelectionView : BaseModalView, IMultipleStringSelectionView
   {
      private IMultipleStringSelectionPresenter _presenter;

      public MultipleStringSelectionView(IMainView mainView)
         : base(mainView)
      {
         InitializeComponent();
         selectionList.SelectionMode = SelectionMode.MultiExtended;
      }

      protected override void SetActiveControl()
      {
         ActiveControl = txtNewName;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         selectionList.SelectedIndexChanged += updateStatus;
         txtNewName.TextChanged += updateStatus;
         btAddNew.Click += addNewClick;
      }

      private void updateStatus(object sender, EventArgs e)
      {
         SetOkButtonEnable();
      }

      public string AddToListToolTip
      {
         get => btAddNew.ToolTip;
         set => btAddNew.ToolTip = value;
      }

      public IEnumerable<string> SelectedItems()
      {
         return selectionList.SelectedItems.Select(selectedItem => selectedItem.ToString()).ToList();
      }

      public void AddItem(string newItem)
      {
         selectionList.Items.Add(newItem);
         selectionList.SelectedItem = newItem;
      }

      public void EnableAddingValues(bool canAdd)
      {
         layoutControlItemAddButton.Visibility = LayoutVisibilityConvertor.FromBoolean(canAdd);
         layoutControlItemNewName.Visibility = layoutControlItemAddButton.Visibility;
      }

      public string NewItem
      {
         get => txtNewName.Text;
         set => txtNewName.Text = value;
      }

      public IEnumerable<string> ValueList
      {
         set
         {
            selectionList.Items.Clear();
            selectionList.Items.AddRange(value.ToArray());
         }
      }

      public string DisplayText
      {
         set => lblDescription.Text = value;
      }

      public void AttachPresenter(IMultipleStringSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      private void addNewClick(object sender, EventArgs e)
      {
         _presenter.AddNameToList(txtNewName.Text);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemAddButton.AdjustButtonSize(layoutControl);
      }

      public override bool HasError => base.HasError || !hasSelection();

      private bool hasSelection()
      {
         return selectionList.SelectedItems.Count > 0 && string.IsNullOrEmpty(NewItem);
      }
   }
}