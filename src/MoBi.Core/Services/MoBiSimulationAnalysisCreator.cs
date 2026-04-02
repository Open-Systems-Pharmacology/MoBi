using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface IMoBiSimulationAnalysisCreator : ISimulationAnalysisCreator
   {
      ISimulationAnalysis CreateTimeProfileAnalysisFor(IMoBiSimulation simulation);
      ISimulationAnalysis CreatePredictedVsObservedAnalysisFor(IMoBiSimulation simulation);
      ISimulationAnalysis CreateResidualsVsTimeAnalysisFor(IMoBiSimulation simulation);
   }

   public class MoBiSimulationAnalysisCreator : SimulationAnalysisCreator, IMoBiSimulationAnalysisCreator
   {
      private readonly IChartFactory _chartFactory;
      private readonly ICloneManager _cloneManager;

      public MoBiSimulationAnalysisCreator(IContainerTask containerTask, IOSPSuiteExecutionContext executionContext,
         IChartFactory chartFactory, ICloneManager cloneManager) : base(containerTask, executionContext)
      {
         _chartFactory = chartFactory;
         _cloneManager = cloneManager;
      }

      public ISimulationAnalysis CreateTimeProfileAnalysisFor(IMoBiSimulation simulation)
      {
         var chart = _chartFactory.Create<MoBiSimulationTimeProfileChart>().WithAxes();
         AddSimulationAnalysisTo(simulation, chart);
         return chart;
      }

      public ISimulationAnalysis CreatePredictedVsObservedAnalysisFor(IMoBiSimulation simulation)
      {
         var chart = _chartFactory.Create<SimulationPredictedVsObservedChart>();
         AddSimulationAnalysisTo(simulation, chart);
         return chart;
      }

      public ISimulationAnalysis CreateResidualsVsTimeAnalysisFor(IMoBiSimulation simulation)
      {
         var chart = _chartFactory.Create<SimulationResidualVsTimeChart>();
         AddSimulationAnalysisTo(simulation, chart);
         return chart;
      }

      public override ISimulationAnalysis CreateAnalysisBasedOn(ISimulationAnalysis sourceAnalysis)
      {
         if (sourceAnalysis is CurveChart curveChart)
            return _cloneManager.Clone(curveChart) as ISimulationAnalysis;

         return null;
      }
   }
}
