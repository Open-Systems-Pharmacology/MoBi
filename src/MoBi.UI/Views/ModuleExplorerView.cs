using DevExpress.XtraTreeList;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
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
         e.Result = _moduleExplorerPresenter.OrderingComparisonFor(e.Node1.Tag as ITreeNode<IWithName>, e.Node2.Tag as ITreeNode<IWithName>);
      }
   }
}