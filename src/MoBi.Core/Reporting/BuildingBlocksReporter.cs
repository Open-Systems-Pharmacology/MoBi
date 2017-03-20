using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   public abstract class BuildingBlocksReporter<T> : OSPSuiteTeXReporter<IReadOnlyCollection<T>> where T : IWithName
   {
      private readonly IOSPSuiteTeXReporter<T> _buildingBlockReporter;
      private readonly StructureElement _structureElement;

      protected BuildingBlocksReporter(IOSPSuiteTeXReporter<T> buildingBlockReporter, string caption) : this(buildingBlockReporter, new Chapter(caption))
      {
      }

      protected BuildingBlocksReporter(IOSPSuiteTeXReporter<T> buildingBlockReporter, StructureElement structureElement)
      {
         _buildingBlockReporter = buildingBlockReporter;
         _structureElement = structureElement;
      }

      public override IReadOnlyCollection<object> Report(IReadOnlyCollection<T> buildingBlocksToReport, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         if (!buildingBlocksToReport.Any())
            return listToReport;

         listToReport.Add(_structureElement);
         buildTracker.Track(_structureElement);
         foreach (var buildingBlock in buildingBlocksToReport.OrderBy(o => o.Name))
            listToReport.AddRange(_buildingBlockReporter.Report(buildingBlock, buildTracker));

         return listToReport;
      }
   }
}