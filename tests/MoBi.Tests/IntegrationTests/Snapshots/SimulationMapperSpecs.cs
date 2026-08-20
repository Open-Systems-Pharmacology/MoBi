using System.Linq;
using MoBi.Core.Chart;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Core.Snapshots;
using MoBi.Core.Snapshots.Mappers;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Container;
using SimulationPredictedVsObservedChart = OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart;
using SnapshotSimulation = MoBi.Core.Snapshots.Simulation;

namespace MoBi.IntegrationTests.Snapshots
{
   public class concern_for_SimulationMapper : ContextForIntegration<SimulationMapper>
   {
      protected MoBiSimulation _simulation;
      private ISimulationConfigurationFactory _simulationConfigurationFactory;
      protected MoBiProject _project;

      private DataRepository _dataRepository;
      protected Parameter _parameter;
      protected MoleculeAmount _moleculeAmount;

      protected override void Context()
      {
         base.Context();
         _project = new MoBiProject();
         _simulation = new MoBiSimulation
         {
            Model = new Model
            {
               Root = new Container().WithName(Constants.ROOT)
            }.WithName("sim")
         }.WithName("sim");

         var container = new Container().WithName("container");
         _parameter = new Parameter().WithName("quantity").WithValue(1);

         container.Add(_parameter);
         _simulation.Model.Root.Add(container);

         _simulation.AddOriginalQuantityValue(new OriginalQuantityValue
         {
            Dimension = _parameter.Dimension,
            DisplayUnit = _parameter.DisplayUnit,
            Path = new ObjectPath(container.Name, _parameter.Name),
            Type = OriginalQuantityValue.Types.Quantity,
            Value = _parameter.Value
         });

         _parameter.Value = 5;

         _moleculeAmount = new MoleculeAmount().WithName("amount").WithScaleFactor(1.0);
         container.Add(_moleculeAmount);

         _simulation.AddOriginalQuantityValue(new OriginalQuantityValue
         {
            Path = new ObjectPath(container.Name, _moleculeAmount.Name),
            Type = OriginalQuantityValue.Types.ScaleDivisor,
            Value = _moleculeAmount.ScaleDivisor
         });

         _simulationConfigurationFactory = IoC.Resolve<ISimulationConfigurationFactory>();
         _dataRepository = DomainHelperForSpecs.ObservedData().WithName("obsdata");

         _simulation.Configuration = _simulationConfigurationFactory.Create(DomainFactoryForSpecs.CreateDefaultSimulationSettings());

         _simulation.OutputSelections.AddOutput(new QuantitySelection("sim|container|quantity"));

         _simulation.OutputMappings.Add(new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(_simulation, new QuantitySelection("container|quantity")),
            WeightedObservedData = new WeightedObservedData(_dataRepository)
         });

         _simulation.AddAnalysis(new MoBiSimulationTimeProfileChart());
         _simulation.AddAnalysis(new SimulationResidualVsTimeChart());
         _simulation.AddAnalysis(new SimulationPredictedVsObservedChart());
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
         _result.Charts.Length.ShouldBeEqualTo(1);
         _result.PredictedVsObservedCharts.Length.ShouldBeEqualTo(1);
         _result.ResidualVsTimeCharts.Length.ShouldBeEqualTo(1);
      }

      [Observation]
      public void parameter_value_changes_should_be_stored()
      {
         _result.Parameters.Length.ShouldBeEqualTo(1);
         _result.Parameters.First().Path.ShouldBeEqualTo(_parameter.ConsolidatedPath());
      }

      [Observation]
      public void scale_divisor_changes_should_be_stored()
      {
         _result.ScaleDivisors.Length.ShouldBeEqualTo(1);
         _result.ScaleDivisors.First().Path.ShouldBeEqualTo(_moleculeAmount.ConsolidatedPath());
      }

      [Observation]
      public void output_selections_should_be_mapped()
      {
         _result.OutputSelections.Count().ShouldBeEqualTo(1);
         _result.OutputSelections.First().ShouldBeEqualTo("sim|container|quantity");
      }
   }

   public abstract class concern_for_mapping_analyses_to_simulation : concern_for_SimulationMapper
   {
      protected MoBiSimulation _simulationWithAnalyses;
      protected SnapshotSimulation _snapshot;
      protected SimulationContext _context;
      protected bool _run;

      protected override void Context()
      {
         base.Context();

         _simulationWithAnalyses = new MoBiSimulation().WithName("sim");
         SetupResults();

         _snapshot = new SnapshotSimulation
         {
            Charts = new[]
            {
               new OSPSuite.Core.Snapshots.CurveChart
               {
                  Curves = new[]
                  {
                     new OSPSuite.Core.Snapshots.Curve
                     {
                        Name = "simulation output curve",
                        X = "Time",
                        Y = "sim|Comp|Liver|Cell|Concentration",
                        CurveOptions = new OSPSuite.Core.Snapshots.CurveOptions()
                     }
                  }
               }
            }
         };

         _context = new SimulationContext(_run, new SnapshotContext(_project, OSPSuite.Core.Snapshots.SnapshotVersions.Current));
      }

      protected abstract void SetupResults();

      protected override void Because()
      {
         sut.MapAnalysesToSimulation(_simulationWithAnalyses, _snapshot, _context);
      }
   }

   public class When_mapping_analyses_to_a_simulation_that_has_results : concern_for_mapping_analyses_to_simulation
   {
      protected override void Context()
      {
         _run = true;
         base.Context();
      }

      protected override void SetupResults()
      {
         _simulationWithAnalyses.ResultsDataRepository = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("sim");
      }

      [Observation]
      public void the_time_profile_chart_should_be_added_to_the_simulation()
      {
         _simulationWithAnalyses.Charts.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_curve_referencing_the_simulation_output_should_be_created_from_the_result_data()
      {
         _simulationWithAnalyses.Charts.Single().Curves.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_mapping_analyses_to_a_simulation_that_has_no_results : concern_for_mapping_analyses_to_simulation
   {
      protected override void Context()
      {
         _run = false;
         base.Context();
      }

      protected override void SetupResults()
      {
      }

      [Observation]
      public void the_time_profile_chart_should_be_added_to_the_simulation()
      {
         _simulationWithAnalyses.Charts.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_curve_referencing_the_simulation_output_cannot_be_resolved_without_results()
      {
         _simulationWithAnalyses.Charts.Single().Curves.Count.ShouldBeEqualTo(0);
      }
   }
}