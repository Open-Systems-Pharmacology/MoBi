using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class ApplicationMoleculeBuilderTEXBuilder : OSPSuiteTeXBuilder<ApplicationMoleculeBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ApplicationMoleculeBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(ApplicationMoleculeBuilder applicationMoleculeBuilder, OSPSuiteTracker tracker)
      {
         var listToReport = new List<object>();

         listToReport.Add(new Paragraph(applicationMoleculeBuilder.Name));
         listToReport.AddRange(this.ReportDescription(applicationMoleculeBuilder, tracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.DIMENSION, applicationMoleculeBuilder.Dimension));

         listToReport.Add(new SubParagraph(Constants.START_FORMULA));
         listToReport.Add(applicationMoleculeBuilder.Formula);


         _builderRepository.Report(listToReport, tracker);
      }
   }
}