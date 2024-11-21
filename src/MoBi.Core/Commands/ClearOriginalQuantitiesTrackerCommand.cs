using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class ClearOriginalQuantitiesTrackerCommand : MoBiReversibleCommand
   {
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;
      private readonly List<byte[]> _serializedOriginalQuantities = new List<byte[]>();

      public ClearOriginalQuantitiesTrackerCommand(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _simulationId = _simulation.Id;
         ObjectType = ObjectTypes.Simulation;
         Description = AppConstants.Commands.RemoveTrackedQuantityChanges;
         CommandType = AppConstants.Commands.DeleteCommand;
         _simulationId = _simulation.Id;
         Visible = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _serializedOriginalQuantities.AddRange(_simulation.OriginalQuantityValues.Select(context.Serialize));
         _simulation.ClearOriginalQuantities();
      }

      protected override void ClearReferences()
      {
         _simulation = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         var originalQuantities = _serializedOriginalQuantities.Select(context.Deserialize<OriginalQuantityValue>).ToList();
         return new RestoreOriginalQuantitiesTrackerCommand(_simulation, originalQuantities);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulation = context.Get<IMoBiSimulation>(_simulationId);
      }

      // Should only ever be invoked as an inverse to the ClearOriginalQuantitiesTrackerCommand command
      private class RestoreOriginalQuantitiesTrackerCommand : MoBiReversibleCommand
      {
         IMoBiSimulation _simulation;
         private readonly string _simulationId;
         private readonly IReadOnlyCollection<OriginalQuantityValue> _originalQuantities;

         public RestoreOriginalQuantitiesTrackerCommand(IMoBiSimulation simulation, IReadOnlyCollection<OriginalQuantityValue> originalQuantities)
         {
            _simulation = simulation;
            _simulationId = _simulation.Id;
            _originalQuantities = originalQuantities;
            Visible = false;

            ObjectType = ObjectTypes.Simulation;
            Description = AppConstants.Commands.RestoreTrackedQuantityChanges;
            CommandType = AppConstants.Commands.AddCommand;
         }

         protected override void ExecuteWith(IMoBiContext context)
         {
            _originalQuantities.Each(x => _simulation.AddOriginalQuantityValue(x));
            context.PublishEvent(new SimulationStatusChangedEvent(_simulation));
         }

         protected override void ClearReferences()
         {
            _simulation = null;
         }

         protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return new ClearOriginalQuantitiesTrackerCommand(_simulation);
         }

         public override void RestoreExecutionData(IMoBiContext context)
         {
            _simulation = context.Get<IMoBiSimulation>(_simulationId);
         }
      }
   }
}