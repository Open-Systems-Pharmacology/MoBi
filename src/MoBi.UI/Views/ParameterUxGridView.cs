using System;
using System.Linq;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;

namespace MoBi.UI.Views
{
   public class ParameterUxGridView : UxGridView
   {
      public ParameterUxGridView(GridControl ownerGrid)
         : base(ownerGrid)
      {
      }

      public ParameterUxGridView()
      {
      }

      protected override void DoInit()
      {
         base.DoInit();
         PopupMenuShowing += OnMobiPopupMenuShowing;
      }

      private void OnMobiPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.HitInfo.HitTest != GridHitTest.RowIndicator || !e.Menu.Items.Any() || GetSelectedRows().Length != 1)
            return;

         var copyPathMenuItem = new DXMenuItem(
            "Copy Path",
            (s, args) => OnCopyPathRequested?.Invoke(GetSelectedRows().First()),
            ApplicationIcons.Copy
         );

         e.Menu.Items.Add(copyPathMenuItem);
        }

      public event Action<int> OnCopyPathRequested;
   }
}