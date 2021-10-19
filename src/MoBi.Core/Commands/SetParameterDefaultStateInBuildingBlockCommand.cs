using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SetParameterDefaultStateInBuildingBlockCommand : EditQuantityInBuildingBlockCommand<IParameter>
   {
      private readonly bool _isDefault;
      private bool _oldIsDefault;

      public SetParameterDefaultStateInBuildingBlockCommand(IParameter parameter, bool isDefault, IBuildingBlock buildingBlock) : base(parameter, buildingBlock)
      {
         _isDefault = isDefault;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldIsDefault = _quantity.IsDefault;
         _quantity.IsDefault = _isDefault;
         Description = AppConstants.Commands.SetParameterDefaultStateInBuildingBlock(_quantity.EntityPath(), _oldIsDefault, _isDefault, _buildingBlock.Name);

      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetParameterDefaultStateInBuildingBlockCommand(_quantity, _oldIsDefault, _buildingBlock).AsInverseFor(this);
      }
   }
}