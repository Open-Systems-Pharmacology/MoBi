using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views.BaseDiagram
{
   public class MoBiBaseDiagramView : DiagramView, IMoBiBaseDiagramView
   {
      private IMoBiBaseDiagramPresenter _moBiDiagramPresenter;

      public MoBiBaseDiagramView(IImageListRetriever imageListRetriever)
         : base(imageListRetriever)
      {
      }

      public bool IsMoleculeNode(IBaseNode baseNode) => baseNode is MoleculeNode;

      public void ExpandParents(IBaseNode baseNode)
      {
         baseNode.GetParentNodes().Each(parent => parent.IsExpanded = true);
      }

      public void AttachPresenter(IMoBiBaseDiagramPresenter presenter)
      {
         base.AttachPresenter(presenter);
         _moBiDiagramPresenter = presenter;
      }

      protected override void OnNodeDoubleClicked(IBaseNode node)
      {
         _moBiDiagramPresenter.ModelSelect(node.Id);
      }

      protected override void OnSelectionDeleting(IReadOnlyList<IBaseNode> nodes, IReadOnlyList<IBaseLink> links)
      {
         links.Each(link => _moBiDiagramPresenter.Unlink(link.GetFromNode(), link.GetToNode(), null, null));
         nodes.OfType<INeighborhoodNode>().Each(node => _moBiDiagramPresenter.Unlink(node.FirstNeighbor, node.SecondNeighbor, null, null));
      }

      protected override void OnLinkCreated(IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort)
      {
         _moBiDiagramPresenter.Link(fromNode, toNode, fromPort, toPort);
      }
   }
}