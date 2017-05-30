using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Diagram.Managers;

namespace MoBi.UI.Diagram.DiagramManagers
{
   public class MoBiReactionDiagramManager : ReactionDiagramManager<IMoBiReactionBuildingBlock>, IMoBiReactionDiagramManager
   {
      public override IDiagramManager<IMoBiReactionBuildingBlock> Create()
      {
         return new MoBiReactionDiagramManager();
      }
   }
}