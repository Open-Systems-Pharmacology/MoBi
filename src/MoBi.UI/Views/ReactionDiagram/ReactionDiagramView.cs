using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Presenters;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views.ReactionDiagram
{
   public class ReactionDiagramView : DevExpressDiagramView, IReactionDiagramView
   {
      private ReactionDiagramPresenter _reactionDiagramPresenter;

      public ReactionDiagramView(IImageListRetriever imageListRetriever)
         : base(imageListRetriever)
      {
      }

      public void AttachPresenter(IReactionDiagramPresenter presenter)
      {
         _reactionDiagramPresenter = presenter as ReactionDiagramPresenter;
         base.AttachPresenter(presenter);
      }

      public void AttachPresenter(IMoBiBaseDiagramPresenter presenter)
      {
         base.AttachPresenter(presenter);
      }

      public bool IsMoleculeNode(IBaseNode baseNode) => baseNode is MoleculeNode;

      public void ExpandParents(IBaseNode baseNode)
      {
         baseNode.GetParentNodes().Each(parent => parent.IsExpanded = true);
      }

      protected override void OnLinkCreated(IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort)
      {
         _reactionDiagramPresenter.Link(fromNode, toNode, fromPort, toPort);
      }

      protected override void OnSelectionDeleting(IReadOnlyList<IBaseNode> nodes, IReadOnlyList<IBaseLink> links)
      {
         if (anyMoleculesOrReactions(nodes))
            _reactionDiagramPresenter.RemoveSelection(nodes);
         else
            links.OfType<ReactionLink>().Each(unlink);
      }

      protected override void OnNodeDoubleClicked(IBaseNode node)
      {
         _reactionDiagramPresenter.ModelSelect(node.Id);
      }

      private void unlink(ReactionLink link)
      {
         var fromNode = link.GetFromNode();
         var toNode = link.GetToNode();
         _reactionDiagramPresenter.Unlink(fromNode, toNode, portFor(fromNode, link), portFor(toNode, link));
      }

      private static object portFor(IBaseNode node, ReactionLink link) => node is ReactionNode ? (object) link.Type : null;

      private static bool anyMoleculesOrReactions(IReadOnlyList<IBaseNode> nodes)
      {
         return nodes.Any(x => x is MoleculeNode || x is ReactionNode);
      }
   }
}
