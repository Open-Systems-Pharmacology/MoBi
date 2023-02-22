using DevExpress.XtraEditors;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class NewNameExpressionProfileBuildingBlockView : BaseModalView, INewNameExpressionProfileBuildingBlockView
   {
      private readonly ScreenBinder<RenameExpressionProfileDTO> _screenBinder = new ScreenBinder<RenameExpressionProfileDTO>();
      public NewNameExpressionProfileBuildingBlockView()
      {
         InitializeComponent();
         ExtraVisible = false;
         tbName.ReadOnly = true;
         MaximizeBox = false;
         MinimizeBox = false;
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.Category).To(tbCategory);
         _screenBinder.Bind(x => x.MoleculeName).To(tbMoleculeName);
         _screenBinder.Bind(x => x.Species).To(tbSpecies);
         _screenBinder.Bind(x => x.Name).To(tbName);
         _screenBinder.SavingMode = SavingMode.Always;

         // We need to validate the text box when the text changes so that the Name property is validated
         // even though that text box is readonly. The Name is composed of each of the other text boxes content
         tbCategory.TextChanged += (o,e) => OnEvent(() => validateTextBox(o));
         tbMoleculeName.TextChanged += (o,e) => OnEvent(() => validateTextBox(o));
         tbSpecies.TextChanged += (o,e) => OnEvent(() => validateTextBox(o));

         RegisterValidationFor(_screenBinder);
      }

      private void validateTextBox(object source)
      {
         if(source is TextEdit textEdit)
            textEdit.DoValidate();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblSpecies.Text = Captions.Species;
         lblCategory.Text = Captions.Category;
         lblName.Text = Captions.Name;
         ApplicationIcon = ApplicationIcons.Rename;
      }

      public void AttachPresenter(INewNameForExpressionProfileBuildingBlockPresenter presenter)
      {
         // nothing to do here
      }

      public void BindTo(RenameExpressionProfileDTO dto)
      {
         lblNameType.Text = dto.Type;
         _screenBinder.BindToSource(dto);
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}
