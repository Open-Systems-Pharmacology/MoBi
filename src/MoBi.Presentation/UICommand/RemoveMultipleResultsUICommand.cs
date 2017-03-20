using System.Collections.Generic;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveMultipleResultsUICommand : ObjectUICommand<IReadOnlyList<DataRepository>>
   {
      private readonly IObservedDataTask _observedDataTask;

      public RemoveMultipleResultsUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.RemoveResultsFromSimulations(Subject);
      }
   }
}