using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
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
   public partial class EditEventAssignmentBuilderView : BaseUserControl, IEditEventAssignmentBuilderView
   {
      private IEditAssignmentBuilderPresenter _presenter;
      private ScreenBinder<EventAssignmentBuilderDTO> _screenBinder;

      public EditEventAssignmentBuilderView()
      {
         InitializeComponent();
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_Events;

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<EventAssignmentBuilderDTO>();
         _screenBinder.Bind(dto => dto.Name)
            .To(btnName)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(dto => dto.ChangedEntityPath)
            .To(btnTargetPath);

         _screenBinder.Bind(dto => dto.UseAsValue)
            .To(chkUseAsValue)
            .OnValueUpdating += OnValueUpdating;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         btnTargetPath.ButtonClick += (o, e) => OnEvent(_presenter.SelectPath);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkUseAsValue.Text = AppConstants.Captions.UseAsValue;
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutItemChangedEntity.Text = AppConstants.Captions.ChangedEntity.FormatForLabel();
         layoutItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutGroupAssignment.Text = AppConstants.Captions.Assignment;
         btnTargetPath.Properties.ReadOnly = true;
      }

      private void OnValueUpdating<T>(EventAssignmentBuilderDTO eventAssignmentBuilder, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      public void SetFormulaView(IView subView)
      {
         pnlFormula.FillWith(subView);
      }

      public string TargetPath
      {
         get { return btnTargetPath.Text; }
         set { btnTargetPath.Text = value; }
      }

      public void AttachPresenter(IEditAssignmentBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(EventAssignmentBuilderDTO eventAssignmentBuilderDTO)
      {
         _screenBinder.BindToSource(eventAssignmentBuilderDTO);
      }

      public override bool HasError => base.HasError || _screenBinder.HasError;

      public void Activate()
      {
         ActiveControl = btnName;
      }
   }
}