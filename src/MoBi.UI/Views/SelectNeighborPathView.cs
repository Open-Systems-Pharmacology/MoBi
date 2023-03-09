using DevExpress.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class SelectNeighborPathView : BaseUserControl, ISelectNeighborPathView
   {
      private ISelectNeighborPathPresenter _presenter;
      private readonly ScreenBinder<ObjectPathDTO> _screenBinder = new ScreenBinder<ObjectPathDTO>();

      public SelectNeighborPathView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectNeighborPathPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddContainerCriteriaView(IView view)
      {
         panelContainerSelection.FillWith(view);
      }

      public void BindTo(ObjectPathDTO objectPathDTO)
      {
         _screenBinder.BindToSource(objectPathDTO);
      }

      public string Label
      {
         set => layoutItemContainerPath.Text = value.FormatForLabel();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.Path)
            .To(tbContainerPath);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Label = AppConstants.Captions.Path;
         layoutItemContainerPath.TextLocation = Locations.Top;

      }

      public override bool HasError => _screenBinder.HasError;
   }
}