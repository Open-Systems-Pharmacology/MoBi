using OSPSuite.Core.Diagram;

namespace MoBi.BatchTool.Services
{
   public class BatchReactionDiagramModelFactory : IReactionDiagramModelFactory
   {
      public IDiagramModel Create()
      {
         return new BatchDiagramModel();
      }
   }
}
