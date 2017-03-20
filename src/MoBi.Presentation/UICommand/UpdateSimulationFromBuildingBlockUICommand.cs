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

      private readonly ISimulationUpdateTask _simualtionUpdateTask;
      private readonly IMoBiContext _context;
      private readonly INotificationPresenter _notificationPresenter;

      public UpdateSimulationFromBuildingBlockUICommand(IMoBiContext context, ISimulationUpdateTask simualtionUpdateTask, INotificationPresenter notificationPresenter)
      {
         _context = context;
         _simualtionUpdateTask = simualtionUpdateTask;
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
         _context.AddToHistory(_simualtionUpdateTask.UpdateSimulationFrom(_simulationToUpdate, _changedBuildingBlock));
      }
   }
}