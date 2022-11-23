using MoBi.Assets;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditExpressionProfileBuildingBlockView : EditBuildingBlockBaseView, IEditExpressionProfileBuildingBlockView
   {
      public EditExpressionProfileBuildingBlockView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }


      public void AttachPresenter(IEditExpressionProfileBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddExpressionProfileView(IView baseView)
      {
         tabEditBuildingBlock.FillWith(baseView);
         EditCaption = AppConstants.Captions.Parameters;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.ExpressionProfile;
      }
   }
}
