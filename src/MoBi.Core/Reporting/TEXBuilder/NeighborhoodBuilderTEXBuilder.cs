using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class NeighborhoodBuilderTEXBuilder : OSPSuiteTeXBuilder<NeighborhoodBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public NeighborhoodBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(NeighborhoodBuilder neighborhood, OSPSuiteTracker tracker)
      {
         var listToReport = new List<object>();
         listToReport.Add(new SubSubSection(neighborhood.Name));
         listToReport.AddRange(this.ReportDescription(neighborhood, tracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, neighborhood.ContainerType));
         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MODE, neighborhood.Mode));

         listToReport.Add(new TextBox(Constants.NEIGHBORHOOD,
                                      new Text("{0}",
                                               new SideBySide(
                                                  new Text("{0}{1}", new Paragraph(Constants.FIRST_NEIGHBOR),
                                                           neighborhood.FirstNeighbor.Name),
                                                  new Text("{0}{1}", new Paragraph(Constants.SECOND_NEIGHBOR),
                                                           neighborhood.SecondNeighbor.Name)))));

         if (neighborhood.MoleculeProperties != null)
         {
            listToReport.Add((new Paragraph(Constants.MOLECULE_PROPERTIES)));
            listToReport.Add(neighborhood.MoleculeProperties);
         }

         if (neighborhood.Tags.Any())
            listToReport.Add(neighborhood.Tags);

         if (neighborhood.Parameters.Any())
            listToReport.Add(neighborhood.Parameters);

         _builderRepository.Report(listToReport, tracker);
      }
   }
}
