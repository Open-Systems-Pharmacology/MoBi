using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class UpdateSimulationUICommand : ReconfigureSimulationUICommand
   {
      public UpdateSimulationUICommand(ISimulationUpdateTask simulationUpdateTask,
         INotificationPresenter notificationPresenter,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) :
         base(activeSubjectRetriever, simulationUpdateTask, notificationPresenter, context)
      {
      }

      protected override ICommand PerformReconfigure()
      {
         return _simulationUpdateTask.UpdateSimulation(Subject);
      }
   }
}