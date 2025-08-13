using System.ComponentModel;
using System.Linq;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views.BaseDiagram
{
   public partial class MoBiBaseDiagramView : BaseDiagramView, IMoBiBaseDiagramView
   {
      private IMoBiBaseDiagramPresenter _moBiDiagramPresenter;

      public MoBiBaseDiagramView(IImageListRetriever imageListRetriever)
         : base(imageListRetriever)
      {
         InitializeComponent();
      }

      public bool IsMoleculeNode(IBaseNode baseNode)
      {
         return baseNode.IsAnImplementationOf<MoleculeNode>();
      }

      public void ExpandParents(IBaseNode baseNode)
      {
         foreach (var parent in baseNode.GetParentNodes())
         {
            parent.IsExpanded = true;
         }
      }

      public void AttachPresenter(IMoBiBaseDiagramPresenter presenter)
      {
         base.AttachPresenter(presenter);
         _moBiDiagramPresenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _goView.LinkCreated += (o, e) => this.DoWithinExceptionHandler(() => onLinkCreated(e));
         _goView.SelectionDeleting += (o, e) => this.DoWithinExceptionHandler(() => OnSelectionDeleting(e));
         _goView.BackgroundContextClicked += (o, e) => this.DoWithinExceptionHandler(() => onBackgroundContextClicked(e));
         _goView.ObjectDoubleClicked += (o, e) => this.DoWithinExceptionHandler(() => onDoubleClicked(e));
      }

      private void onLinkCreated(GoSelectionEventArgs e)
      {
         var link = (NewLink)e.GoObject;
         var node1 = (IBaseNode)link.FromNode;
         var node2 = (IBaseNode)link.ToNode;
         var portObject1 = link.FromPort.UserObject;
         var portObject2 = link.ToPort.UserObject;

         _moBiDiagramPresenter.Link(node1, node2, portObject1, portObject2);
         _goView.Document.Remove(link);
      }

      protected virtual void OnSelectionDeleting(CancelEventArgs e)
      {
         removeLinks(_goView.Selection);
         e.Cancel = true;
      }

      private void removeLinks(GoSelection links)
      {
         links.ToList().Each(link =>
         {
            if (isBaseLink(link) && isGoLink(link))
               unlinkBaseNodes(link as IBaseLink, link as GoLink);
            else if (link.IsAnImplementationOf<INeighborhoodNode>())
               unlinkNeighborhoodNode(link);
         });
      }

      private void unlinkBaseNodes(IBaseLink baseLink, GoLink goLink) => _moBiDiagramPresenter.Unlink(baseLink.GetFromNode(), baseLink.GetToNode(), goLink.FromPort.UserObject, goLink.ToPort.UserObject);

      private void unlinkNeighborhoodNode(GoObject itemToDelete)
      {
         var neighborhoodNode = (INeighborhoodNode)itemToDelete;
         _moBiDiagramPresenter.Unlink(neighborhoodNode.FirstNeighbor, neighborhoodNode.SecondNeighbor, null, null);
      }

      private static bool isGoLink(GoObject goLink) => goLink is GoLink;

      private static bool isBaseLink(GoObject baseLink) => baseLink is IBaseLink;

      private void onBackgroundContextClicked(GoInputEventArgs e)
      {
         _moBiDiagramPresenter.ShowContextMenu(null, e.ViewPoint, e.DocPoint);
      }

      private void onDoubleClicked(GoObjectEventArgs e)
      {
         if (e.GoObject == null) return;
         var baseNode = e.GoObject as IBaseNode;
         if (baseNode == null && e.GoObject.Parent != null)
            baseNode = e.GoObject.Parent as IBaseNode;
         if (baseNode == null) return;
         _moBiDiagramPresenter.ModelSelect(baseNode.Id);
      }
   }
}