using MoBi.Presentation.DTO;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.Nodes
{
   public class HistoricalResultsNode : ObjectWithIdAndNameNode<DataRepository>, IViewItem
   {
      public HistoricalResultsNode(DataRepository dataRepository)
         : base(dataRepository)
      {
      }
   }

   public class EventNode : ObjectWithIdAndNameNode<IObjectBaseDTO>
   {
      public EventNode(IObjectBaseDTO tag) : base(tag)
      {
      }
   }

   public class ChartNode : ObjectWithIdAndNameNode<CurveChart>, IViewItem
   {
      public ChartNode(CurveChart chart)
         : base(chart)
      {
      }
   }
}