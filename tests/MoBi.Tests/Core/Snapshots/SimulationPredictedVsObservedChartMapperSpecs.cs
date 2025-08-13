using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Snapshots.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using DataRepository = OSPSuite.Core.Domain.Data.DataRepository;

namespace MoBi.Core.Snapshots
{
   public class concern_for_SimulationPredictedVsObservedChartMapper : ContextSpecificationAsync<SimulationPredictedVsObservedChartMapper>
   {
      private IIdGenerator _idGenerator;
      private ChartMapper _chartMapper;
      private AxisMapper _axisMapper;
      private CurveMapper _curveMapper;

      protected override Task Context()
      {
         _idGenerator = A.Fake<IIdGenerator>();
         _chartMapper = A.Fake<ChartMapper>();
         _axisMapper = A.Fake<AxisMapper>();
         _curveMapper = A.Fake<CurveMapper>();

         sut = new SimulationPredictedVsObservedChartMapper(_chartMapper, _axisMapper, _curveMapper, _idGenerator);
         return Task.CompletedTask;
      }
   }

   public class When_mapping_snapshot_to_predicted_vs_observed : concern_for_SimulationPredictedVsObservedChartMapper
   {
      private OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart _result;
      private SimulationPredictedVsObservedChart _chart;

      protected override async Task Context()
      {
         await base.Context();
         _chart = new SimulationPredictedVsObservedChart
         {
            DeviationFoldValues = new[] { 2.3f }
         };
      }

      protected override Task Because()
      {
         _result = sut.MapToModel(_chart, new SimulationAnalysisContext(new List<DataRepository>(), new SnapshotContext(new MoBiProject(), SnapshotVersions.Current))).Result;
         return Task.CompletedTask;
      }

      [Observation]
      public void snapshot_deviation_fold_values_are_mapped()
      {
         _result.DeviationFoldValues.Count.ShouldBeEqualTo(1);
         _result.DeviationFoldValues[0].ShouldBeEqualTo(2.3f);
      }
   }

   public class When_mapping_predicted_vs_observed_to_snapshot : concern_for_SimulationPredictedVsObservedChartMapper
   {
      private OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart _chart;
      private SimulationPredictedVsObservedChart _result;

      protected override async Task Context()
      {
         await base.Context();
         _chart = new OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart();
         _chart.DeviationFoldValues.Add(2.3f);
      }

      protected override Task Because()
      {
         _result = sut.MapToSnapshot(_chart).Result;
         return Task.CompletedTask;
      }

      [Observation]
      public void snapshot_deviation_fold_values_are_mapped()
      {
         _result.DeviationFoldValues.Length.ShouldBeEqualTo(1);
         _result.DeviationFoldValues[0].ShouldBeEqualTo(2.3f);
      }
   }
}