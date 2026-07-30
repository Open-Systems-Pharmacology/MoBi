using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditApplicationMoleculeBuilderView : BaseUserControl, IEditApplicationMoleculeBuilderView
   {
      private IEditApplicationMoleculeBuilderPresenter _presenter;
      private ScreenBinder<ApplicationMoleculeBuilderDTO> _screenBinder;

      public EditApplicationMoleculeBuilderView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ApplicationMoleculeBuilderDTO>();
         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(dto => dto.RelativeContainerPath)
            .To(btContainerPath);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btContainerPath.ButtonClick += selectContainerPath;
      }

      public void Activate()
      {
         ActiveControl = btContainerPath;
      }

      private void OnValueUpdating<T>(ApplicationMoleculeBuilderDTO applicationMoleculeBuilder, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      public void AttachPresenter(IEditApplicationMoleculeBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(ApplicationMoleculeBuilderDTO dtoApplicationMoleculeBuilder)
      {
         _screenBinder.BindToSource(dtoApplicationMoleculeBuilder);
      }

      public void SetFormulaView(IView view)
      {
         panelFormula.FillWith(view);
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

      private void selectContainerPath(object sender, ButtonPressedEventArgs e)
      {
         OnEvent(() =>
         {
            var newPath = _presenter.SetObjectPath();
            btContainerPath.Text = newPath.ToString();
         });
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemContainerName.Text = AppConstants.Captions.ContainerPath.FormatForLabel();
         layoutItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutGroupFormula.Text = AppConstants.Captions.Formula;
      }
   }
}