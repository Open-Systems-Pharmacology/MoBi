using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class AddInitialConditionsUICommand : AbstractStartValueSubjectRetrieverUICommand<InitialConditionsBuildingBlock, InitialCondition>
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiHistoryManager _moBiHistoryManager;

      public AddInitialConditionsUICommand(
         IInitialConditionsTask<InitialConditionsBuildingBlock> initialConditionsTask,
         IActiveSubjectRetriever activeSubjectRetriever,
         IMoBiApplicationController applicationController, IMoBiHistoryManager moBiHistoryManager)
         : base(initialConditionsTask, activeSubjectRetriever)
      {
         _applicationController = applicationController;
         _moBiHistoryManager = moBiHistoryManager;
      }

      protected override void PerformExecute()
      {
         var presenter = _applicationController.Open<IEditInitialConditionsPresenter, InitialConditionsBuildingBlock>(Subject, _moBiHistoryManager);
         presenter.AddNewEmptyInitialCondition();
      }
   }
}