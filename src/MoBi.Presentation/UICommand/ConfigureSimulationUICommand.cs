using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ConfigureSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly ISimulationUpdateTask _simulationUpdateTask;
      private readonly INotificationPresenter _notificationPresenter;
      private readonly IMoBiContext _context;

      public ConfigureSimulationUICommand(
         ISimulationUpdateTask simulationUpdateTask,
         INotificationPresenter notificationPresenter,
         IMoBiContext context,
         IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _simulationUpdateTask = simulationUpdateTask;
         _notificationPresenter = notificationPresenter;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _notificationPresenter.ClearNotifications(MessageOrigin.Simulation);
         _context.AddToHistory(_simulationUpdateTask.ConfigureSimulation(Subject));
      }
   }
}