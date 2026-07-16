using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Presenters;
using MoBi.UI.Views.BaseDiagram;
using Northwoods.Go;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views.ReactionDiagram
{
   public class ReactionDiagramView : MoBiBaseDiagramView, IReactionDiagramView
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

      protected override void OnSelectionDeleting(CancelEventArgs e)
      {
         var goObjects = _goView.Selection.ToList();
         // Any nodes that match our types and we process this delete
         if (anyMoleculesOrReactions(goObjects))
            _reactionDiagramPresenter.RemoveSelection(goObjects);
         // Otherwise pass requests to delete links and other items to the base class
         else
            base.OnSelectionDeleting(e);

         e.Cancel = true;
      }

      private static bool anyMoleculesOrReactions(List<GoObject> goObjects)
      {
         return goObjects.Any(x => x.IsAnImplementationOf<MoleculeNode>()) || goObjects.Any(x => x.IsAnImplementationOf<ReactionNode>());
      }
   }
}