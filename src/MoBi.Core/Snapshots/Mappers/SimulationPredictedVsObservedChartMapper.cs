using System.Threading.Tasks;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Snapshots.Mappers;
using ModelSimulationPredictedVsObservedChart = OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart;

namespace MoBi.Core.Snapshots.Mappers;

public class SimulationPredictedVsObservedChartMapper : NewableCurveChartMapper<ModelSimulationPredictedVsObservedChart, SimulationPredictedVsObservedChart>
{
   public SimulationPredictedVsObservedChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
   {
   }

   public override async Task<SimulationPredictedVsObservedChart> MapToSnapshot(ModelSimulationPredictedVsObservedChart chart)
   {
      var snapshot = await base.MapToSnapshot(chart);

      snapshot.DeviationFoldValues = chart.DeviationFoldValues.ToArray();
      return snapshot;
   }

   public override async Task<ModelSimulationPredictedVsObservedChart> MapToModel(SimulationPredictedVsObservedChart snapshot, SimulationAnalysisContext simulationAnalysisContext)
   {
      var chart = await base.MapToModel(snapshot, simulationAnalysisContext);
      chart.DeviationFoldValues.AddRange(snapshot.DeviationFoldValues);
      return chart;
   }
}