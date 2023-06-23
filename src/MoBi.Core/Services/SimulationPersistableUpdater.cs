using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public interface ISimulationPersistableUpdater : OSPSuite.Core.Domain.Services.ISimulationPersistableUpdater
   {
      /// <summary>
      /// All selectable quantities defined in the <paramref name="simulation"/> will be set to not persistable
      /// </summary>
      void ResetPersistable(IMoBiSimulation simulation);


      /// <summary>
      /// Reads the simulation settings and ensure that only the existing quantities defined in the <see cref="OutputSelections"/> are 
      /// set to be persitable
      /// </summary>
      void UpdatePersistableFromSettings(IMoBiSimulation simulation);

      /// <summary>
      /// Returns <c>true</c> if the quantity can potentially be selected to be persistable otherwise false.
      /// If the parameter <paramref name="forceAmountToBeSelectable"/> is <c>true</c>, amounts are set to be selectable even in a concentration based project
      /// Default is false
      /// </summary>
      bool QuantityIsSelectable(IQuantity quantity, bool forceAmountToBeSelectable = false);

      /// <summary>
      /// Returns the list of <see cref="IQuantity"/> that can potentially be selected by the user to be persitable
      /// </summary>
      IEnumerable<IQuantity> AllSelectableIn(IMoBiSimulation simulation);
   }

   public class SimulationPersistableUpdater : OSPSuite.Core.Domain.Services.SimulationPersistableUpdater, ISimulationPersistableUpdater
   {
      private readonly IContainerTask _containerTask;
      private readonly IReactionDimensionRetriever _reactionDimensionRetriever;

      public SimulationPersistableUpdater(IEntitiesInSimulationRetriever entitiesInSimulationRetriever, IContainerTask containerTask, IReactionDimensionRetriever reactionDimensionRetriever) : 
         base(entitiesInSimulationRetriever)
      {
         _containerTask = containerTask;
         _reactionDimensionRetriever = reactionDimensionRetriever;
      }

      public void ResetPersistable(IMoBiSimulation simulation)
      {
         var allQuantities = allQuantitiesFrom(simulation, includeAmounts: true);
         allQuantities.Each(o => o.Persistable = false);
      }

      public void UpdatePersistableFromSettings(IMoBiSimulation simulation)
      {
         var allQuantities = allQuantitiesFrom(simulation, includeAmounts: false);
         ResetPersistable(simulation);
         foreach (var selectedQuantity in simulation.OutputSelections.AllOutputs)
         {
            var quantity = allQuantities[selectedQuantity.Path];
            if (quantity != null)
               quantity.Persistable = true;
         }
      }

      private PathCache<IQuantity> allQuantitiesFrom(IMoBiSimulation simulation, bool includeAmounts)
      {
         return _containerTask.CacheAllChildrenSatisfying<IQuantity>(simulation.Model.Root, x => QuantityIsSelectable(x, includeAmounts));
      }

      public bool QuantityIsSelectable(IQuantity quantity, bool forceAmountToBeSelectable)
      {
         if (quantity.IsAnImplementationOf<Observer>())
            return true;

         if (quantity.IsAnImplementationOf<MoleculeAmount>() && forceAmountToBeSelectable)
            return true;

         if (isMoleculeAmountInAmountBasedProject(quantity))
            return true;

         if (isMoleculeConcentrationInConcentrationBasedProject(quantity))
            return true;

         if (isPersistableParameter(quantity))
            return true;

         return false;
      }

      private bool isPersistableParameter(IQuantity quantity)
      {
         return quantity.IsAnImplementationOf<IParameter>() && quantity.Persistable;
      }

      private bool isMoleculeConcentrationInConcentrationBasedProject(IQuantity quantity)
      {
         return quantity.IsAnImplementationOf<IParameter>()
            && _reactionDimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.ConcentrationBased
            && quantity.IsNamed(Constants.Parameters.CONCENTRATION)
            && quantity.ParentContainer.IsAnImplementationOf<MoleculeAmount>();
      }

      private bool isMoleculeAmountInAmountBasedProject(IQuantity quantity)
      {
         return quantity.IsAnImplementationOf<MoleculeAmount>()
            && _reactionDimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.AmountBased;
      }

      public IEnumerable<IQuantity> AllSelectableIn(IMoBiSimulation simulation)
      {
         return allQuantitiesFrom(simulation, includeAmounts: false);
      }
   }
}