using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   public abstract class BuildingBlockReporter<TBuildingBlock, TBuilder> : OSPSuiteTeXReporter<TBuildingBlock> where TBuildingBlock : IBuildingBlock<TBuilder> where TBuilder : class, IBuilder
   {
      private readonly string _caption;
      private readonly string _chapterCaption;

      protected BuildingBlockReporter(string caption, string chapterCaption)
      {
         _caption = caption;
         _chapterCaption = chapterCaption;
      }

      public override IReadOnlyCollection<object> Report(TBuildingBlock buildingBlock, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         
         if (noChapterCreatedYet(buildTracker))
            listToReport.Add(new Chapter(_chapterCaption));

         listToReport.Add(new Section(string.Format(Constants.BUILDING_BLOCK_PROMPT_FORMAT, _caption, buildingBlock.Name)));
         listToReport.AddRange(this.ReportDescription(buildingBlock, buildTracker));

         AddBuildersReport(buildingBlock, listToReport,buildTracker);
         return listToReport;
      }

      private static bool noChapterCreatedYet(OSPSuiteTracker buildTracker)
      {
         return !buildTracker.TrackedObjects.Any(s => s.IsAnImplementationOf<Chapter>());
      }

      protected virtual void AddBuildersReport(TBuildingBlock buildingBlock, List<object> listToReport, OSPSuiteTracker buildTracker)
      {
         foreach (var builder in buildingBlock.OrderBy(o => o.Name))
         {
            listToReport.Add(new SubSection(builder.Name));
            listToReport.Add(builder);
         }
      }
   }
}