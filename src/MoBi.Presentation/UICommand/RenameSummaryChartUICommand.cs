using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Services;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.UICommand
{
   public class RenameSummaryChartUICommand : RenameObjectBaseUICommand<CurveChart>
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public RenameSummaryChartUICommand(IOSPSuiteExecutionContext context, IEventPublisher eventPublisher, IApplicationController applicationController, IMoBiProjectRetriever projectRetriever)
         : base(context, eventPublisher, applicationController)
      {
         _projectRetriever = projectRetriever;
      }

      protected override IEnumerable<string> ForbiddenNamesFor(CurveChart chart)
      {
         return _projectRetriever.Current.Charts.Select(x => x.Name).ToList();
      }
   }
}
