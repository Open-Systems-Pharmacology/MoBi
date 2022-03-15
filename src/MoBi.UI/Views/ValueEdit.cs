using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using DevExpress.XtraEditors.DXErrorProvider;

using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ValueEdit : BaseUserControl
   {
      private ScreenBinder<ValueEditDTO> _screenBinder;
      private ValueEditDTO _valueEditDTO;

      /// <summary>
      ///    Event is raised whenever a value is being changed in the user control
      /// </summary>
      public event Action Changing = delegate { };

      /// <summary>
      ///    Event is raised whenever a value has changed
      /// </summary>
      public event Action Changed = delegate { };

      public event Action<ValueEditDTO, double> ValueChanged = delegate { };
      public event Action<ValueEditDTO, Unit> UnitChanged = delegate { };

      public ValueEdit()
      {
         InitializeComponent();
         InitializeBinding();
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<ValueEditDTO>();

         _screenBinder.Bind(p => p.Value)
            .To(tbValue).OnValueUpdating += onValueUpdating;

         _screenBinder.Bind(p => p.DisplayUnit).To(cbUnit)
            .WithValues(p => p.Dimension.Units)
            .OnValueUpdating += (o, e) => UnitChanged(o, e.NewValue);

         _screenBinder.Changed += () => Changed();

         RegisterValidationFor(_screenBinder, () => Changing());

         tbValue.EnterMoveNextControl = true;
         layoutItemUnit.TextVisible = false;
         layoutItemValue.TextVisible = false;
      }

      private void onValueUpdating(ValueEditDTO dto, PropertyValueSetEventArgs<double> e)
      {
         // Some artifact with this control when values have a lot of digits could lead to an event being raised even though nothing has changed.
         // This should be fixed in OSPSuite.DataBinding but we'll be done at another time
         if (ValueComparer.AreValuesEqual(e.OldValue, e.NewValue))
            return;

         ValueChanged(dto, e.NewValue);
      }

      public override bool HasError => _screenBinder.HasError;

      public void ValidateControl()
      {
         _screenBinder.Validate();
      }

      public string ToolTip
      {
         set => tbValue.ToolTip = value;
         get => tbValue.ToolTip;
      }

      public void SetError(string errorMessage)
      {
         errorProvider.SetError(tbValue, errorMessage);
      }

      public void SetWarning(string errorMessage)
      {
         warningProvider.SetError(tbValue, errorMessage, ErrorType.Warning);
      }

      public void BindTo(ValueEditDTO value)
      {
         if(Equals(value, _valueEditDTO))
            _screenBinder.Update();
         else
         {
            _valueEditDTO = value;
            _screenBinder.BindToSource(_valueEditDTO);
         }
      }

      public void Rebind()
      {
         if (_valueEditDTO == null) return;
         _screenBinder.BindToSource(_valueEditDTO);
      }
   }
}