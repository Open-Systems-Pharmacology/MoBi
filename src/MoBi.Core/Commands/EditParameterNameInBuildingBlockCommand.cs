using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditParameterNameInBuildingBlockCommand : EditParameterPropertyInBuildingBlockCommand<string>
   {
      public EditParameterNameInBuildingBlockCommand(string newName, string oldName, IParameter parameter, IBuildingBlock buildingBlock) 
         : base(newName, oldName, parameter, buildingBlock)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parameter.Name = _newValue;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditParameterNameInBuildingBlockCommand(_oldValue, _newValue, _parameter, _buildingBlock).AsInverseFor(this);
      }

      protected override string GetCommandDescription(string newName, string oldName, IParameter parameter, IBuildingBlock buildingBlock)
      {
         return AppConstants.Commands.ChangeParameterName(parameter.Name, oldName, newName, buildingBlock.Name);
      }
   }
}