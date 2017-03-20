using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX.Converter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class TransportBuilderTEXBuilder : OSPSuiteTeXBuilder<ITransportBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public TransportBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ITransportBuilder transportBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new Paragraph(transportBuilder.Name));
         listToReport.AddRange(this.ReportDescription(transportBuilder, buildTracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, transportBuilder.TransportType.ToString("F")));

         listToReport.Add(new SubParagraph(Constants.PLACE_OF_TRANSPORT));
         listToReport.Add(Constants.PLACE_OF_TRANSPORT_PHRASE);
         listToReport.Add(new LineBreak());
         listToReport.Add(new Par());
         var leftSide = new Text("{0}{1}", (new Text(Constants.SOURCE_CRITERIA) {FontStyle = Text.FontStyles.bold}),
                                 (new Text(_builderRepository.ChunkFor(transportBuilder.SourceCriteria)) {Converter = NoConverter.Instance}));
         var rightSide = new Text("{0}{1}", (new Text(Constants.TARGET_CRITERIA) {FontStyle = Text.FontStyles.bold}),
                                 (new Text(_builderRepository.ChunkFor(transportBuilder.TargetCriteria)) {Converter = NoConverter.Instance}));
         listToReport.Add(new SideBySide(leftSide, rightSide));

         listToReport.Add(new Paragraph(Constants.KINETIC));
         listToReport.Add(transportBuilder.Formula);

         if (transportBuilder.Parameters.Any())
         {
            listToReport.Add(new Paragraph(Constants.PARAMETERS));
            listToReport.Add(transportBuilder.Parameters.OrderBy(x => x.Name));
         }

         _builderRepository.Report(listToReport, buildTracker);
      }

   }
}
