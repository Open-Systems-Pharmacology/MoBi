using System.Collections.Generic;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveMultipleModulesUICommand : ObjectUICommand<IReadOnlyList<Module>>
   {
      private readonly IObservedDataTask _observedDataTask;

      public RemoveMultipleModulesUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.RemoveMultipleModules(Subject);
      }
   }
}