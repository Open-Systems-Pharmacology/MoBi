using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditOutputSchemaView : BaseUserControl, IEditOutputSchemaView
   {
      private GridViewBinder<OutputIntervalDTO> _gridViewBinder;
      private IEditOutputSchemaPresenter _presenter;
      private readonly UxComboBoxUnit<ParameterDTO> _comboBoxUnit;

      public EditOutputSchemaView()
      {
         InitializeComponent();
         _comboBoxUnit = new UxComboBoxUnit<ParameterDTO>(gridControl);
      }

      public override void InitializeBinding()
      {
         _gridViewBinder = new GridViewBinder<OutputIntervalDTO>(gridViewIntervals) { BindingMode = BindingMode.OneWay, ValidationMode = ValidationMode.LeavingRow };

         _gridViewBinder.Bind(dto => dto.StartTime)
            .WithFormat(x => x.StartTimeParameter.ParameterFormatter())
            .WithCaption(AppConstants.Captions.StartTime)
            .WithEditorConfiguration((activeEditor, intervalDTO) => _comboBoxUnit.UpdateUnitsFor(activeEditor, intervalDTO.StartTimeParameter as ParameterDTO))
            .OnValueUpdating += (o, e) => setParameterValue(o.StartTimeParameter, e.NewValue);

         _gridViewBinder.Bind(dto => dto.EndTime)
            .WithFormat(x => x.EndTimeParameter.ParameterFormatter())
            .WithCaption(AppConstants.Captions.EndTime)
            .WithEditorConfiguration((activeEditor, intervalDTO) => _comboBoxUnit.UpdateUnitsFor(activeEditor, intervalDTO.EndTimeParameter as ParameterDTO))
            .OnValueUpdating += (o, e) => setParameterValue(o.EndTimeParameter, e.NewValue);

         _gridViewBinder.Bind(dto => dto.Resolution)
            .WithFormat(x => x.ResolutionParameter.ParameterFormatter())
            .WithEditorConfiguration((activeEditor, intervalDTO) => _comboBoxUnit.UpdateUnitsFor(activeEditor, intervalDTO.ResolutionParameter as ParameterDTO))
            .OnValueUpdating += (o, e) => setParameterValue(o.ResolutionParameter, e.NewValue);

         _gridViewBinder.AddUnboundColumn()
            .WithRepository(dto => createAddRemoveButtons())
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithFixedWidth(2 * OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _comboBoxUnit.ParameterUnitSet += setParameterUnit;
         gridViewIntervals.HiddenEditor += (o, e) => { _comboBoxUnit.Visible = false; };

         btnAddOutputInterval.Click += (o, e) => OnEvent(_presenter.AddOutputInterval);
      }

      private void setParameterValue(IParameterDTO parameterDTO, double newValue)
      {
         this.DoWithinExceptionHandler(() => _presenter.SetParameterValue(parameterDTO, newValue));
      }

      private void setParameterUnit(IParameterDTO parameterDTO, Unit newUnit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridViewIntervals.CloseEditor();
            _presenter.SetParameterUnit(parameterDTO, newUnit);
         });
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutGroupIntervals.Text = AppConstants.Captions.OutputIntervals;
         Caption = AppConstants.Captions.OutputIntervals;
         btnAddOutputInterval.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddInterval);
         layoutControlItemAddInterval.AdjustButtonSize(layoutControl);
      }

      private RepositoryItem createAddRemoveButtons()
      {
         var buttonRepository = new UxRepositoryItemButtonEdit { TextEditStyle = TextEditStyles.HideTextEditor };
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Plus;
         buttonRepository.AddButton(ButtonPredefines.Delete);
         buttonRepository.ButtonClick += (o, e) => this.DoWithinExceptionHandler(() => onButtonClick(e));
         return buttonRepository;
      }

      private void onButtonClick(ButtonPressedEventArgs e)
      {
         if (e.Button.Kind.Equals(ButtonPredefines.Plus))
            _presenter.AddOutputInterval();
         else
            _presenter.RemoveOutputInterval(_gridViewBinder.FocusedElement);
      }

      public void AttachPresenter(IEditOutputSchemaPresenter presenter) => _presenter = presenter;

      public void Show(IEnumerable<OutputIntervalDTO> outputIntervalInfos) => _gridViewBinder.BindToSource(outputIntervalInfos);

      public bool ShowGroupCaption
      {
         set
         {
            layoutGroupIntervals.TextVisible = value;
            layoutGroupIntervals.GroupBordersVisible = value;
         }
      }

      public override ApplicationIcon ApplicationIcon => _presenter.Icon;
   }
}