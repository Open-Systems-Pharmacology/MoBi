using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;

namespace MoBi.Core.Commands
{
   public class concern_for_AddOutputIntervalCommand : ContextSpecification<AddOutputIntervalCommand>
   {
      protected SimulationSettings _simulationSettings;
      protected OutputInterval _interval;
      protected OutputSchema _schema;
      protected IMoBiContext _context;
      protected IBuildingBlockVersionUpdater _buildingBlockVersionUpdater;
      protected string _intervalId;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _intervalId = "intervalId";
         _buildingBlockVersionUpdater = A.Fake<IBuildingBlockVersionUpdater>();
         _schema = new OutputSchema();
         _interval = new OutputInterval().WithId(_intervalId);
         _simulationSettings = new SimulationSettings
         {
            OutputSchema = _schema,
            Id = "simulationSettingsId"
         };
         sut = new AddOutputIntervalCommand(_schema, _interval, _simulationSettings);

         A.CallTo(() => _context.Get<SimulationSettings>(_simulationSettings.Id)).Returns(_simulationSettings);
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(_buildingBlockVersionUpdater);
      }
   }

   public class When_reverting_the_add_output_interval_command : concern_for_AddOutputIntervalCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<OutputInterval>(_intervalId)).Returns(_interval);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_interval_must_be_removed_from_the_schema()
      {
         _schema.Intervals.ShouldNotContain(_interval);
      }
   }

   public class When_adding_an_output_interval_to_a_schema : concern_for_AddOutputIntervalCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_interval_must_be_registered()
      {
         A.CallTo(() => _context.Register(_interval)).MustHaveHappened();
      }

      [Observation]
      public void the_schema_must_contain_the_new_interval()
      {
         _schema.Intervals.ShouldContain(_interval);
      }

      [Observation]
      public void an_event_must_be_raised_for_the_schema()
      {
         A.CallTo(() => _context.PublishEvent(A<OutputSchemaChangedEvent>.That.Matches(x => x.OutputSchema == _schema))).MustHaveHappened();
      }

      [Observation]
      public void the_building_block_version_updater_should_update_the_building_block_version()
      {
         A.CallTo(() => _buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_simulationSettings, true)).MustHaveHappened();
      }
   }
}
