using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateOutputSelectionInBuildingBlockCommand : BuildingBlockChangeCommandBase<ISimulationSettings>
   {
      private OutputSelections _outputSelections;
      private OutputSelections _oldOutputSelections;


      public UpdateOutputSelectionInBuildingBlockCommand(OutputSelections outputSelections, ISimulationSettings buildingBlock)
         : base(buildingBlock)
      {
         _outputSelections = outputSelections;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.OutputSelections;
         Description = AppConstants.Commands.UpdateOutputSelectionInSimulationDescription(buildingBlock.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateOutputSelectionInBuildingBlockCommand(_oldOutputSelections, _buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _outputSelections = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldOutputSelections = _buildingBlock.OutputSelections;
         _buildingBlock.OutputSelections = _outputSelections;
      }
   }
}