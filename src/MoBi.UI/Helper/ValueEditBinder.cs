using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.Core;

using MoBi.Presentation.DTO;
using MoBi.UI.Views;

namespace MoBi.UI.Helper
{
   public class ValueEditBinder<TObject, TValueEditDTO> : ElementBinder<TObject, TValueEditDTO> where TValueEditDTO : ValueEditDTO
   {
      protected readonly ValueEdit _valueEdit;

      public ValueEditBinder(IPropertyBinderNotifier<TObject, TValueEditDTO> propertyBinder, ValueEdit valueEdit)
         : base(propertyBinder)
      {
         _valueEdit = valueEdit;
         _valueEdit.Changing += ValueInControlChanging;

         _valueEdit.ValueChanged += (o, e) => NotifyChange();
         _valueEdit.UnitChanged += (o, e) => NotifyChange();
         _valueEdit.Changed += NotifyChange;
      }

      public override Control Control
      {
         get { return _valueEdit; }
      }

      public override TValueEditDTO GetValueFromControl()
      {
         return GetValueFromSource();
      }

      public override void SetValueToControl(TValueEditDTO value)
      {
         _valueEdit.BindTo(value);
      }

      public override bool HasError
      {
         get { return _valueEdit.HasError; }
      }

      public override void Validate()
      {
         _valueEdit.ValidateControl();
      }
   }

   public static class ScreenToElementBinderExtensionsForValueEdit
   {
      public static IElementBinder<TObject, TValueEditDTO> To<TObject, TValueEditDTO>(
         this IScreenToElementBinder<TObject, TValueEditDTO> screenToElementBinder, ValueEdit valueEdit) where TValueEditDTO : ValueEditDTO
      {
         var element = new ValueEditBinder<TObject, TValueEditDTO>(screenToElementBinder.PropertyBinder, valueEdit);
         screenToElementBinder.ScreenBinder.AddElement(element);
         return element;
      }
   }
}