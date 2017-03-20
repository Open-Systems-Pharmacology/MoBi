using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Services;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Diagram.Elements;

namespace MoBi.UI.Services
{
   public class DiagramUpdater : IDiagramUpdater
   {
      public void RemoveMoleculeNodeFromDiagram(string moleculeNodeId, IMoBiReactionDiagramManager reactionDiagramManager, IDiagramModel diagramModel)
      {
         var node = diagramModel.FindByName(moleculeNodeId);
         reactionDiagramManager.RemoveMoleculeNode(diagramModel.GetNode<MoleculeNode>(moleculeNodeId));
      }
   }
}