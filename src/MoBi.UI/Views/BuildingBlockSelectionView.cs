using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Extensions;
using OSPSuite.Assets;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class BuildingBlockSelectionView : BaseUserControl, IBuildingBlockSelectionView
   {
      private ScreenBinder<BuildingBlockSelectionDTO> _screenBinder;
      private IBuildingBlockSelectionPresenter _presenter;
      public event EventHandler<ViewResizedEventArgs> HeightChanged = delegate { };

      public BuildingBlockSelectionView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IBuildingBlockSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<BuildingBlockSelectionDTO>();
         _screenBinder.Bind(x => x.BuildingBlock)
            .To(cbBuildingBlocks)
            .WithValues(x => _presenter.AllAvailableBlocks)
            .AndDisplays(x => _presenter.DisplayNameFor(x))
            .Changed += () => OnEvent(_presenter.SelectedBuildingBlockChanged);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnNew.Click += (o, e) => OnEvent(() => _presenter.CreateNew());
      }

      public bool NewVisible
      {
         set => layoutItemNew.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public void BindTo(BuildingBlockSelectionDTO buildingBlockSelectionDTO)
      {
         _screenBinder.BindToSource(buildingBlockSelectionDTO);
         AdjustHeight();
      }

      public void RefreshElementList()
      {
         _screenBinder.RefreshListElements();
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

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnNew.Image = ApplicationIcons.Create.ToImage(IconSizes.Size16x16);
         btnNew.ImageLocation = ImageLocation.MiddleCenter;
         layoutItemNew.AdjustButtonSizeWithImageOnly();
         cbBuildingBlocks.Properties.AllowHtmlDraw = DefaultBoolean.True;
      }
   }
}