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
            .ToEnableOf(btnExtra)
            .EnabledWhen(x => !string.IsNullOrEmpty(x));

         _importStartValuesDTOScreenBinder.Bind(x => x.Count)
            .ToEnableOf(btnOk)
            .EnabledWhen(count => count > 0);

         btnExtra.InitWithImage(ApplicationIcons.Run, AppConstants.Captions.StartImport);
         btnExtra.Click += (o, e) => OnEvent(() => _presenter.StartImport());
         ExtraVisible = true;

         filePathButtonEdit.Click += (o, e) => OnEvent(() => _presenter.SelectFile());

         btnOk.Click += (o, e) => OnEvent(() => _presenter.TransferImportedQuantities());
         btnOk.Text = AppConstants.Captions.Transfer;
      }

      public override bool HasError
      {
         get { return _excelSheetSelectionScreenBinder.HasError; }
      }

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