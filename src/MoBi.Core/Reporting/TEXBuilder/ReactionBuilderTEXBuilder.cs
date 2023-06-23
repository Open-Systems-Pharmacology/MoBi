using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class ReactionBuilderTEXBuilder : OSPSuiteTeXBuilder<ReactionBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private readonly IStoichiometricStringCreator _stoichiometricStringCreator;

      public ReactionBuilderTEXBuilder(ITeXBuilderRepository builderRepository, IStoichiometricStringCreator stoichiometricStringCreator)
      {
         _builderRepository = builderRepository;
         _stoichiometricStringCreator = stoichiometricStringCreator;
      }

      public override void Build(ReactionBuilder reactionBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(reactionBuilder, buildTracker));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.MODE, reactionBuilder.Mode));

         listToReport.Add(new Paragraph(Constants.STOICHIOMETRY));
         listToReport.Add(_stoichiometricStringCreator.CreateFrom(reactionBuilder.Educts,
                                                                                reactionBuilder.Products));
         if (reactionBuilder.ModifierNames.Any())
         {
            listToReport.Add(new Paragraph(Constants.MODIFIERS));
            foreach (var modifier in reactionBuilder.ModifierNames)
               listToReport.Add(modifier);
         }

         listToReport.Add(new Paragraph(Constants.KINETIC));
         listToReport.Add(reactionBuilder.Formula);

         if (reactionBuilder.Parameters.Any())
         {
            listToReport.Add(new Paragraph(Constants.PARAMETERS));
            listToReport.Add(reactionBuilder.Parameters.OrderBy(x => x.Name));
         }

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}
