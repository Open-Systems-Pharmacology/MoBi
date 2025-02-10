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
   public partial class EditEventAssignmentBuilderView : BaseUserControl, IEditEventAssignmentBuilderView
   {
      private IEditAssignmentBuilderPresenter _presenter;
      private readonly ScreenBinder<EventAssignmentBuilderDTO> _screenBinder = new ScreenBinder<EventAssignmentBuilderDTO>();

      public EditEventAssignmentBuilderView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(dto => dto.Name)
            .To(tbName)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(dto => dto.Dimension)
            .To(cbDimension)
            .WithValues(dto => _presenter.AllDimensions())
            .OnValueUpdated += (o, e) => OnEvent(_presenter.SetDimension, e);

         _screenBinder.Bind(dto => dto.ChangedEntityPath)
            .To(btnTargetPath)
            .OnValueUpdated += (o, e) => OnEvent(_presenter.SetEventAssignmentPath, e);

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
         layoutItemDimension.Text = AppConstants.Captions.Dimension.FormatForLabel();
         layoutGroupAssignment.Text = AppConstants.Captions.Assignment;
      }

      private void OnValueUpdating<T>(EventAssignmentBuilderDTO eventAssignmentBuilder, PropertyValueSetEventArgs<T> e) => 
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));

      public void SetFormulaView(IView subView) => pnlFormula.FillWith(subView);

      public void ValidateAll() => _screenBinder.Validate();

      public void AttachPresenter(IEditAssignmentBuilderPresenter presenter) => _presenter = presenter;

      public void Show(EventAssignmentBuilderDTO eventAssignmentBuilderDTO) => _screenBinder.BindToSource(eventAssignmentBuilderDTO);

      public override bool HasError => base.HasError || _screenBinder.HasError;

      public void Activate() => ActiveControl = tbName;
   }
}