using System.Threading.Tasks;
using FakeItEasy;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots
{
   public class concern_for_SimulationMapper : ContextSpecificationAsync<SimulationMapper>
   {
      protected SimulationConfigurationMapper _simulationConfigurationMapper;
      protected ISimulationFactory _simulationFactory;

      protected override Task Context()
      {
         _simulationConfigurationMapper = A.Fake<SimulationConfigurationMapper>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         sut = new SimulationMapper(
            _simulationConfigurationMapper,
            A.Fake<OutputMappingMapper>(),
            A.Fake<MoBiSimulationTimeProfileChartMapper>(),
            _simulationFactory,
            A.Fake<OutputSelectionsMapper>(),
            A.Fake<SimulationPredictedVsObservedChartMapper>(),
            A.Fake<SimulationResidualVsTimeChartMapper>(),
            A.Fake<IOSPSuiteLogger>(),
            A.Fake<ParameterMapper>(),
            A.Fake<IMoBiDimensionFactory>(),
            A.Fake<IQuantityValueInSimulationChangeTracker>(),
            A.Fake<ValueOriginMapper>());
         return Task.CompletedTask;
      }
   }

   public abstract class When_mapping_a_simulation_snapshot : concern_for_SimulationMapper
   {
      protected Simulation _snapshot;
      protected SimulationContext _context;
      protected OSPSuite.Core.Domain.Builder.SimulationConfiguration _configuration;
      protected MoBiSimulation _createdSimulation;
      protected bool? _showProgressDuringConstruction;
      protected MoBiSimulation _result;

      protected override async Task Context()
      {
         await base.Context();
         _snapshot = new Simulation {Name = "S1"};
         _context = new SimulationContext(false, new SnapshotContext(new MoBiProject(), SnapshotVersions.Current));

         _configuration = new OSPSuite.Core.Domain.Builder.SimulationConfiguration();
         A.CallTo(() => _simulationConfigurationMapper.MapToModel(_snapshot.Configuration, _context)).Returns(_configuration);

         //the factory hands the simulation a clone of the configuration: the clone carries the flag as set during construction
         _createdSimulation = new MoBiSimulation();
         A.CallTo(() => _simulationFactory.CreateSimulationAndValidate(_configuration, _snapshot.Name)).ReturnsLazily(() =>
         {
            _showProgressDuringConstruction = _configuration.ShowProgress;
            _createdSimulation.Configuration = new OSPSuite.Core.Domain.Builder.SimulationConfiguration
            {
               ShowProgress = _configuration.ShowProgress,
               SimulationSettings = new OSPSuite.Core.Domain.Builder.SimulationSettings()
            };
            return new SimulationAndValidationResult(_createdSimulation, new ValidationResult());
         });
      }

      protected override async Task Because()
      {
         _result = await sut.MapToModel(_snapshot, _context);
      }
   }

   public class When_mapping_a_simulation_snapshot_to_model : When_mapping_a_simulation_snapshot
   {
      [Observation]
      public void should_suppress_the_core_progress_while_the_model_is_constructed()
      {
         _showProgressDuringConstruction.ShouldBeEqualTo(false);
      }

      [Observation]
      public void should_restore_the_progress_flag_on_the_configuration_kept_by_the_simulation()
      {
         _result.Configuration.ShowProgress.ShouldBeTrue();
      }
   }

   public class When_mapping_a_simulation_snapshot_whose_configuration_disables_progress : When_mapping_a_simulation_snapshot
   {
      protected override async Task Context()
      {
         await base.Context();
         //the flag is not mapped from the snapshot today: starting from false pins the restore to the
         //captured value rather than to the default
         _configuration.ShowProgress = false;
      }

      [Observation]
      public void should_suppress_the_core_progress_while_the_model_is_constructed()
      {
         _showProgressDuringConstruction.ShouldBeEqualTo(false);
      }

      [Observation]
      public void should_keep_the_flag_disabled_on_the_configuration_kept_by_the_simulation()
      {
         _result.Configuration.ShowProgress.ShouldBeFalse();
      }
   }
}
