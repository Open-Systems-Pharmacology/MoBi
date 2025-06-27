using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Core.Snapshots;
using MoBi.Core.Snapshots.Mappers;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Container;
using SimulationPredictedVsObservedChart = OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart;

namespace MoBi.IntegrationTests.Snapshots
{
   public class concern_for_SimulationMapper : ContextForIntegration<SimulationMapper>
   {
      protected MoBiSimulation _simulation;
      private ISimulationConfigurationFactory _simulationConfigurationFactory;
      protected MoBiProject _project;

      private DataRepository _dataRepository;

      protected override void Context()
      {
         base.Context();
         _project = new MoBiProject();
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

         _simulationConfigurationFactory = IoC.Resolve<ISimulationConfigurationFactory>();
         _dataRepository = DomainHelperForSpecs.ObservedData().WithName("obsdata");

         _simulation.Configuration = _simulationConfigurationFactory.Create(DomainFactoryForSpecs.CreateDefaultSimulationSettings());

         _simulation.OutputSelections.AddOutput(new QuantitySelection("sim|container|quantity"));

         _simulation.OutputMappings.Add(new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(_simulation, new QuantitySelection("container|quantity")),
            WeightedObservedData = new WeightedObservedData(_dataRepository)
         });

         _simulation.Chart = new CurveChart();
         _simulation.ResidualVsTimeChart = new SimulationResidualVsTimeChart();
         _simulation.PredictedVsObservedChart = new SimulationPredictedVsObservedChart();
         _simulation.ResultsDataRepository = DomainHelperForSpecs.ObservedData().WithName("results");
      }
   }

   public class When_mapping_simulation_to_snapshot : concern_for_SimulationMapper
   {
      private Simulation _result;

      protected override void Because()
      {
         _result = sut.MapToSnapshot(_simulation, _project).Result;
      }

      [Observation]
      public void output_mappings_should_be_mapped()
      {
         _result.OutputMappings.Length.ShouldBeEqualTo(1);
         var outputMapping = _result.OutputMappings.First();
         outputMapping.Path.ShouldBeEqualTo("sim|container|quantity");
         outputMapping.ObservedData.ShouldBeEqualTo("obsdata");
      }

      [Observation]
      public void result_charts_are_mapped()
      {
         _result.Chart.ShouldNotBeNull();
      }

      [Observation]
      public void output_selections_should_be_mapped()
      {
         _result.OutputSelections.Count().ShouldBeEqualTo(1);
         _result.OutputSelections.First().ShouldBeEqualTo("sim|container|quantity");
      }
   }
}