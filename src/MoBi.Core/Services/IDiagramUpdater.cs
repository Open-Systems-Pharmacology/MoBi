using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;

namespace MoBi.Core.Services
{
   public interface IDiagramUpdater
   {
      void RemoveMoleculeNodeFromDiagram(string moleculeNodeId, IMoBiReactionDiagramManager reactionDiagramManager, IDiagramModel diagramModel);
   }
}