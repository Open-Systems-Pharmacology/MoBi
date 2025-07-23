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
      void TrackQuantityChange(IQuantity quantity, IMoBiSimulation simulation, Action<IQuantity> actionModifyingQuantity, bool withEvents = true);
      void TrackScaleChange(MoleculeAmount moleculeAmount, IMoBiSimulation simulation, Action<MoleculeAmount> actionModifyingScaleDivisor, bool withEvents = true);
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

      public void TrackQuantityChange(IQuantity quantity, IMoBiSimulation simulation, Action<IQuantity> actionModifyingQuantity, bool withEvents = true)
      {
         storeOriginalQuantityValue(quantity, simulation, withEvents);

         actionModifyingQuantity(quantity);

         checkForOriginalQuantityRestored(quantity, simulation, withEvents);
      }

      public void TrackScaleChange(MoleculeAmount moleculeAmount, IMoBiSimulation simulation, Action<MoleculeAmount> actionModifyingScaleDivisor, bool withEvents)
      {
         storeOriginalScaleFactor(moleculeAmount, simulation, withEvents);
         actionModifyingScaleDivisor(moleculeAmount);
         checkForOriginalQuantityRestored(moleculeAmount, simulation, withEvents);
      }

      private void storeOriginalScaleFactor(MoleculeAmount moleculeAmount, IMoBiSimulation simulation, bool withEvents)
      {
         var originalQuantityValue = _quantityToOriginalQuantityValueMapper.MapFrom(moleculeAmount);
         if (!shouldStore(originalQuantityValue, simulation))
            return;

         simulation.AddOriginalQuantityValue(originalQuantityValue);
         if (withEvents)
            _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }

      private void storeOriginalQuantityValue(IQuantity quantity, IMoBiSimulation simulation, bool withEvents)
      {
         var originalQuantityValue = _quantityToOriginalQuantityValueMapper.MapFrom(quantity);
         if (!shouldStore(originalQuantityValue, simulation))
            return;

         simulation.AddOriginalQuantityValue(originalQuantityValue);
         if (withEvents)
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
      private void checkForOriginalQuantityRestored(IQuantity quantity, IMoBiSimulation simulation, bool withEvents)
      {
         var newParameterValue = _quantityToOriginalQuantityValueMapper.MapFrom(quantity);
         checkForOriginalRestored(simulation, newParameterValue, withEvents);
      }

      private void checkForOriginalQuantityRestored(MoleculeAmount moleculeAmount, IMoBiSimulation simulation, bool withEvents)
      {
         var newParameterValue = _quantityToOriginalQuantityValueMapper.MapFrom(moleculeAmount);
         checkForOriginalRestored(simulation, newParameterValue, withEvents);
      }

      private void checkForOriginalRestored(IMoBiSimulation simulation, OriginalQuantityValue newParameterValue, bool withEvents)
      {
         var oldParameterValue = simulation.OriginalQuantityValueFor(newParameterValue);

         if (oldParameterValue == null || !areEquivalent(newParameterValue, oldParameterValue))
            return;

         simulation.RemoveOriginalQuantityValue(oldParameterValue);
         if(withEvents)
            _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }
   }
}