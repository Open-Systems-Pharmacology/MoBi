using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class ResetQuantityValueInBuildingBlockCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IQuantity _quantity;
      private readonly double _oldQuantityValue;
      private readonly string _quantityId;

      public ResetQuantityValueInBuildingBlockCommand(IQuantity quantity, IBuildingBlock buildingBlock) :
         base(buildingBlock)
      {
         _quantity = quantity;
         _quantityId = _quantity.Id;
         _oldQuantityValue = quantity.Value;
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = new ObjectTypeResolver().TypeFor(quantity);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var unitName = _quantity.DisplayUnit.Name;
         var oldDisplayValue = _quantity.ConvertToDisplayUnit(_oldQuantityValue);

         base.ExecuteWith(context);
         _quantity.IsFixedValue = false;

         var newDisplayValue = _quantity.ValueInDisplayUnit;
         Description = AppConstants.Commands.SetQuantityValueInBuildingBlock(ObjectType, newDisplayValue, unitName, oldDisplayValue, unitName, _quantity.EntityPath(), _buildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _quantity = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _quantity = context.Get<IQuantity>(_quantityId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetQuantityValueInBuildingBlockCommand(_quantity, _oldQuantityValue, _buildingBlock).AsInverseFor(this);
      }
   }
}