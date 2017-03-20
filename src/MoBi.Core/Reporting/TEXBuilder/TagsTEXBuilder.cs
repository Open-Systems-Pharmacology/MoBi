using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.TeXReporting.TeX.Converter;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class TagsTeXBuilder : OSPSuiteTeXBuilder<Tags>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public TagsTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(Tags tags, OSPSuiteTracker tracker)
      {
         var listToReport = new List<object>();

         listToReport.Add(new SubParagraph(Constants.TAGS));
         listToReport.Add(listFor(tags));

         _builderRepository.Report(listToReport, tracker);
      }

      private List listFor(Tags tags)
      {
         var items = tags.Select(tag => new Text(tag.Value) {Converter = DefaultConverter.Instance}).ToList();
         return new List(items);
      }
   }
}