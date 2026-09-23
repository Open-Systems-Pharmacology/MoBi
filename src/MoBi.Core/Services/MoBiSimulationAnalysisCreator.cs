using System.Linq;
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
      void AddTimeProfileAnalysisIfMissing(IMoBiSimulation simulation);
   }

   public class MoBiSimulationAnalysisCreator : SimulationAnalysisCreator, IMoBiSimulationAnalysisCreator
   {
      private readonly IContainerTask _containerTask;
      private readonly IChartFactory _chartFactory;
      private readonly ICloneManager _cloneManager;

      public MoBiSimulationAnalysisCreator(IContainerTask containerTask, IOSPSuiteExecutionContext executionContext,
         IChartFactory chartFactory, ICloneManager cloneManager) : base(containerTask, executionContext)
      {
         _containerTask = containerTask;
         _chartFactory = chartFactory;
         _cloneManager = cloneManager;
      }

      public void AddTimeProfileAnalysisIfMissing(IMoBiSimulation simulation)
      {
         if (simulation.Analyses.OfType<MoBiSimulationTimeProfileChart>().Any())
            return;

         var chart = _chartFactory.Create<MoBiSimulationTimeProfileChart>().WithAxes();
         chart.Name = _containerTask.CreateUniqueName(simulation.Analyses, DefaultAnalysisNameFor(chart), canUseBaseName: true);
         simulation.AddAnalysis(chart);
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
         var updateable = sourceAnalysis as IUpdatable;
         return _cloneManager.Clone(updateable) as ISimulationAnalysis;
      }
   }
}
