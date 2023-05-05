using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class AddParameterStartValuesUICommand : AbstractStartValueSubjectRetrieverUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiHistoryManager _moBiHistoryManager;

      public AddParameterStartValuesUICommand(
         IParameterStartValuesTask startValueTasks,
         IActiveSubjectRetriever activeSubjectRetriever,
         IMoBiApplicationController applicationController,IMoBiHistoryManager moBiHistoryManager)
         : base(startValueTasks, activeSubjectRetriever)
      {
         _applicationController = applicationController;
         _moBiHistoryManager = moBiHistoryManager;
      }

      protected override void PerformExecute()
      {
         var presenter = _applicationController.Open<IEditParameterStartValuesPresenter, ParameterValuesBuildingBlock>(Subject, _moBiHistoryManager);
         presenter.AddNewEmptyStartValue();
      }
   }
}