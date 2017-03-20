using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class AddMoleculeStartValuesUICommand : AbstractStartValueSubjectRetrieverUICommand<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiHistoryManager _moBiHistoryManager;

      public AddMoleculeStartValuesUICommand(
         IMoleculeStartValuesTask startValueTasks,
         IActiveSubjectRetriever activeSubjectRetriever,
         IMoBiApplicationController applicationController, IMoBiHistoryManager moBiHistoryManager)
         : base(startValueTasks, activeSubjectRetriever)
      {
         _applicationController = applicationController;
         _moBiHistoryManager = moBiHistoryManager;
      }

      protected override void PerformExecute()
      {
         var presenter = _applicationController.Open<IEditMoleculeStartValuesPresenter,IMoleculeStartValuesBuildingBlock>(Subject, _moBiHistoryManager);
         presenter.AddNewEmptyStartValue();
      }
   }
}