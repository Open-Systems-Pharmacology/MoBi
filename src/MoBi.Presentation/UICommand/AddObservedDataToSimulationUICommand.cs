using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddObservedDataToSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IObservedDataTask _observedDataTask;
      private IReadOnlyList<DataRepository> _observedData;

      public AddObservedDataToSimulationUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.AddObservedDataToAnalysable(_observedData, Subject, showData: true);
      }

      public AddObservedDataToSimulationUICommand For(DataRepository observedData) => For(new[] {observedData});

      public AddObservedDataToSimulationUICommand For(IReadOnlyList<DataRepository> observedData)
      {
         _observedData = observedData;
         return this;
      }
   }
}
