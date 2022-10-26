using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Events;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class RemoveSimulationCommand : MoBiReversibleCommand
   {
      private IMoBiSimulation _simulation;
      private byte[] _serializationStream;

      public RemoveSimulationCommand(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         ObjectType = ObjectTypes.Simulation;
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveFromProjectDescription(ObjectType, simulation.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         context.CurrentProject.RemoveSimulation(_simulation);
         context.UnregisterSimulation(_simulation);
         context.PublishEvent(new SimulationRemovedEvent(_simulation));
         
         //Do not serialize ResultsDataRepository
         _simulation.ResultsDataRepository = null; 
         _serializationStream = context.Serialize(_simulation);
      }

      protected override void ClearReferences()
      {
         _simulation = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddSimulationCommand(_simulation).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulation = context.Deserialize<IMoBiSimulation>(_serializationStream);
      }
   }
}