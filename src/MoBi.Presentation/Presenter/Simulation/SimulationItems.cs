using System.Collections.Generic;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Presenter.Simulation
{
   public static class SimulationItems
   {
      private static readonly List<ISubPresenterItem> _allSimulationItems = new List<ISubPresenterItem>();

      public static readonly SimulationItem<IEditModuleConfigurationsPresenter> ModuleConfiguration = createFor<IEditModuleConfigurationsPresenter>();
      public static readonly SimulationItem<IEditIndividualAndExpressionConfigurationsPresenter> IndividualAndExpressionConfiguration = createFor<IEditIndividualAndExpressionConfigurationsPresenter>();
      public static readonly SimulationItem<IEditMoleculeCalculationMethodsPresenter> MoleculeCalculationMethodsConfiguration = createFor<IEditMoleculeCalculationMethodsPresenter>();

      private static SimulationItem<T> createFor<T>() where T : ISimulationConfigurationItemPresenter
      {
         var simulationItem = new SimulationItem<T> { Index = _allSimulationItems.Count };
         _allSimulationItems.Add(simulationItem);
         return simulationItem;
      }

      public static IReadOnlyList<ISubPresenterItem> All => _allSimulationItems;
   }

   public class SimulationItem<TSimulationItemPresenter> : SubPresenterItem<TSimulationItemPresenter> where TSimulationItemPresenter : ISimulationConfigurationItemPresenter
   {
   }
}