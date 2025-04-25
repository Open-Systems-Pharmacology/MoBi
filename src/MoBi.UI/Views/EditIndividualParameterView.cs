using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditIndividualParameterView : BaseUserControl, IEditIndividualParameterView
   {
      private IEditIndividualParameterPresenter _presenter;
      private readonly ScreenBinder<IndividualParameterDTO> _screenBinder = new ScreenBinder<IndividualParameterDTO>();

      public EditIndividualParameterView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutControlItemFindParameter.AdjustLargeButtonSize(uxLayoutControl);
         btnFindParameter.InitWithImage(ApplicationIcons.Search, AppConstants.Captions.FindParameter);
         lblWarning.AllowHtmlString = true;
         layoutControlGroupWarning.Text = AppConstants.Captions.Warning;
         layoutControlItemWarning.Padding += new Padding(0, 0, 0, 5);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Name)
            .To(textEditName);
         textEditName.Properties.ReadOnly = true;

         btnFindParameter.Click += (o, e) => OnEvent(() => _presenter.NavigateToParameter());
      }

      public void AttachPresenter(IEditIndividualParameterPresenter presenter) => _presenter = presenter;

      public void BindTo(IndividualParameterDTO individualParameterDTO) => _screenBinder.BindToSource(individualParameterDTO);

      public void ShowWarningFor(string buildingBlockName) =>
         lblWarning.Text = AppConstants.Warnings.PleaseEditTheParameterInTheIndividualBuildingblock(buildingBlockName).FormatForDescription();

      private void disposeBinders() => _screenBinder?.Dispose();
   }
}