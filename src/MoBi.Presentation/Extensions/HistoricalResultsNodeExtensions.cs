using OSPSuite.Utility.Extensions;
using MoBi.Presentation.Nodes;

namespace MoBi.Presentation.Extensions
{
   public static class HistoricalResultsNodeExtensions
   {
      public static bool BelongsToSimulation(this HistoricalResultsNode historicalResultNode)
      {
         return historicalResultNode != null && historicalResultNode.ParentNode.IsAnImplementationOf<SimulationNode>();
      }
   }
}
