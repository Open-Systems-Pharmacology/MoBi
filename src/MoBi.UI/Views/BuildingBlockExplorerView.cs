using OSPSuite.UI;
using DevExpress.XtraTreeList;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class BuildingBlockExplorerView : BaseExplorerView, IBuildingBlockExplorerView
   {
      public BuildingBlockExplorerView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
         InitializeComponent();
         treeView.CompareNodeValues += compareNodeValues;
      }

      protected override int TopicId => HelpId.MoBi_Quicktour;

      public void AttachPresenter(IBuildingBlockExplorerPresenter presenter)
      {
         base.AttachPresenter(presenter);
      }

      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         //we only want to sort for the top nodes (level 0)
         if (e.Node1 == null)
            return;

         //we do not want to sort the root nodes
         if (e.Node1.Level == 0)
            e.Result = 0;

      }
   }
}