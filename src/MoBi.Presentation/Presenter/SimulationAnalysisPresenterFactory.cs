using MoBi.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.Presenter
{
   public class SimulationAnalysisPresenterFactory : OSPSuite.Presentation.Presenters.SimulationAnalysisPresenterFactory
   {
      public SimulationAnalysisPresenterFactory(IContainer container) : base(container)
      {
      }

      protected override ISimulationAnalysisPresenter PresenterFor(ISimulationAnalysis simulationAnalysis, IContainer container)
      {
         if (simulationAnalysis.IsAnImplementationOf<MoBiSimulationTimeProfileChart>())
            return container.Resolve<ISimulationChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<SimulationPredictedVsObservedChart>())
            return container.Resolve<ISimulationPredictedVsObservedChartPresenter>();

         if (simulationAnalysis.IsAnImplementationOf<SimulationResidualVsTimeChart>())
            return container.Resolve<ISimulationResidualVsTimeChartPresenter>();

         return null;
      }
   }
}