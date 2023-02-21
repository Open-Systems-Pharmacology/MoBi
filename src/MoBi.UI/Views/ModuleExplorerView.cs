using DevExpress.XtraTreeList;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ModuleExplorerView : BaseExplorerView, IModuleExplorerView
   {
      private IModuleExplorerPresenter _moduleExplorerPresenter;

      public ModuleExplorerView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
         InitializeComponent();
         treeView.CompareNodeValues += compareNodeValues;
      }

      public void AttachPresenter(IModuleExplorerPresenter presenter)
      {
         _moduleExplorerPresenter = presenter;
         base.AttachPresenter(presenter);
      }

      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         //we only want to sort for the top nodes (level 0)
         if (e.Node1 == null)
            return;

         //we do not want to sort the root nodes or if the presenter indicates no sort
         if (e.Node1.Level == 0 || !_moduleExplorerPresenter.ShouldSort(e.Node1.Tag as ITreeNode))
            e.Result = 0;
      }
   }
}