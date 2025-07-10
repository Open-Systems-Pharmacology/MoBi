using System;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;

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