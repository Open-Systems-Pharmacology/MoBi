using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditInitialConditionsView : EditBuildingBlockBaseView, IEditInitialConditionsView
   {
      public EditInitialConditionsView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditInitialConditionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddInitialConditionsView(IView view)
      {
         tabEditBuildingBlock.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         EditCaption = AppConstants.Captions.InitialConditions;
         ApplicationIcon = ApplicationIcons.InitialConditions;
      }
   }
}