using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   public partial class EditEventBuilderView : BaseUserControl, IEditEventBuilderView
   {
      private IEditEventBuilderPresenter _presenter;
      private ScreenBinder<EventBuilderDTO> _screenBinder;
      private GridViewBinder<EventAssignmentBuilderDTO> _gridBinder;

      public EditEventBuilderView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         tabParameters.Text = AppConstants.Captions.Parameters;
         tabProperties.Text = AppConstants.Captions.Properties;
         layoutGroupAssignment.Text = AppConstants.Captions.Assignment;
         layoutGroupCondition.Text = AppConstants.Captions.Condition;
         layoutControlItemAddFormula.AdjustLongButtonSize();
         layoutControlItemAddAssignment.AdjustLongButtonSize();
         layoutControlItemDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutControlItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         chkOneTime.Text = AppConstants.Captions.OneTimeEvent;
         btnAddFormula.Text = AppConstants.Captions.AddFormula;
         btnAddFormula.Image = ApplicationIcons.Add;
         btnAddAssignment.Text = AppConstants.Captions.AddAssignment;
         btnAddAssignment.Image = ApplicationIcons.Add;
         btnAddFormula.Click += (o, e) => OnEvent(_presenter.AddConditionFormula);
         btnAddAssignment.Click += (o, e) => OnEvent(_presenter.AddAssigment);
         htmlEditor.Properties.ShowIcon = false;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder = new ScreenBinder<EventBuilderDTO>();
         _screenBinder.Bind(dto => dto.Name).To(btName).OnValueUpdating += onPropertyValueSet;
         _screenBinder.Bind(dto => dto.Description).To(htmlEditor).OnValueUpdating += onPropertyValueSet;
         _screenBinder.Bind(dto => dto.OneTime).To(chkOneTime).OnValueUpdating += onPropertyValueSet;
         _screenBinder.Bind(dto => dto.Condition).To(cmbCondition)
            .WithValues(dto => _presenter.AllFormulaNames())
            .OnValueUpdating += onConditionFormulaNameSet;

         _gridBinder = new GridViewBinder<EventAssignmentBuilderDTO>(grdAssingments);
         var selectButtonrepository = createSelectButtonRepository();
         selectButtonrepository.ButtonClick += (o, e) => onSelectButtonClick(_gridBinder.FocusedElement);
         _gridBinder.Bind(dto => dto.ChangedEntityPath)
            .WithCaption(AppConstants.Captions.ChangedEntityPath)
            .WithRepository(d => selectButtonrepository);
         _gridBinder.Bind(dto => dto.NewFormula)
            .WithCaption(AppConstants.Captions.NewFormula)
            .WithRepository(d => getFormualReposititory(grdAssingments))
            .WithOnValueUpdating(onAssingmentFormulaSet);
         _gridBinder.Bind(dto => dto.UseAsValue)
            .WithCaption(AppConstants.Captions.UseAsValue)
            .OnValueUpdating += onAssignmentPropertySet;

         var buttonRepository = createAddRemoveButtonRepository();
         _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => buttonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH * 2);
         buttonRepository.ButtonClick += (o, e) => onButtonClicked(e,_gridBinder.FocusedElement);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void onAssingmentFormulaSet(EventAssignmentBuilderDTO eventAssignmentBuilder, PropertyValueSetEventArgs<FormulaBuilderDTO> e)
      {
         OnEvent(() => _presenter.SetFormulaFor(eventAssignmentBuilder, e.NewValue));
      }

      private void onAssignmentPropertySet<T>(EventAssignmentBuilderDTO eventAssignmentBuilder, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFor(eventAssignmentBuilder, e.PropertyName, e.NewValue, e.OldValue));
      }

      private void onSelectButtonClick(EventAssignmentBuilderDTO eventAssignmentBuilderDTO)
      {
         OnEvent(() => _presenter.SetTargetPathFor(eventAssignmentBuilderDTO));
      }

      private RepositoryItemButtonEdit createSelectButtonRepository()
      {
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.DisableTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Ellipsis;
         return buttonRepository;
      }

      private RepositoryItemButtonEdit createAddRemoveButtonRepository()
      {
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.HideTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Plus;
         buttonRepository.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
         return buttonRepository;
      }

      private void onButtonClicked(ButtonPressedEventArgs buttonPressedEventArgs, EventAssignmentBuilderDTO parameterToEdit)
      {
         var pressedButton = buttonPressedEventArgs.Button;
         if (pressedButton.Kind.Equals(ButtonPredefines.Plus))
         {
            OnEvent(() => _presenter.AddAssigment());
         }
         else
         {
            OnEvent(() => _presenter.RemoveAssignment(parameterToEdit));
         }
      }

      private void onPropertyValueSet<T>(EventBuilderDTO eventBuilder, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      private RepositoryItem getFormualReposititory(GridView gridView)
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.GetFormulas());
         return repository;
      }

      public void AttachPresenter(IEditEventBuilderPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(EventBuilderDTO dtoEventBuilder)
      {
         _screenBinder.BindToSource(dtoEventBuilder);
         _gridBinder.BindToSource(dtoEventBuilder.Assignments);
         initNameControl(dtoEventBuilder);
      }

      private void onConditionFormulaNameSet(object sender, EventArgs e)
      {
         OnEvent(() => _presenter.SetConditionFormula(cmbCondition.Text));
      }

      private void initNameControl(EventBuilderDTO dtoEventBuilder)
      {
         var isNewBuilder = dtoEventBuilder.Name.IsNullOrEmpty();
         btName.Properties.ReadOnly = !isNewBuilder;
         btName.Properties.Buttons[0].Visible = !isNewBuilder;
      }

      public void SetParametersView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public void SetFormulaView(IView view)
      {
         splitContainerControl.Panel1.FillWith(view);
      }

      public void SetSelectReferenceView(IView view)
      {
         splitContainerControl.Panel2.FillWith(view);
      }

      public void NameValidationError(string message)
      {
         OnValidationError(cmbCondition, message);
      }

      public void ResetNameValidationError()
      {
         OnClearError(cmbCondition);
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public override bool HasError
      {
         get { return base.HasError || _screenBinder.HasError; }
      }

      public void Activate()
      {
         ActiveControl = btName;
      }
   }
}