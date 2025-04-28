using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class AddParameterValuesUICommand : AbstractStartValueSubjectRetrieverUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiHistoryManager _moBiHistoryManager;

      public AddParameterValuesUICommand(
         IParameterValuesTask parameterValueTask,
         IActiveSubjectRetriever activeSubjectRetriever,
         IMoBiApplicationController applicationController, IMoBiHistoryManager moBiHistoryManager)
         : base(parameterValueTask, activeSubjectRetriever)
      {
         _applicationController = applicationController;
         _moBiHistoryManager = moBiHistoryManager;
      }

      protected override void PerformExecute()
      {
         var presenter = _applicationController.Open<IEditParameterValuesPresenter, ParameterValuesBuildingBlock>(Subject, _moBiHistoryManager);
         presenter.AddNewParameterValues();
      }
   }
}