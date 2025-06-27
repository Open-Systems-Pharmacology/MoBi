using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Snapshots.Mappers;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using Parameter = OSPSuite.Core.Domain.Parameter;

namespace MoBi.IntegrationTests.Snapshots
{
   public class concern_for_SimulationSettingsMapper : ContextForIntegration<SimulationSettingsMapper>
   {
      protected SimulationSettings _settings;

      protected override void Context()
      {
         base.Context();

         _settings = DomainFactoryForSpecs.CreateDefaultSimulationSettings();
      }
   }

   public class When_mapping_simulation_settings_to_model : concern_for_SimulationSettingsMapper
   {
      private Core.Snapshots.SimulationSettings _snapshot;
      private ISimulation _simulation;
      private SimulationSettings _result;

      protected override void Context()
      {
         base.Context();
         _snapshot = sut.MapToSnapshot(_settings).Result;
         _simulation = new MoBiSimulation
         {
            Model = new Model
            {
               Root = new Container().WithName("sim")
            }.WithName("sim")
         }.WithName("sim");

         var container = new Container().WithName("container");
         container.Add(new Parameter().WithName("quantity"));
         _simulation.Model.Root.Add(container);
      }

      protected override void Because()
      {
         _result = sut.MapToModel(_snapshot, new SimulationContext(false, new SnapshotContext(new MoBiProject(), SnapshotVersions.Current))).Result;
      }

      [Observation]
      public void the_properties_should_be_mapped()
      {
         _result.OutputSchema.ShouldNotBeNull();
         _result.Solver.ShouldNotBeNull();
      }
   }

   public class When_mapping_simulation_settings_to_snapshot : concern_for_SimulationSettingsMapper
   {
      private Core.Snapshots.SimulationSettings _result;

      protected override void Because()
      {
         _result = sut.MapToSnapshot(_settings).Result;
      }

      [Observation]
      public void the_output_schema_should_be_mapped()
      {
         _result.OutputSchema.ShouldNotBeNull();
         _result.OutputSchema.Count().ShouldBeEqualTo(1);
         _result.OutputSchema.First().Parameters.Length.ShouldBeEqualTo(3);
         var parameters = _result.OutputSchema.First().Parameters.ToArray();
         parameters[0].Name.ShouldBeEqualTo(Constants.Parameters.START_TIME);
         parameters[0].Value.ShouldBeEqualTo(0);
         parameters[1].Name.ShouldBeEqualTo(Constants.Parameters.END_TIME);
         parameters[1].Value.ShouldBeEqualTo(24);
         parameters[2].Name.ShouldBeEqualTo(Constants.Parameters.RESOLUTION);
         parameters[2].Value.ShouldBeEqualTo(4);
      }

      [Observation]
      public void the_solver_settings_should_be_mapped()
      {
         _result.Solver.ShouldNotBeNull();
      }
   }
}