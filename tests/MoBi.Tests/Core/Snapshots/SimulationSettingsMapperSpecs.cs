using System.Threading.Tasks;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OutputSchema = OSPSuite.Core.Domain.OutputSchema;
using OutputSchemaMapper = MoBi.Core.Snapshots.Mappers.OutputSchemaMapper;
using SolverSettings = OSPSuite.Core.Domain.SolverSettings;
using SolverSettingsMapper = MoBi.Core.Snapshots.Mappers.SolverSettingsMapper;

namespace MoBi.Core.Snapshots
{
   public class concern_for_SimulationSettingsMapper : ContextSpecificationAsync<SimulationSettingsMapper>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected OutputSchemaMapper _outputSchemaMapper;
      protected SolverSettingsMapper _solverSettingsMapper;

      protected override async Task Context()
      {
         await base.Context();
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _outputSchemaMapper = A.Fake<OutputSchemaMapper>();
         _solverSettingsMapper = A.Fake<SolverSettingsMapper>();

         sut = new SimulationSettingsMapper(_outputSchemaMapper, _solverSettingsMapper, _objectBaseFactory);
      }
   }

   public class When_mapping_a_snapshot_to_simulation_settings : concern_for_SimulationSettingsMapper
   {
      private SimulationSettings _simulationSettings;
      private OSPSuite.Core.Domain.Builder.SimulationSettings _result;
      private SimulationContext _context;

      protected override async Task Context()
      {
         await base.Context();
         _context = new SimulationContext(false, new SnapshotContext(new MoBiProject(), SnapshotVersions.Current));
         _simulationSettings = new SimulationSettings
         {
            Solver = new OSPSuite.Core.Snapshots.SolverSettings(),
            OutputSchema = new OSPSuite.Core.Snapshots.OutputSchema(),
            RandomSeed = 4.4
         };
      }

      protected override Task Because()
      {
         _result = sut.MapToModel(_simulationSettings, _context).Result;
         return Task.CompletedTask;
      }

      [Observation]
      public void the_output_schema_mapper_is_used_to_map_the_schema()
      {
         A.CallTo(() => _outputSchemaMapper.MapToModel(_simulationSettings.OutputSchema, _context)).MustHaveHappened();
      }

      [Observation]
      public void the_solver_settings_mapper_is_used_to_map_the_solver()
      {
         A.CallTo(() => _solverSettingsMapper.MapToModel(_simulationSettings.Solver, _context)).MustHaveHappened();
      }

      [Observation]
      public void random_seed_is_mapped()
      {
         _result.RandomSeed.ShouldBeEqualTo(_simulationSettings.RandomSeed);
      }
   }

   public class When_mapping_a_simulation_settings_to_snapshot : concern_for_SimulationSettingsMapper
   {
      private OSPSuite.Core.Domain.Builder.SimulationSettings _simulationSettings;
      private SimulationSettings _result;

      protected override async Task Context()
      {
         await base.Context();
         _simulationSettings = new OSPSuite.Core.Domain.Builder.SimulationSettings();
         _simulationSettings.Solver = new SolverSettings();
         _simulationSettings.OutputSchema = new OutputSchema();
         _simulationSettings.RandomSeed = 4.4;
      }

      protected override Task Because()
      {
         _result = sut.MapToSnapshot(_simulationSettings).Result;
         return Task.CompletedTask;
      }

      [Observation]
      public void the_output_schema_mapper_is_used_to_map_the_schema()
      {
         A.CallTo(() => _outputSchemaMapper.MapToSnapshot(_simulationSettings.OutputSchema)).MustHaveHappened();
      }

      [Observation]
      public void the_solver_settings_mapper_is_used_to_map_the_solver()
      {
         A.CallTo(() => _solverSettingsMapper.MapToSnapshot(_simulationSettings.Solver)).MustHaveHappened();
      }

      [Observation]
      public void random_seed_is_mapped()
      {
         _result.RandomSeed.ShouldBeEqualTo(_simulationSettings.RandomSeed);
      }
   }
}