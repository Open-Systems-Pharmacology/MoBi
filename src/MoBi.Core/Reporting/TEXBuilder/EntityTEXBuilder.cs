using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class EntityTEXBuilder : OSPSuiteTeXBuilder<IEntity>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public EntityTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IEntity entity, OSPSuiteTracker tracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.NAME, entity.Name));

         listToReport.AddRange(this.ReportDescription(entity, tracker));

         if (entity.Tags.Any())
            listToReport.Add(entity.Tags);

         _builderRepository.Report(listToReport, tracker);
      }
   }
}