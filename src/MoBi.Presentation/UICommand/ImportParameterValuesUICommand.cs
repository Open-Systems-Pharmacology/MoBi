using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ImportParameterValuesUICommand : ObjectUICommand<IBuildingBlock>
   {
      private readonly IMoBiApplicationController _applicationController;

      public ImportParameterValuesUICommand(IMoBiApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected override void PerformExecute()
      {
         using (var presenter = _applicationController.Start<IImportParameterValuesPresenter>())
         {
            presenter.ImportStartValuesForBuildingBlock(Subject);
         }
      }
   }
}