using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Snapshots.Mappers;
using ModelSimulationResidualVsTimeChart = OSPSuite.Core.Chart.Simulations.SimulationResidualVsTimeChart;

namespace MoBi.Core.Snapshots.Mappers;

public class SimulationResidualVsTimeChartMapper : NewableCurveChartMapper<ModelSimulationResidualVsTimeChart>
{
   public SimulationResidualVsTimeChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
   {
   }
}