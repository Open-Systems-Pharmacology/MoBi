using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class SimulationExplorerView : BaseExplorerView, ISimulationExplorerView
   {
      private IExplorerPresenter _explorerPresenter;

      public SimulationExplorerView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
         InitializeComponent();
         treeView.CompareNodeValues += compareNodeValues;
      }

      public void AttachPresenter(ISimulationExplorerPresenter presenter)
      {
         base.AttachPresenter(presenter);
         _explorerPresenter = presenter;
      }

      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         if (e.Node1 == null)
            return;

         if (e.Node1.Level == 0)
            e.Result = 0;

         e.Result = _explorerPresenter.TryGetOrderingComparison(e.Node1.ParentNode?.Tag, e.Node1.Tag as ITreeNode<IWithName>, e.Node2.Tag as ITreeNode<IWithName>); 
      }
    }
}