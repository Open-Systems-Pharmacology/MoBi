using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterDescriptionInBuildingBlockComand : EditParameterPropertyInBuildingBlockCommand<string>
   {
      public EditParameterDescriptionInBuildingBlockComand(string newDescription, string oldDescription, IParameter parameter, IBuildingBlock buildingBlock)
         : base(newDescription, oldDescription, parameter, buildingBlock)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parameter.Description = _newValue;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterDescriptionInBuildingBlockComand(_oldValue, _newValue, _parameter, _buildingBlock).AsInverseFor(this);
      }

      protected override string GetCommandDescription(string newDescription, string oldDescription, IParameter parameter, IBuildingBlock buildingBlock)
      {
         return AppConstants.Commands.ChangeParameterDescription(parameter.Name, oldDescription, newDescription, buildingBlock.Name);
      }
   }
}