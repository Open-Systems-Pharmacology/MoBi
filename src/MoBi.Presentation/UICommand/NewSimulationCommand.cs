using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks.Interaction;

namespace MoBi.Presentation.UICommand
{
   public class NewSimulationCommand : IUICommand
   {
      private readonly IInteractionTasksForSimulation _editTasksForSimulation;
      private readonly INotificationPresenter _notificationPresenter;
      private readonly IMoBiContext _context ;

      public NewSimulationCommand(IInteractionTasksForSimulation editTasksForSimulation, INotificationPresenter notificationPresenter, IMoBiContext context)
      {
         _editTasksForSimulation = editTasksForSimulation;
         _notificationPresenter = notificationPresenter;
         _context = context;
      }

      public void Execute()
      {
         _notificationPresenter.ClearNotifications(MessageOrigin.Simulation);
         _context.AddToHistory(_editTasksForSimulation.CreateSimulation());
      }
   }
}