using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class SelectFolderAndIndividualAndExpressionFromProjectView : BaseModalView, ISelectFolderAndIndividualAndExpressionFromProjectView
   {
      private readonly ScreenBinder<IndividualExpressionAndFilePathDTO> _screenBinder = new ScreenBinder<IndividualExpressionAndFilePathDTO>();
      private readonly GridViewBinder<ExpressionProfileBuildingBlockSelectionDTO> _gridViewBinder;
      private ISelectFolderAndIndividualAndExpressionFromProjectPresenter _presenter;
      private readonly IImageListRetriever _imageListRetriever;

      public SelectFolderAndIndividualAndExpressionFromProjectView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ExpressionProfileBuildingBlockSelectionDTO>(gridView);

         // Disable "selected" appearance for rows. UxRepositoryItemImageComboBox do not have a "selected" appearance.
         Load += (o, e) => OnEvent(formLoad);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemSelectIndividual.Text = Individual;
         
         layoutControlItemSelectFilePath.Text = Captions.FilePath;
         Caption = SelectIndividualAndPathForContainerExport;
         btnSelectFilePath.ReadOnly = true;
         gridView.ConfigureGridForCheckBoxSelect(nameof(ExpressionProfileBuildingBlockSelectionDTO.Selected));
         gridView.DisableGrouping();
         gridView.SelectionChanged += (o, e) => OnEvent(gridViewSelectionChanged);
         layoutControlItemExpressionSelect.TextLocation = Locations.Top;
         layoutControlItemExpressionSelect.TextVisible = true;
         layoutControlItemExpressionSelect.Text = ExpressionProfiles;
      }

      private void gridViewSelectionChanged()
      {
         // The select/unselect of a molecule will affect the validation of another molecule
         // with the same name. If both were selected, and one is unselected, then the other becomes
         // valid.
         gridView.RefreshData();
         _presenter.SelectionChanged();
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.IndividualBuildingBlock).To(cmbSelectIndividual).WithValues(x => _presenter.AllIndividuals);
         _screenBinder.Bind(x => x.FilePath).To(btnSelectFilePath);
         _screenBinder.Bind(x => x.Description).To(descriptionLabel);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         _gridViewBinder.AutoBind(dto => dto.DisplayName).WithRepository(configureExpressionRepository).WithCaption(ExpressionProfile).AsReadOnly();
      }

      private RepositoryItem configureExpressionRepository(ExpressionProfileBuildingBlockSelectionDTO expressionProfile)
      {
         var repository = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return repository.AddItem(expressionProfile.DisplayName, expressionProfile.Icon);
      }

      public void AttachPresenter(ISelectFolderAndIndividualAndExpressionFromProjectPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IndividualExpressionAndFilePathDTO dto)
      {
         _screenBinder.BindToSource(dto);
         _gridViewBinder.BindToSource(dto.SelectableExpressionProfiles);
      }

      private void btnSelectFilePathClick(object sender, ButtonPressedEventArgs e)
      {
         btnSelectFilePath.EditValue = _presenter.BrowseFilePath();
      }

      private void formLoad()
      {
         gridControl.ForceInitialize();
         gridView.Appearance.SelectedRow.Assign(gridView.PaintAppearance.Row);
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
         _gridViewBinder.Dispose();
      }
   }
}