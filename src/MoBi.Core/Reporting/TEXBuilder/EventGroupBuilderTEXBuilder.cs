using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class EventGroupBuilderTEXBuilder : OSPSuiteTeXBuilder<EventGroupBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public EventGroupBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(EventGroupBuilder eventGroupBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new Paragraph(Constants.EVENT_GROUP_BUILDER));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.NAME, eventGroupBuilder.Name));
         listToReport.AddRange(this.ReportDescription(eventGroupBuilder, buildTracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MODE, eventGroupBuilder.Mode));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.EVENT_GROUP_TYPE, eventGroupBuilder.EventGroupType));

         var applicationBuilder = eventGroupBuilder as ApplicationBuilder;
         if (applicationBuilder != null)
         {
            listToReport.Add(new Paragraph(Constants.APPLICATION_BUILDER));
            listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, Constants.APPLICATION));
            listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MOLECULE_NAME, applicationBuilder.MoleculeName));

            if (applicationBuilder.Molecules.Any())
            {
               listToReport.Add(new Paragraph(Constants.APPLICATION_MOLECULE_BUILDER));
               listToReport.AddRange(applicationBuilder.Molecules);
            }

            listToReport.Add(Constants.SOURCE_CRITERIA);
            listToReport.Add(new LineBreak());
            listToReport.Add(applicationBuilder.SourceCriteria);

            listToReport.AddRange(applicationBuilder.GetChildren<TransportBuilder>());

            listToReport.AddRange(applicationBuilder.GetChildren<IContainer>());
         }

         listToReport.AddRange(eventGroupBuilder.GetChildren<EventGroupBuilder>());

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}
