using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class AddSimulationCommand : MoBiReversibleCommand
   {
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;

      public AddSimulationCommand(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _simulationId = _simulation.Id;
         ObjectType = ObjectTypes.Simulation;
         CommandType = AppConstants.Commands.AddCommand;
         Description = AppConstants.Commands.AddToProjectDescription(ObjectType, simulation.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         context.Register(_simulation);
         project.AddSimulation(_simulation);
         context.PublishEvent(new SimulationAddedEvent(_simulation));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveSimulationCommand(_simulation).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _simulation = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulation = context.Get<IMoBiSimulation>(_simulationId);
      }
   }
}