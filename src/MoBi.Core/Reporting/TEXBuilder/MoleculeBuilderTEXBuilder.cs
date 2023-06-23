using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder 
{
   class MoleculeBuilderTEXBuilder : OSPSuiteTeXBuilder<MoleculeBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public MoleculeBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(MoleculeBuilder moleculeBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(moleculeBuilder, buildTracker));
        
         
         listToReport.Add(string.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MOLECULE_TYPE, moleculeBuilder.QuantityType.ToString()));
         listToReport.Add(string.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.IS_STATIONARY, (!moleculeBuilder.IsFloating).ToString()));

         if (!string.IsNullOrEmpty(moleculeBuilder.DefaultStartFormula.Name))
         {
            listToReport.Add(new Paragraph(Constants.INITIAL_CONDITIONS));
            listToReport.Add(moleculeBuilder.DefaultStartFormula);
         }

         if (moleculeBuilder.TransporterMoleculeContainerCollection.Any())
         {
            foreach (var transporter in moleculeBuilder.TransporterMoleculeContainerCollection)
            {
               listToReport.Add(new Paragraph(transporter.Name));

               listToReport.Add(new SubParagraph(Constants.ACTIVE_TRANSPORTS));
               listToReport.Add(transporter.ActiveTransportRealizations);  
               
               if (transporter.Parameters.Any())
               {
                  listToReport.Add(new SubParagraph(Constants.PARAMETERS));
                  listToReport.Add(transporter.Parameters.OrderBy(o => o.Name));
               }
            }
         }
         if (moleculeBuilder.UsedCalculationMethods.Any())
         {
            listToReport.Add(new Paragraph(Constants.CALCULATION_METHODS));
            listToReport.Add(moleculeBuilder.UsedCalculationMethods);
         }

         if (moleculeBuilder.Parameters.Any())
         {
            listToReport.Add(new Paragraph(Constants.PARAMETERS));
            listToReport.Add(moleculeBuilder.Parameters.OrderBy(o => o.Name));
         }

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}
