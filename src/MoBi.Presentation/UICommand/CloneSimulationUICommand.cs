using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.UICommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoBi.Presentation.UICommand
{
   internal class CloneSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IInteractionTasksForSimulation _editTasksForSimulation;
      private readonly IEditTasksForSimulation _simulationTasks;
      private readonly INotificationPresenter _notificationPresenter;
      private readonly IMoBiContext _context;

      public CloneSimulationUICommand(IEditTasksForSimulation simulationTasks, IInteractionTasksForSimulation editTasksForSimulation, INotificationPresenter notificationPresenter, IMoBiContext context)
      {
         _editTasksForSimulation = editTasksForSimulation;
         _simulationTasks = simulationTasks;
         _notificationPresenter = notificationPresenter;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _notificationPresenter.ClearNotifications(MessageOrigin.Simulation);
         var newSimulation = _simulationTasks.Clone(Subject);
         _context.AddToHistory(_editTasksForSimulation.AddToProject(newSimulation));
      }
   }
}
