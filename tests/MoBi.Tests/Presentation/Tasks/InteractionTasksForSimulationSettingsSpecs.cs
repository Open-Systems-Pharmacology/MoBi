using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
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

   public class When_committing_simulation_solver_and_schema : concern_for_InteractionTasksForSimulationSettings
   {
      private SimulationSettings _simulationSettings;
      private IMoBiContext _context;
      private MoBiProject _currentProject;
      private SimulationSettings _clonedSettings;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _context = A.Fake<IMoBiContext>();

         _clonedSettings = new SimulationSettings
         {
            Solver = new SolverSettings(),
            OutputSchema = new OutputSchema()
         };
         _currentProject = new MoBiProject
         {
            SimulationSettings = new SimulationSettings
            {
               Solver = new SolverSettings(),
               OutputSchema = new OutputSchema()
            }
         };
         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
         A.CallTo(() => _context.CurrentProject).Returns(_currentProject);
         A.CallTo(() => _context.Clone(_simulationSettings)).Returns(_clonedSettings);
      }

      protected override void Because()
      {
         sut.UpdateDefaultSimulationSettingsInProject(_simulationSettings);
      }

      [Observation]
      public void the_simulation_settings_should_be_updated()
      {
         _context.CurrentProject.SimulationSettings.OutputSchema.ShouldBeEqualTo(_clonedSettings.OutputSchema);
         _context.CurrentProject.SimulationSettings.Solver.ShouldBeEqualTo(_clonedSettings.Solver);
      }
   }

   public class When_committing_simulation_output_selections : concern_for_InteractionTasksForSimulationSettings
   {
      private SimulationSettings _simulationSettings;
      private IMoBiContext _context;
      private MoBiProject _currentProject;
      private SimulationSettings _clonedSettings;

      protected override void Context()
      {
         base.Context();
         _simulationSettings = new SimulationSettings();
         _context = A.Fake<IMoBiContext>();
         _currentProject = new MoBiProject
         {
            SimulationSettings = new SimulationSettings()
         };

         _clonedSettings = new SimulationSettings
         {
            OutputSelections = new OutputSelections()
         };

         A.CallTo(() => _interactionTaskContext.Context).Returns(_context);
         A.CallTo(() => _context.CurrentProject).Returns(_currentProject);
         A.CallTo(() => _context.Clone(_simulationSettings)).Returns(_clonedSettings);
      }

      protected override void Because()
      {
         sut.UpdateDefaultOutputSelectionsInProject(_simulationSettings);
      }

      [Observation]
      public void the_simulation_settings_should_be_updated()
      {
         _context.CurrentProject.SimulationSettings.OutputSelections.ShouldBeEqualTo(_clonedSettings.OutputSelections);
      }
   }

   public class When_updating_default_simulation_settings_and_the_import_contains_one_or_more_items : concern_for_InteractionTasksForSimulationSettings
   {
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
         sut.LoadDefaultSimulationSettingsInProject();
      }

      [Observation]
      public void must_have_published_an_event_with_new_default_simulation_settings()
      {
         A.CallTo(() => _interactionTaskContext.Context.PublishEvent(A<DefaultSimulationSettingsUpdatedEvent>.That.Matches(x => x.NewSimulationSettings.Equals(_simulationSettings1)))).MustHaveHappened();
      }
   }
}