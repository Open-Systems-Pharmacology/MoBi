using System;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Services
{
   public interface IQuantityValueInSimulationChangeTracker
   {
      void TrackChanges(IQuantity quantity, IMoBiSimulation simulation, Action<IQuantity> actionModifyingQuantity);
   }

   public class QuantityValueInSimulationChangeTracker : IQuantityValueInSimulationChangeTracker
   {
      private readonly IQuantityToOriginalQuantityValueMapper _quantityToOriginalQuantityValueMapper;
      private readonly IEventPublisher _eventPublisher;
      private readonly IEntityPathResolver _entityPathResolver;

      public QuantityValueInSimulationChangeTracker(IQuantityToOriginalQuantityValueMapper quantityToOriginalQuantityValueMapper, IEventPublisher eventPublisher, IEntityPathResolver entityPathResolver)
      {
         _quantityToOriginalQuantityValueMapper = quantityToOriginalQuantityValueMapper;
         _eventPublisher = eventPublisher;
         _entityPathResolver = entityPathResolver;
      }

      public void TrackChanges(IQuantity quantity, IMoBiSimulation simulation, Action<IQuantity> actionModifyingQuantity)
      {
         storeOriginalQuantityValue(quantity, simulation);

         actionModifyingQuantity(quantity);

         checkForOriginalValueRestored(quantity, simulation);
      }

      private void storeOriginalQuantityValue(IQuantity quantity, IMoBiSimulation simulation)
      {
         var pathForQuantity = _entityPathResolver.ObjectPathFor(quantity);
         if (simulation.OriginalQuantityValueFor(pathForQuantity) != null)
            return;

         simulation.AddOriginalQuantityValue(_quantityToOriginalQuantityValueMapper.MapFrom(quantity));
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }

      private bool areEquivalent(OriginalQuantityValue newParameterValue, OriginalQuantityValue oldParameterValue)
      {
         return newParameterValue.Value == oldParameterValue.Value &&
                newParameterValue.DisplayUnit == oldParameterValue.DisplayUnit &&
                newParameterValue.Path.Equals(oldParameterValue.Path) &&
                newParameterValue.ValueOrigin.Equals(oldParameterValue.ValueOrigin);
      }

      /// <summary>
      ///    If the result of the tracked change is that the new quantity becomes the same as the original quantity,
      ///    then we remove the original quantity value from the simulation.
      /// </summary>
      private void checkForOriginalValueRestored(IQuantity quantity, IMoBiSimulation simulation)
      {
         var newParameterValue = _quantityToOriginalQuantityValueMapper.MapFrom(quantity);
         var oldParameterValue = simulation.OriginalQuantityValueFor(newParameterValue.Path);

         if (oldParameterValue == null || !areEquivalent(newParameterValue, oldParameterValue))
            return;

         simulation.RemoveOriginalQuantityValue(newParameterValue.Path);
         _eventPublisher.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }
   }
}