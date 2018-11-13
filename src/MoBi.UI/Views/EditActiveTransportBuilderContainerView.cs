using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditActiveTransportBuilderContainerView : BaseUserControl, IEditActiveTransportBuilderContainerView
   {
      private IEditTransporterMoleculeContainerPresenter _presenter;
      private readonly ScreenBinder<TransporterMoleculeContainerDTO> _screenBinder;

      public EditActiveTransportBuilderContainerView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<TransporterMoleculeContainerDTO>();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemName.Text = AppConstants.Captions.TranporterMoleculeName.FormatForLabel();
         layoutControlItemTranportName.Text = AppConstants.Captions.TransporterName.FormatForLabel();
         layoutControlItemTranportName.OptionsToolTip.ToolTip = ToolTips.TransporterName;
         layoutControlItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutControlItemDescription.OptionsToolTip.ToolTip = ToolTips.Description;
         tabParameter.Text = AppConstants.Captions.Parameters;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Name).To(btEditName);
         _screenBinder.Bind(dto => dto.TransportName).To(btTransportName);
         _screenBinder.Bind(dto => dto.Description).To(htmlEditor);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btTransportName.ButtonClick += (o,e)=> OnEvent(_presenter.ChangeTransportName);
         btEditName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);

         btEditName.Properties.ReadOnly = true;
         btTransportName.Properties.ReadOnly = true;
      }

      public void AttachPresenter(IEditTransporterMoleculeContainerPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(TransporterMoleculeContainerDTO dtoTransporterMoleculeContainer)
      {
         _screenBinder.BindToSource(dtoTransporterMoleculeContainer);
      }

      public void SetParameterView(IView view)
      {
         tabParameter.FillWith(view);
      }

      public void ShowParameters()
      {
         tabParameter.Show();
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

   }
}