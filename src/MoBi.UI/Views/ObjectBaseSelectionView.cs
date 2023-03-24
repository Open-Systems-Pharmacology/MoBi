using System;
using OSPSuite.UI.Extensions;
using OSPSuite.Assets;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public abstract partial class ObjectBaseSelectionView : BaseUserControl
   {
      public event EventHandler<ViewResizedEventArgs> HeightChanged = delegate { };

      protected ObjectBaseSelectionView()
      {
         InitializeComponent();
      }

      public bool NewVisible
      {
         set => layoutItemNew.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public void AdjustHeight()
      {
         HeightChanged(this, new ViewResizedEventArgs(OptimalHeight));
      }

      public void Repaint()
      {
         Refresh();
      }

      public int OptimalHeight => layoutItemComboBox.Height;

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnNew.Image = ApplicationIcons.Create.ToImage(IconSizes.Size16x16);
         btnNew.ImageLocation = ImageLocation.MiddleCenter;
         layoutItemNew.AdjustButtonSizeWithImageOnly(layoutControl);
         cbBuildingBlocks.Properties.AllowHtmlDraw = DefaultBoolean.True;
      }

      protected abstract void DisposeBinders();
   }
}