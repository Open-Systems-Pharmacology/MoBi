using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.UICommand
{
   internal class AddParameterValuesUICommand : AbstractStartValueSubjectRetrieverUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      private readonly IParameterValuesPresenter _presenter;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiHistoryManager _moBiHistoryManager;

      public AddParameterValuesUICommand(
         IParameterValuesTask parameterValueTask,
         IActiveSubjectRetriever activeSubjectRetriever,
         IMoBiApplicationController applicationController, IMoBiHistoryManager moBiHistoryManager,
         IParameterValuesPresenter presenter)
         : base(parameterValueTask, activeSubjectRetriever)
      {
         _presenter = presenter;
         _applicationController = applicationController;
         _moBiHistoryManager = moBiHistoryManager;
      }

      protected override void PerformExecute()
      {
         var presenter = _applicationController.Open<IEditParameterValuesPresenter, ParameterValuesBuildingBlock>(Subject, _moBiHistoryManager);
         presenter.AddNewParameterValue();
      }
   }
}