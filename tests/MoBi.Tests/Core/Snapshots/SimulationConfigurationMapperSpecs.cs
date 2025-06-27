using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using SolverSettings = OSPSuite.Core.Domain.SolverSettings;

namespace MoBi.Core.Snapshots
{
   public class concern_for_SimulationConfigurationMapper : ContextSpecificationAsync<SimulationConfigurationMapper>
   {
      protected ISimulationConfigurationFactory _simulationConfigurationFactory;
      protected SimulationSettingsMapper _simulationSettingsMapper;
      protected ModuleConfigurationMapper _moduleConfigurationMapper;

      protected override async Task Context()
      {
         await base.Context();
         _simulationConfigurationFactory = A.Fake<ISimulationConfigurationFactory>();
         _simulationSettingsMapper = A.Fake<SimulationSettingsMapper>();
         _moduleConfigurationMapper = A.Fake<ModuleConfigurationMapper>();
         sut = new SimulationConfigurationMapper(_simulationConfigurationFactory, _simulationSettingsMapper, _moduleConfigurationMapper);
      }
   }

   public class When_mapping_to_snapshot : concern_for_SimulationConfigurationMapper
   {
      private OSPSuite.Core.Domain.Builder.SimulationConfiguration _simulationConfiguration;
      private SimulationConfiguration _result;

      protected override Task Context()
      {
         var settings = new OSPSuite.Core.Domain.Builder.SimulationSettings
         {
            Solver = new SolverSettings()
         };
         _simulationConfiguration = new OSPSuite.Core.Domain.Builder.SimulationConfiguration
         {
            SimulationSettings = settings,
            Individual = new IndividualBuildingBlock().WithName("individual")
         };

         _simulationConfiguration.AddExpressionProfile(new ExpressionProfileBuildingBlock().WithName("expression|human|healthy"));

         _simulationConfiguration.AddModuleConfiguration(new OSPSuite.Core.Domain.ModuleConfiguration(new Module()));

         return base.Context();
      }

      protected override Task Because()
      {
         _result = sut.MapToSnapshot(_simulationConfiguration).Result;
         return Task.FromResult(_result);
      }

      [Observation]
      public void settings_are_mapped_to_snapshot()
      {
         A.CallTo(() => _simulationSettingsMapper.MapToSnapshot(_simulationConfiguration.SimulationSettings)).MustHaveHappened();
      }

      [Observation]
      public void the_individual_is_mapped_to_snapshot()
      {
         _result.IndividualBuildingBlock.ShouldBeEqualTo("individual");
      }

      [Observation]
      public void the_expressions_are_mapped_to_snapshot()
      {
         _result.ExpressionProfiles.Length.ShouldBeEqualTo(1);
         _result.ExpressionProfiles[0].ShouldBeEqualTo("expression|human|healthy");
      }

      [Observation]
      public void the_module_configuration_mapper_maps_module_configuration_to_snapshot()
      {
         A.CallTo(() => _moduleConfigurationMapper.MapToSnapshot(_simulationConfiguration.ModuleConfigurations.First())).MustHaveHappened();
      }
   }

   public class When_mapping_to_model : concern_for_SimulationConfigurationMapper
   {
      private SimulationConfiguration _simulationConfiguration;
      private OSPSuite.Core.Domain.Builder.SimulationConfiguration _result;
      private MoBiProject _project;
      private SimulationContext _context;

      protected override Task Context()
      {
         base.Context();
         _project = new MoBiProject();
         _project.AddIndividualBuildingBlock(new IndividualBuildingBlock().WithName("individual"));
         _project.AddExpressionProfileBuildingBlock(new ExpressionProfileBuildingBlock().WithName("expression|human|healthy"));
         _project.AddModule(new Module().WithName("module"));
         _context = new SimulationContext(false, new SnapshotContext(_project, SnapshotVersions.Current));
         _simulationConfiguration = new SimulationConfiguration
         {
            Settings = new SimulationSettings(),
            ExpressionProfiles = new[] { "expression|human|healthy" },
            IndividualBuildingBlock = "individual",
            ModuleConfigurations = new[] { new ModuleConfiguration { Module = "module" } }
         };

         _simulationConfiguration.ModuleConfigurations = new[]
         {
            new ModuleConfiguration { Module = "module" },
         };

         var simulationSettings = new OSPSuite.Core.Domain.Builder.SimulationSettings();
         A.CallTo(() => _simulationSettingsMapper.MapToModel(_simulationConfiguration.Settings, _context)).Returns(simulationSettings);
         A.CallTo(() => _simulationConfigurationFactory.Create(simulationSettings)).Returns(new OSPSuite.Core.Domain.Builder.SimulationConfiguration { SimulationSettings = simulationSettings });

         return Task.CompletedTask;
      }

      protected override Task Because()
      {
         _result = sut.MapToModel(_simulationConfiguration, _context).Result;
         return Task.FromResult(_result);
      }

      [Observation]
      public void settings_are_mapped_to_snapshot()
      {
         A.CallTo(() => _simulationSettingsMapper.MapToModel(_simulationConfiguration.Settings, _context)).MustHaveHappened();
      }

      [Observation]
      public void the_individual_is_mapped_to_snapshot()
      {
         _result.Individual.ShouldBeEqualTo(_project.IndividualsCollection.First());
      }

      [Observation]
      public void the_expressions_are_mapped_to_snapshot()
      {
         _result.ExpressionProfiles.Count.ShouldBeEqualTo(1);
         _result.ExpressionProfiles[0].ShouldBeEqualTo(_project.ExpressionProfileCollection.First());
      }

      [Observation]
      public void the_module_configuration_mapper_maps_module_configuration_to_snapshot()
      {
         A.CallTo(() => _moduleConfigurationMapper.MapToModel(_simulationConfiguration.ModuleConfigurations.First(), _context)).MustHaveHappened();
      }
   }
}