using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateDefaultSimulationSettingsInProjectCommand : MoBiReversibleCommand
   {
      private SimulationSettings _simulationSettings;
      private byte[] _serializedOldSimulationSettings;

      public UpdateDefaultSimulationSettingsInProjectCommand(SimulationSettings simulationSettings)
      {
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.SimulationSettings;
         Description = AppConstants.Commands.UpdateProjectDefaultSimulationSettings;
         _simulationSettings = simulationSettings;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _serializedOldSimulationSettings = context.Serialize(context.CurrentProject.SimulationSettings);
         context.CurrentProject.SimulationSettings = _simulationSettings;
         context.PublishEvent(new DefaultSimulationSettingsUpdatedEvent(context.CurrentProject.SimulationSettings));
      }

      protected override void ClearReferences()
      {
         _simulationSettings = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateDefaultSimulationSettingsInProjectCommand(_simulationSettings);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulationSettings = context.Deserialize<SimulationSettings>(_serializedOldSimulationSettings);
      }
   }
}