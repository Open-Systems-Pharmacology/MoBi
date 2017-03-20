using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   public class DeleteAllResultsInAllSimulationsUICommand : IUICommand
   {
      private readonly IObservedDataTask _observedDataTask;

      public DeleteAllResultsInAllSimulationsUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      public void Execute()
      {
         _observedDataTask.DeleteAllResultsFromAllSimulation();
      }
   }
}