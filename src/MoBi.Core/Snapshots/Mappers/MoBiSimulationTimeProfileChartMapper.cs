using MoBi.Core.Chart;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public class MoBiSimulationTimeProfileChartMapper : NewableCurveChartMapper<MoBiSimulationTimeProfileChart>
{
   public MoBiSimulationTimeProfileChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
   {
   }
}
