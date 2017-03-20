using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.UI;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Views.BaseDiagram;
using Northwoods.Go;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views.ReactionDiagram
{
   public class ReactionDiagramView : MoBiBaseDiagramView, IReactionDiagramView
   {
      private IReactionDiagramPresenter _reactionDiagramPresenter;

      public ReactionDiagramView(IImageListRetriever imageListRetriever)
         : base(imageListRetriever)
      {
      }

      public void AttachPresenter(IReactionDiagramPresenter presenter)
      {
         _reactionDiagramPresenter = presenter;
         base.AttachPresenter(presenter);
      }

      protected override void OnSelectionDeleting(CancelEventArgs e)
      {
         var goObjects = _goView.Selection.ToList();
         // Any nodes that match our types and we process this delete
         if(anyMoleculesOrReactions(goObjects))
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