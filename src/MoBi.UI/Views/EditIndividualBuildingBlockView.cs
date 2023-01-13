using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditIndividualBuildingBlockView : EditBuildingBlockBaseView, IEditIndividualBuildingBlockView
   {
      public EditIndividualBuildingBlockView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditIndividualBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddIndividualView(IView baseView)
      {
         tabEditBuildingBlock.FillWith(baseView);
         EditCaption = AppConstants.Captions.Parameters;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.Individual;
      }
   }
}