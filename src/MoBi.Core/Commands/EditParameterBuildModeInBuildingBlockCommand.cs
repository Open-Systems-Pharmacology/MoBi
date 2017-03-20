using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterBuildModeInBuildingBlockCommand : EditParameterPropertyInBuildingBlockCommand<ParameterBuildMode>
   {
      public EditParameterBuildModeInBuildingBlockCommand(ParameterBuildMode newBuildMode, IParameter parameter, IBuildingBlock buildingBlock)
         : base(newBuildMode, parameter.BuildMode, parameter, buildingBlock)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parameter.BuildMode = _newValue;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterBuildModeInBuildingBlockCommand(_oldValue, _parameter, _buildingBlock).AsInverseFor(this);
      }

      protected override string GetCommandDescription(ParameterBuildMode newBuildMode, ParameterBuildMode oldBuildMode, IParameter parameter, IBuildingBlock buildingBlock)
      {
         return AppConstants.Commands.UpdateParameterBuildMode(parameter.Name, newBuildMode.ToString(), oldBuildMode.ToString(), buildingBlock.Name);
      }
   }
}