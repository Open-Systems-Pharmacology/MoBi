using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ExportSelectedQuantitiesView : BaseModalView, IExportSelectedQuantitiesView
   {
      private IExportSelectedQuantitiesPresenter _presenter;
      private readonly ScreenBinder<ExportQuantitiesSelectionDTO> _screenBinder;

      public ExportSelectedQuantitiesView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ExportQuantitiesSelectionDTO>();
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.ReportFile).To(btnSelectReportPath);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnSelectReportPath.ButtonClick += (o, e) => this.DoWithinExceptionHandler(() => _presenter.SelectReportFile());
      }

      public void AttachPresenter(IExportSelectedQuantitiesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSelectionView(IView view)
      {
         panelControl.FillWith(view);
      }

      public void BindTo(ExportQuantitiesSelectionDTO exportQuantitiesSelectionDTO)
      {
         _screenBinder.BindToSource(exportQuantitiesSelectionDTO);
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.SelectDataToExport;
         ApplicationIcon = ApplicationIcons.ObservedData;
         layoutItemSelectionPath.Text = AppConstants.Captions.ReportFile.FormatForLabel();
      }
   }
}