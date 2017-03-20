using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class ObserverBuilderTEXBuilder : OSPSuiteTeXBuilder<IObserverBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public ObserverBuilderTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IObserverBuilder observerBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(observerBuilder, buildTracker));

         var amountObserver = observerBuilder as AmountObserverBuilder;
         var containerObserver = observerBuilder as ContainerObserverBuilder;
         if (amountObserver != null)
            listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, AppConstants.Captions.MoleculeObserver));
         if (containerObserver != null)
            listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.TYPE, AppConstants.Captions.ContainerObserver));

         listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.DIMENSION, observerBuilder.Dimension));

         if (observerBuilder.ForAll)
         {
            listToReport.Add(String.Format(Constants.PROPERTY_PROMPT_FORMAT, Constants.CALCULATED_FOR_ALL_MOLECULES,
                                           observerBuilder.ForAll));
         }
         else
         {
            listToReport.Add(new Paragraph(Constants.CALCULATED_FOR_FOLLOWING_MOLECULES));
            listToReport.Add(observerBuilder.MoleculeNames().ToArray());
         }

         listToReport.Add(new Paragraph(Constants.IN_CONTAINERS_WITH));
         listToReport.Add(observerBuilder.ContainerCriteria);

         listToReport.Add(new Paragraph(Constants.MONITOR));
         listToReport.Add(observerBuilder.Formula);

         _builderRepository.Report(listToReport, buildTracker);
      }

   }
}
