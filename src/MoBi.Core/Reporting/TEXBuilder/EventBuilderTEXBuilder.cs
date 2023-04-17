using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class EventBuilderTEXBuilder : OSPSuiteTeXBuilder<EventBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public EventBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(EventBuilder eventBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new Paragraph(eventBuilder.Name));
         listToReport.AddRange(this.ReportDescription(eventBuilder, buildTracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, eventBuilder.ContainerType));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MODE, eventBuilder.Mode));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.ONE_TIME, eventBuilder.OneTime));

         listToReport.Add(new SubParagraph(Constants.CONDITION));
         listToReport.Add(eventBuilder.Formula);

         if (eventBuilder.Assignments.Any())
         {
            listToReport.Add(new SubParagraph(Constants.EVENT_ASSIGNMENTS));
            listToReport.AddRange(eventBuilder.Assignments.OrderBy(o => o.Name));
         }
         if (eventBuilder.Parameters.Any())
         {
            listToReport.Add(new SubParagraph(Constants.PARAMETERS));
            listToReport.Add(eventBuilder.Parameters.OrderBy(o => o.Name));
         }

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}
