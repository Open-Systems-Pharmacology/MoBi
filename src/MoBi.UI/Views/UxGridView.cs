using DevExpress.XtraGrid;
using MoBi.Presentation;

namespace MoBi.UI.Views
{
   public class UxGridView : OSPSuite.UI.Controls.UxGridView
   {
      public UxGridView(GridControl ownerGrid)
         : base(ownerGrid)
      {
      }

      public UxGridView()
      {
      }

      protected override void DoInit()
      {
         base.DoInit();
         ShowColumnChooser = true;
         ShowRowIndicator = true;
         MultiSelect = true;
      }
   }
}