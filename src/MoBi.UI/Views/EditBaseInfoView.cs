using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   public partial class EditBaseInfoView : BaseUserControl, IView<ICanEditPropertiesPresenter>
   {
      private ScreenBinder<ObjectBaseDTO> _screenBinder;
      private ICanEditPropertiesPresenter _presenter;
      private bool _readOnly;

      public EditBaseInfoView()
      {
         InitializeComponent();
         ReadOnly = false;
         initialiseBinding();
      }

      private void initialiseBinding()
      {
         _screenBinder = new ScreenBinder<ObjectBaseDTO>();
         _screenBinder.Bind(item => item.Description).To(htmlEditor).OnValueUpdating += onPropertySet;
         _screenBinder.Bind(item => item.Name).To(bttxtName).OnValueUpdating += onPropertySet;
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void onPropertySet<T>(ObjectBaseDTO arg1, PropertyValueSetEventArgs<T> arg2)
      {
         _presenter.SetPropertyValueFromView(arg2.PropertyName, arg2.NewValue, arg2.OldValue);
      }

      public void AttachPresenter(ICanEditPropertiesPresenter presenter)
      {
         _presenter = presenter;
      }

      public bool ReadOnly
      {
         get { return _readOnly; }
         set
         {
            _readOnly = value;
            htmlEditor.Enabled = !value;
            bttxtName.Enabled = !value;
         }
      }

      public void BindToSource(ObjectBaseDTO dto)
      {
         _screenBinder.BindToSource(dto);
         bttxtName.Properties.ReadOnly = !dto.Name.IsNullOrEmpty();
         bttxtName.Properties.Buttons[0].Visible = !dto.Name.IsNullOrEmpty();
      }

      private void bttxtName_ButtonClick(object sender, ButtonPressedEventArgs e)
      {
         _presenter.RenameSubject();
      }
   }
}