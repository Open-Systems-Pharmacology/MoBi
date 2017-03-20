using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;


namespace MoBi.Core.Reporting.TEXBuilder
{
   class EventAssignmentBuilderTEXBuilder : OSPSuiteTeXBuilder<IEventAssignmentBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public EventAssignmentBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IEventAssignmentBuilder eventAssignmentBuilder, OSPSuiteTracker tracker)
      {
         var listToReport = new List<object>();

         listToReport.Add(new SubParagraph(eventAssignmentBuilder.Name));
         listToReport.AddRange(this.ReportDescription(eventAssignmentBuilder, tracker));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.DIMENSION, eventAssignmentBuilder.Dimension));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.ASSIGNMENT, eventAssignmentBuilder.ObjectPath.PathAsString));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.USE_AS_VALUE, eventAssignmentBuilder.UseAsValue));
         listToReport.Add(eventAssignmentBuilder.Formula);

         _builderRepository.Report(listToReport, tracker);
      }
   }
}
