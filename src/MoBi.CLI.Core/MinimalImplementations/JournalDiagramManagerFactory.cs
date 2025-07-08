using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using System;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class JournalDiagramManagerFactory : IJournalDiagramManagerFactory
   {
      public IDiagramManager<JournalDiagram> Create()
      {
         throw new NotSupportedException();
      }
   }
}
