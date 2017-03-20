using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.UI.Diagram.Managers;
using OSPSuite.UI.Diagram.Services;

namespace MoBi.BatchTool.Services
{
   public class BatchJournalDiagramManagerFactory : IJournalDiagramManagerFactory
   {
      public IDiagramManager<JournalDiagram> Create()
      {
         return new JournalDiagramManager(new DiagramToolTipCreator());
      }
   }
}