using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class CreateBuildingBlockMergeView : BaseModalView, ICreateBuildingBlockMergeView
   {
      public CreateBuildingBlockMergeView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(ICreateBuildingBlockMergePresenter presenter)
      {
      }

      public void AddView(IView view)
      {
         panelView.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.MergeSimulationIntoProject;
         ApplicationIcon = ApplicationIcons.Merge;
      }
   }
}
