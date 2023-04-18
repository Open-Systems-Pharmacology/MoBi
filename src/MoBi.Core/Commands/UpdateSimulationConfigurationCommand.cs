using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateSimulationConfigurationCommand : OSPSuiteCommand<IMoBiContext>
   {
      private SimulationConfiguration _newConfiguration;
      private IMoBiSimulation _simulation;

      public UpdateSimulationConfigurationCommand(IMoBiSimulation simulation, SimulationConfiguration newConfiguration)
      {
         ObjectType = ObjectTypes.Simulation;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.ChangeSimulationConfiguration(simulation.Name);
         _newConfiguration = newConfiguration;
         _simulation = simulation;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _simulation.Configuration = _newConfiguration;
      }

      protected override void ClearReferences()
      {
         _simulation = null;
         _newConfiguration = null;
      }
   }
}