using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Reflection;

namespace MoBi.Presentation.UICommand
{
   public class AddObservedDataToSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IObservedDataTask _observedDataTask;
      private WeakRef<DataRepository> _observedDataReference;

      public AddObservedDataToSimulationUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.AddObservedDataToAnalysable(new[] {_observedDataReference.Target}, Subject, showData: true);
      }

      public AddObservedDataToSimulationUICommand For(DataRepository observedData)
      {
         _observedDataReference = new WeakRef<DataRepository>(observedData);
         return this;
      }
   }
}
