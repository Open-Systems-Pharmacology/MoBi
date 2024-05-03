using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class UpdateSolverAndSchemaInSimulationCommand : SimulationChangeCommandBase
   {
      private SolverSettings _newSolverSettings;
      private OutputSchema _newOutputSchema;
      private OutputSchema _oldOutputSchema;
      private SolverSettings _oldSolverSettings;

      public UpdateSolverAndSchemaInSimulationCommand(IMoBiSimulation simulation, SolverSettings solverSettings, OutputSchema outputSchema) : base(simulation)
      {
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.SimulationSettings;
         Description = AppConstants.Commands.UpdateSimulationSolverSettingsAndSchemaInSimulation(simulation.Name);
         _newSolverSettings = solverSettings;
         _newOutputSchema = outputSchema;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateSolverAndSchemaInSimulationCommand(_simulation, _oldSolverSettings, _oldOutputSchema);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newSolverSettings = null;
         _newOutputSchema = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new SimulationReloadEvent(_simulation));
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldOutputSchema = _simulation.Configuration.SimulationSettings.OutputSchema;
         _oldSolverSettings = _simulation.Configuration.SimulationSettings.Solver;

         _simulation.Configuration.SimulationSettings.OutputSchema = _newOutputSchema;
         _simulation.Configuration.SimulationSettings.Solver = _newSolverSettings;
      }
   }
}