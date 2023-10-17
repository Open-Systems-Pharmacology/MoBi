using DevExpress.XtraEditors.Controls;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.UI.Views;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class SelectFolderAndIndividualFromProjectView : BaseModalView, ISelectFolderAndIndividualFromProjectView
   {
      private readonly ScreenBinder<IndividualAndFilePathDTO> _screenBinder = new ScreenBinder<IndividualAndFilePathDTO>();
      
      private ISelectFolderAndIndividualFromProjectPresenter _presenter;

      public SelectFolderAndIndividualFromProjectView()
      {
         InitializeComponent();
         ExtraVisible = false;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         descriptionLabel.Text = ExportContainerDescription;
         layoutControlItemSelectIndividual.Text = Individual;
         layoutControlItemSelectFilePath.Text = Captions.FilePath;
         Caption = SelectIndividualAndPathForContainerExport;
         btnSelectFilePath.ReadOnly = true;
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.IndividualBuildingBlock).To(cmbSelectIndividual).WithValues(x => _presenter.AllIndividuals);
         _screenBinder.Bind(x => x.FilePath).To(btnSelectFilePath);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public void AttachPresenter(ISelectFolderAndIndividualFromProjectPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IndividualAndFilePathDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }

      private void btnSelectFilePathClick(object sender, ButtonPressedEventArgs e)
      {
         btnSelectFilePath.EditValue = _presenter.BrowseFilePath();
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}