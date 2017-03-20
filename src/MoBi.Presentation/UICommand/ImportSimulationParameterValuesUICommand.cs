using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ImportSimulationParameterValuesUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IMoBiApplicationController _applicationController;

      public ImportSimulationParameterValuesUICommand(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var presenter = _applicationController.Start<IImportQuantitiesForSimulationPresenter>())
         {
            presenter.ImportQuantitiesForSimulation(Subject);
         }
      }
   }
}
