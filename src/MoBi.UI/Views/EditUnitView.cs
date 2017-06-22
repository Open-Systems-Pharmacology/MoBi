using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditUnitView : BaseUserControl, IEditUnitView
   {
      private IEditUnitPresenter _presenter;
      private readonly ScreenBinder<Unit> _screenBinder;

      public EditUnitView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<Unit>();
      }

      public void AttachPresenter(IEditUnitPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(Unit unit)
      {
         BindToSource(unit);
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.Name).To(edName).OnValueUpdating += onPropertyValueSet;
         _screenBinder.Bind(x => x.Factor).To(edFactor).OnValueUpdating += onPropertyValueSet;
         _screenBinder.Bind(x => x.Offset).To(edOffset).OnValueUpdating += onPropertyValueSet;
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void onPropertyValueSet<T>(Unit unit, PropertyValueSetEventArgs<T> e)
      {
         _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.NewValue);
      }

      public void BindToSource(Unit unit)
      {
         _screenBinder.BindToSource(unit);
      }
   }
}