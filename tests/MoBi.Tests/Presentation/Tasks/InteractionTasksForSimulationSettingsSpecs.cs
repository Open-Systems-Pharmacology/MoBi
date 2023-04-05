using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Events;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_InteractionTasksForSimulationSettings : ContextSpecification<InteractionTasksForSimulationSettings>
   {
      private ISimulationSettingsFactory _simulationSettingsFactory;
      protected IEditTasksForBuildingBlock<SimulationSettings> _editTask;
      protected IInteractionTaskContext _interactionTaskContext;

      protected override void Context()
      {
         _simulationSettingsFactory = A.Fake<ISimulationSettingsFactory>();
         _editTask = A.Fake<IEditTasksForBuildingBlock<SimulationSettings>>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();

         sut = new InteractionTasksForSimulationSettings(_interactionTaskContext, _editTask, _simulationSettingsFactory);
      }
   }

   public class When_updating_default_simulation_settings_and_the_import_contains_one_or_more_items : concern_for_InteractionTasksForSimulationSettings
   {
      private IMoBiCommand _command;
      private SimulationSettings _simulationSettings2;
      private SimulationSettings _simulationSettings1;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.AskForFileToOpen(A<string>._, A<string>._, A<string>._)).Returns("A file name");
         _simulationSettings2 = new SimulationSettings().WithName("simset2");
         _simulationSettings1 = new SimulationSettings().WithName("simset1");
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadItems<SimulationSettings>(A<string>._)).Returns(new List<SimulationSettings>
         {
            _simulationSettings1,
            _simulationSettings2
         });
      }

      protected override void Because()
      {
         _command = sut.UpdateDefaultSimulationSettingsInProject();
      }

      [Observation]
      public void must_have_published_an_event_with_new_default_simulation_settings()
      {
         A.CallTo(() => _interactionTaskContext.Context.PublishEvent(A<DefaultSimulationSettingsUpdatedEvent>.That.Matches(x => x.NewSimulationSettings.Equals(_simulationSettings1)))).MustHaveHappened();
      }

      [Observation]
      public void the_command_used_should_be_empty()
      {
         _command.ShouldBeAnInstanceOf<UpdateDefaultSimulationSettingsInProjectCommand>();
      }
   }

   public class When_updating_default_simulation_settings_but_the_import_contains_no_items : concern_for_InteractionTasksForSimulationSettings
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.AskForFileToOpen(A<string>._, A<string>._, A<string>._)).Returns("A file name");
         A.CallTo(() => _interactionTaskContext.InteractionTask.LoadItems<SimulationSettings>(A<string>._)).Returns(new List<SimulationSettings>());
      }

      protected override void Because()
      {
         _command = sut.UpdateDefaultSimulationSettingsInProject();
      }

      [Observation]
      public void the_command_used_should_be_empty()
      {
         _command.ShouldBeAnInstanceOf<MoBiEmptyCommand>();
      }
   }

   public class When_updating_default_simulation_settings_but_canceling_during_add : concern_for_InteractionTasksForSimulationSettings
   {
      private IMoBiCommand _command;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _interactionTaskContext.InteractionTask.AskForFileToOpen(A<string>._, A<string>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         _command = sut.UpdateDefaultSimulationSettingsInProject();
      }

      [Observation]
      public void the_command_used_should_be_empty()
      {
         _command.ShouldBeAnInstanceOf<MoBiEmptyCommand>();
      }
   }
}