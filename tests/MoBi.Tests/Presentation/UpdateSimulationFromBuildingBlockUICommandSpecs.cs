using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Presentation
{
   public abstract class concern_for_UpdateSimulationFromBuildingBlockUICommandSpecs : ContextSpecification<UpdateSimulationFromBuildingBlockUICommand>
   {
      protected IMoBiContext _context;
      protected ISimulationUpdateTask _updateTask;
      protected INotificationPresenter _notificationPresenter;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _updateTask = A.Fake<ISimulationUpdateTask>();
         _notificationPresenter = A.Fake<INotificationPresenter>();
         sut = new UpdateSimulationFromBuildingBlockUICommand(_context,_updateTask,_notificationPresenter);
      }
   }

   class Whem_execution_the_update_simulation_ui_command: concern_for_UpdateSimulationFromBuildingBlockUICommandSpecs
   {
      private IMoBiSimulation _simulation;
      private IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut.Initialize(_buildingBlock, _simulation);
      }


      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_clear_all_notification_messages_originated_from_simulation()
      {
         A.CallTo(() => _notificationPresenter.ClearNotifications(MessageOrigin.Simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_simulation()
      {
         A.CallTo(() => _updateTask.UpdateSimulationFrom(_simulation,_buildingBlock)).MustHaveHappened();  
      }
   }
}	