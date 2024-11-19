using System;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Services
{
   public interface IQuantityValueInSimulationChangeTracker
   {
      void TrackQuantityChange(IQuantity quantity, IMoBiSimulation simulation, Action<IQuantity> actionModifyingQuantity);
      void TrackScaleChange(MoleculeAmount moleculeAmount, IMoBiSimulation simulation, Action<MoleculeAmount> actionModifyingScaleDivisor);
   }

   public class QuantityValueInSimulationChangeTracker : IQuantityValueInSimulationChangeTracker
   {
      private readonly IQuantityToOriginalQuantityValueMapper _quantityToOriginalQuantityValueMapper;
      private readonly IEventPublisher _eventPublisher;

      public QuantityValueInSimulationChangeTracker(IQuantityToOriginalQuantityValueMapper quantityToOriginalQuantityValueMapper, IEventPublisher eventPublisher)
      {
         _quantityToOriginalQuantityValueMapper = quantityToOriginalQuantityValueMapper;
         _eventPublisher = eventPublisher;
      }

      public void TrackQuantityChange(IQuantity quantity, IMoBiSimulation simulation, Action<IQuantity> actionModifyingQuantity)
      {
         storeOriginalQuantityValue(quantity, simulation);

         actionModifyingQuantity(quantity);

         checkForOriginalQuantityRestored(quantity, simulation);
      }

      public void TrackScaleChange(MoleculeAmount moleculeAmount, IMoBiSimulation simulation, Action<MoleculeAmount> actionModifyingScaleDivisor)
      {
         storeOriginalScaleFactor(moleculeAmount, simulation);
         actionModifyingScaleDivisor(moleculeAmount);
         checkForOriginalQuantityRestored(moleculeAmount, simulation);
      }

      private void storeOriginalScaleFactor(MoleculeAmount moleculeAmount, IMoBiSimulation simulation)
      {
         var originalQuantityValue = _quantityToOriginalQuantityValueMapper.MapFrom(moleculeAmount);
         if (!shouldStore(originalQuantityValue, simulation))
            return;

         simulation.AddOriginalQuantityValue(originalQuantityValue);
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }

      private void storeOriginalQuantityValue(IQuantity quantity, IMoBiSimulation simulation)
      {
         var originalQuantityValue = _quantityToOriginalQuantityValueMapper.MapFrom(quantity);
         if (!shouldStore(originalQuantityValue, simulation))
            return;

         simulation.AddOriginalQuantityValue(originalQuantityValue);
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }

      private bool shouldStore(OriginalQuantityValue quantity, IMoBiSimulation simulation)
      {
         return simulation.OriginalQuantityValueFor(quantity) == null;
      }

      private bool areEquivalent(OriginalQuantityValue newParameterValue, OriginalQuantityValue oldParameterValue)
      {
         return newParameterValue.Value == oldParameterValue.Value &&
                newParameterValue.DisplayUnit == oldParameterValue.DisplayUnit &&
                newParameterValue.Id.Equals(oldParameterValue.Id) &&
                newParameterValue.ValueOrigin.Equals(oldParameterValue.ValueOrigin);
      }

      /// <summary>
      ///    If the result of the tracked change is that the new quantity becomes the same as the original quantity,
      ///    then we remove the original quantity value from the simulation.
      /// </summary>
      private void checkForOriginalQuantityRestored(IQuantity quantity, IMoBiSimulation simulation)
      {
         var newParameterValue = _quantityToOriginalQuantityValueMapper.MapFrom(quantity);
         checkForOriginalRestored(simulation, newParameterValue);
      }

      private void checkForOriginalQuantityRestored(MoleculeAmount moleculeAmount, IMoBiSimulation simulation)
      {
         var newParameterValue = _quantityToOriginalQuantityValueMapper.MapFrom(moleculeAmount);
         checkForOriginalRestored(simulation, newParameterValue);
      }

      private void checkForOriginalRestored(IMoBiSimulation simulation, OriginalQuantityValue newParameterValue)
      {
         var oldParameterValue = simulation.OriginalQuantityValueFor(newParameterValue);

         if (oldParameterValue == null || !areEquivalent(newParameterValue, oldParameterValue))
            return;

         simulation.RemoveOriginalQuantityValue(oldParameterValue);
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }
   }
}