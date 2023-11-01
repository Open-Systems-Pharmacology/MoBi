using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class ApplyToSelectionView : BaseUserControl, IApplyToSelectionView
   {
      protected IApplyToSelectionPresenter _presenter;
      protected readonly ScreenBinder<IApplyToSelectionPresenter> _screenBinder;

      public ApplyToSelectionView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IApplyToSelectionPresenter>();
         cbSelection.Properties.SmallImages = imageListRetriever.AllImages16x16;
         cbSelection.Properties.LargeImages = imageListRetriever.AllImages32x32;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnSelection.InitWithImage(ApplicationIcons.OK, AppConstants.Captions.Apply);
         cbSelection.Properties.AutoHeight = false;
         cbSelection.Height = btnSelection.Height;
      }

      public override string Caption
      {
         set => lblCaption.Text = value.FormatForLabel();
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.CurrentSelection)
            .To(cbSelection)
            .WithImages(x => ApplicationIcons.IconIndex(x.Icon))
            .WithValues(x => _presenter.AvailableSelectOptions)
            .AndDisplays(x => x.Caption);

         btnSelection.Click += (o, e) => OnEvent(_presenter.PerformSelectionHandler);
      }

      public void AttachPresenter(IApplyToSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindToSelection()
      {
         _screenBinder.BindToSource(_presenter);
      }
   }
}