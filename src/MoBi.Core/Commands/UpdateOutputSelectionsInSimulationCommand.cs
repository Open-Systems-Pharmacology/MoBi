using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateOutputSelectionsInSimulationCommand : SimulationChangeCommandBase
   {
      private OutputSelections _outputSelections;
      private OutputSelections _oldOutputSelections;

      public UpdateOutputSelectionsInSimulationCommand(OutputSelections outputSelections, IMoBiSimulation simulation) : base(outputSelections, simulation)
      {
         _outputSelections = outputSelections;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.OutputSelections;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateOutputSelectionsInSimulationCommand(_oldOutputSelections, _simulation).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _outputSelections = null;
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldOutputSelections = _simulation.OutputSelections;
         _simulation.SimulationSettings.OutputSelections = _outputSelections;
         Description = AppConstants.Commands.UpdateOutputSelectionInSimulationDescription(_simulation.Name);
      }
   }
}