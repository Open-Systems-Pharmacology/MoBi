﻿using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Presenter.Simulation
{
   public static class SimulationItems
   {
      private static readonly List<ISubPresenterItem> _allSimulationItems = new List<ISubPresenterItem>();

      // public static SimulationItem<IEditSimulationConfigurationPresenter> SimulationConfiguration = createFor<IEditSimulationConfigurationPresenter>();
      // public static SimulationItem<ISelectAndEditMoleculesStartValuesPresenter> MoleculeStartValues = createFor<ISelectAndEditMoleculesStartValuesPresenter>();
      // public static SimulationItem<ISelectAndEditParameterStartValuesPresenter> ParameterStartValues = createFor<ISelectAndEditParameterStartValuesPresenter>();
      // public static SimulationItem<IFinalOptionsPresenter> FinalOptions = createFor<IFinalOptionsPresenter>();

      private static SimulationItem<T> createFor<T>() where T : ISimulationItemPresenter
      {
         var simulationItem = new SimulationItem<T> { Index = _allSimulationItems.Count() };
         _allSimulationItems.Add(simulationItem);
         return simulationItem;
      }

      public static IReadOnlyList<ISubPresenterItem> All => _allSimulationItems;

      public static IReadOnlyList<ISubPresenterItem> AllConfigure => new List<ISubPresenterItem> { /*SimulationConfiguration, MoleculeStartValues, ParameterStartValues*/ };
   }

   public class SimulationItem<TSimulationItemPresenter> : SubPresenterItem<TSimulationItemPresenter> where TSimulationItemPresenter : ISimulationItemPresenter
   {
   }
}