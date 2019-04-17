using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.UICommand
{
   public class UpdateSimulationFromBuildingBlockUICommand : IUICommand
   {
      private IBuildingBlock _changedBuildingBlock;
      private IMoBiSimulation _simulationToUpdate;

      private readonly ISimulationUpdateTask _simulationUpdateTask;
      private readonly IMoBiContext _context;
      private readonly INotificationPresenter _notificationPresenter;

      public UpdateSimulationFromBuildingBlockUICommand(IMoBiContext context, ISimulationUpdateTask simulationUpdateTask, INotificationPresenter notificationPresenter)
      {
         _context = context;
         _simulationUpdateTask = simulationUpdateTask;
         _notificationPresenter = notificationPresenter;
      }

      public UpdateSimulationFromBuildingBlockUICommand Initialize(IBuildingBlock changedBuildingBlock, IMoBiSimulation simulationToUpdate)
      {
         _changedBuildingBlock = changedBuildingBlock;
         _simulationToUpdate = simulationToUpdate;
         return this;
      }

      public void Execute()
      {
         _notificationPresenter.ClearNotifications(MessageOrigin.Simulation);
         _context.AddToHistory(_simulationUpdateTask.UpdateSimulationFrom(_simulationToUpdate, _changedBuildingBlock));
      }
   }
}