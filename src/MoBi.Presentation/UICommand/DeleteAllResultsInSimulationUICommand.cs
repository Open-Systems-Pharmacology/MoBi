using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class DeleteAllResultsInSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly IObservedDataTask _observedDataTask;

      public DeleteAllResultsInSimulationUICommand(IActiveSubjectRetriever activeSubjectRetriever, IObservedDataTask observedDataTask) : base(activeSubjectRetriever)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.DeleteAllResultsFrom(Subject);
      }
   }
}