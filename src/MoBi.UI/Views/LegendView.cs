using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class LegendView : BaseUserControl, ILegendView
   {
      private ILegendPresenter _presenter;

      public LegendView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         legendItemsLayoutControl.Appearance.DisabledLayoutItem.ForeColor = MoBiColors.Black;
         layoutControlGroup.LayoutMode = LayoutMode.Table;
         layoutControlGroup.OptionsTableLayoutGroup.Remove(layoutControlGroup.OptionsTableLayoutGroup.RowDefinitions[1]);
      }

      public void AddLegendItem(LegendItemDTO legendItem)
      {
         var newItem = legendItemsLayoutControl.AddItem();
         newItem.Text = legendItem.Description;

         var tb = new TextEdit { BackColor = legendItem.Color, Enabled = false, MaximumSize = new Size(100, 0), Name = legendItem.Description };
         tb.Properties.AllowFocused = false;
         tb.Properties.ReadOnly = true;
         tb.Properties.Appearance.Options.UseBackColor = true;
         tb.Properties.BorderStyle = BorderStyles.NoBorder;
         newItem.Control = tb;
         newItem.TextLocation = Locations.Right;

         var targetColumn = _presenter.TargetColumnFor(legendItem);
         var targetRow = _presenter.TargetRowFor(legendItem);

         if (targetColumn >= layoutControlGroup.OptionsTableLayoutGroup.ColumnDefinitions.Count)
            layoutControlGroup.OptionsTableLayoutGroup.AddColumn();

         if (targetRow >= layoutControlGroup.OptionsTableLayoutGroup.RowDefinitions.Count)
            layoutControlGroup.OptionsTableLayoutGroup.AddRow();

         newItem.OptionsTableLayoutItem.ColumnIndex = targetColumn;
         newItem.OptionsTableLayoutItem.RowIndex = targetRow;

         layoutControlGroup.AddItem(newItem);

         legendItemsLayoutControl.BestFit();
      }

      public void AttachPresenter(ILegendPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
