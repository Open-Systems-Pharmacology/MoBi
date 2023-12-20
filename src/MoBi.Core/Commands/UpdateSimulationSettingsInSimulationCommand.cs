using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateSimulationSettingsInSimulationCommand : SimulationChangeCommandBase
   {
      private SimulationSettings _newSimulationSettings;
      private byte[] _serializedOldSettings;

      public UpdateSimulationSettingsInSimulationCommand(IMoBiSimulation simulation, SimulationSettings newSimulationSettings) : base(simulation)
      {
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.SimulationSettings;
         Description = AppConstants.Commands.UpdateSimulationSettingsInSimulation(simulation.Name);
         _newSimulationSettings = newSimulationSettings;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateSimulationSettingsInSimulationCommand(_simulation, context.Deserialize<SimulationSettings>(_serializedOldSettings));
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newSimulationSettings = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new SimulationReloadEvent(_simulation));
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _serializedOldSettings = context.Serialize(_simulation.Configuration.SimulationSettings);
         _simulation.Configuration.SimulationSettings = _newSimulationSettings;
      }
   }
}