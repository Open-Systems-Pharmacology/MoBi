using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;

namespace MoBi.Core.Commands
{
   public class UpdateOutputSelectionsInSimulationCommand : SimulationChangeCommandBase
   {
      private OutputSelections _outputSelections;
      private OutputSelections _oldOutputSelections;

      public UpdateOutputSelectionsInSimulationCommand(OutputSelections outputSelections, IMoBiSimulation simulation) : base(simulation)
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
         _simulation.Settings.OutputSelections = _outputSelections;
         context.Resolve<IBuildingBlockVersionUpdater>().UpdateBuildingBlockVersion(_simulation.Settings, shouldIncrementVersion: true);
         Description = AppConstants.Commands.UpdateOutputSelectionInSimulationDescription(_simulation.Name);

         context.PublishEvent(new SimulationOutputSelectionsChangedEvent(_simulation));
      }
   }
}