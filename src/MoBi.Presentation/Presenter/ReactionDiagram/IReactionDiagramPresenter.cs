using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.BaseDiagram;
using MoBi.Presentation.Views.BaseDiagram;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter.ReactionDiagram
{
   public interface IReactionDiagramPresenter : IMoBiBaseDiagramPresenter<IMoBiReactionBuildingBlock>,
      IPresenter<IReactionDiagramView>
   {
      void SetDisplayEductsRightForDiagramSelection(bool displayEductsRight);
      void AddMoleculeNode();
      void LayerLayout(IContainerBase containerBase);
      void LayerLayout();
      void RemoveSelection(IReadOnlyList<GoObject> objectsToBeRemoved);
      void Select(IReactionBuilder reactionBuilder);
      bool IsReactionNode(IBaseNode node);
      bool DisplayEductsRight(IBaseNode node);
   }
}