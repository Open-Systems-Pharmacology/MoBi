using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ImportQuantityView : BaseModalView, IImportQuantityView
   {
      private IImportQuantitiesPresenter _presenter;
      private readonly ScreenBinder<ImportExcelSheetSelectionDTO> _excelSheetSelectionScreenBinder;
      private readonly ScreenBinder<QuantityImporterDTO> _importStartValuesDTOScreenBinder;

      public ImportQuantityView()
      {
         InitializeComponent();
         _excelSheetSelectionScreenBinder = new ScreenBinder<ImportExcelSheetSelectionDTO>();
         _importStartValuesDTOScreenBinder = new ScreenBinder<QuantityImporterDTO>();
      }

      public void BindTo(QuantityImporterDTO quantityImporterDTO)
      {
         _importStartValuesDTOScreenBinder.BindToSource(quantityImporterDTO);
      }

      public void BindTo(ImportExcelSheetSelectionDTO importFileSelectionDTO)
      {
         _excelSheetSelectionScreenBinder.BindToSource(importFileSelectionDTO);
      }

      public string HintLabel
      {
         get { return lblImportFileFormatHint.Text; }
         set { lblImportFileFormatHint.Text = value; }
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _excelSheetSelectionScreenBinder.Bind(x => x.FilePath)
            .To(filePathButtonEdit);

         _excelSheetSelectionScreenBinder.Bind(x => x.SelectedSheet)
            .To(_sheetNameComboBox)
            .WithValues(x => x.AllSheetNames);

         _excelSheetSelectionScreenBinder.Bind(x => x.Messages)
            .To(messageMemoEdit);

         _excelSheetSelectionScreenBinder.Bind(x => x.SelectedSheet)
            .ToEnableOf(ButtonExtra)
            .EnabledWhen(x => !string.IsNullOrEmpty(x));

         _importStartValuesDTOScreenBinder.Bind(x => x.Count)
            .ToEnableOf(ButtonOk)
            .EnabledWhen(count => count > 0);

         ButtonExtra.InitWithImage(ApplicationIcons.Run, AppConstants.Captions.StartImport);
         ExtraVisible = true;

         filePathButtonEdit.Click += (o, e) => OnEvent(() => _presenter.SelectFile());

         OkCaption = AppConstants.Captions.Transfer;
      }

      protected override void OkClicked()
      {
         base.OkClicked();
         _presenter.TransferImportedQuantities();
      }

      protected override void ExtraClicked()
      {
         base.ExtraClicked();
         _presenter.StartImport();
      }

      public override bool HasError => _excelSheetSelectionScreenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         OkEnabled = false;
         messageMemoEdit.Properties.ReadOnly = true;
      }

      public void UpdateSheetList()
      {
         _excelSheetSelectionScreenBinder.RefreshListElements();
      }

      public void AttachPresenter(IImportQuantitiesPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}