using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class ContainerTEXBuilder : OSPSuiteTeXBuilder<IContainer>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ContainerTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IContainer container, OSPSuiteTracker tracker)
      {
         var listToReport = new List<object>();
         switch (container.ContainerType)
         {
            case ContainerType.Organism:
            case ContainerType.Organ:
               listToReport.Add(new SubSubSection(container.Name));
               break;
            default:
               listToReport.Add(new Paragraph(container.Name));
               break;
         }
         listToReport.AddRange(this.ReportDescription(container, tracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, container.ContainerType));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MODE, container.Mode));

         if (container.Tags.Any())
            listToReport.Add(container.Tags);

         var parameters = container.GetChildren<IParameter>().ToList();
         if (parameters.Any())
            listToReport.Add(parameters);

         var containers = container.GetChildren<IContainer>().ToList();
         if (containers.Any())
            listToReport.AddRange(containers);

         _builderRepository.Report(listToReport, tracker);
      }
   }
}