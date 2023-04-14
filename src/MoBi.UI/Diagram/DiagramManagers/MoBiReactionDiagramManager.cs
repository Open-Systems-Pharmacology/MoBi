using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Diagram.Managers;

namespace MoBi.UI.Diagram.DiagramManagers
{
   public class MoBiReactionDiagramManager : ReactionDiagramManager<MoBiReactionBuildingBlock>, IMoBiReactionDiagramManager
   {
      public override IDiagramManager<MoBiReactionBuildingBlock> Create()
      {
         return new MoBiReactionDiagramManager();
      }
   }
}