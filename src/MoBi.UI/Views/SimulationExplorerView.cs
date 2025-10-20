using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
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
      private readonly IModuleExplorerPresenter _moduleExplorerPresenter;

      public SimulationExplorerView(IImageListRetriever imageListRetriever,
         IModuleExplorerPresenter moduleExplorerPresenter) : base(imageListRetriever)
      {
         _moduleExplorerPresenter = moduleExplorerPresenter;
         InitializeComponent();
         treeView.CompareNodeValues += compareNodeValues;
      }

      public void AttachPresenter(ISimulationExplorerPresenter presenter)
      {
         base.AttachPresenter(presenter);
      }


      public override ITreeNode AddNode(ITreeNode nodeToAdd)
      {
         return base.AddNode(nodeToAdd);
      }


      private void compareNodeValues(object sender, CompareNodeValuesEventArgs e)
      {
         //we only want to sort for the top nodes (level 0)
         if (e.Node1 == null)
            return;

         //we do not want to sort the root nodes
         if (e.Node1.Level == 0)
            e.Result = 0;

         //we need to sort the module nodes exactly as we do on the ModuleExplorerView.cs
         if (nodeIsModuleNode(e.Node1.ParentNode))
            e.Result = _moduleExplorerPresenter.OrderingComparisonFor(e.Node1.Tag as ITreeNode<IWithName>, e.Node2.Tag as ITreeNode<IWithName>);

         //we do not want to sort the items under the simulation node (i.e. no children). Otherwise, Nodes are sorted alphabetically
         else if (nodeIsSimulationNode(e.Node1.ParentNode))
            e.Result = 0;
      }

      private bool nodeIsSimulationNode(TreeListNode node) => node != null && node.Tag.IsAnImplementationOf<SimulationNode>();
      private bool nodeIsModuleNode(TreeListNode node) => node != null && node.Tag.IsAnImplementationOf<ModuleConfigurationNode>();
   }
}