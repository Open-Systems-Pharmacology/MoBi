using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class NewFormulaView : BaseModalView, INewFormulaView
   {
      private ScreenBinder<ObjectBaseDTO> _screenBinder;

      public NewFormulaView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.NewFormula;
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutItemFormula.TextVisible = false;
      }

      public void AttachPresenter(INewFormulaPresenter presenter)
      {
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ObjectBaseDTO>();
         _screenBinder.Bind(dto => dto.Name).To(txtName);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError; }
      }

      public void AddReferenceView(IView view)
      {
         splitFormulaEditor.Panel2.FillWith(view);
      }

      public void AddFormulaView(IView view)
      {
         splitFormulaEditor.Panel1.FillWith(view);
      }

      public void BindTo(ObjectBaseDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }
   }
}