using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class ReconfigureSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      protected ISimulationUpdateTask _simulationUpdateTask;
      protected INotificationPresenter _notificationPresenter;
      protected IMoBiContext _context;

      protected ReconfigureSimulationUICommand(IActiveSubjectRetriever activeSubjectRetriever, ISimulationUpdateTask simulationUpdateTask, INotificationPresenter notificationPresenter, IMoBiContext context) : base(activeSubjectRetriever)
      {
         _simulationUpdateTask = simulationUpdateTask;
         _notificationPresenter = notificationPresenter;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _notificationPresenter.ClearNotifications(MessageOrigin.Simulation);
         _context.AddToHistory(PerformReconfigure());
      }

      protected abstract ICommand PerformReconfigure();
   }
}