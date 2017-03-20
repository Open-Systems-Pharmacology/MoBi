using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetQuantityUnitInBuildingBlockCommand : SetQuantityValueInBuildingBlockCommand
   {
      private Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;
      private string _oldDisplayUnitName;

      public SetQuantityUnitInBuildingBlockCommand(IQuantity quantity, Unit newDisplayUnit, IBuildingBlock buildingBlock)
         : base(quantity, buildingBlock)
      {
         _newDisplayUnit = newDisplayUnit;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _oldDisplayUnit = _quantity.DisplayUnit;
         _oldDisplayUnitName = _oldDisplayUnit.Name;
         double oldDisplayValue = _quantity.Dimension.BaseUnitValueToUnitValue(_oldDisplayUnit, _quantity.Value);
         _valueToSet = _quantity.Dimension.UnitValueToBaseUnitValue(_newDisplayUnit, oldDisplayValue);
         _quantity.DisplayUnit = _newDisplayUnit;

         //this needs to be done after the display unit was set
         base.ExecuteWith(context);
       
         Description = AppConstants.Commands.SetQuantityValueInBuildingBlock(ObjectType, _quantity.ValueInDisplayUnit, _newDisplayUnit.Name, oldDisplayValue, _oldDisplayUnitName, _quantity.EntityPath(), _buildingBlock.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetQuantityUnitInBuildingBlockCommand(_quantity, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _oldDisplayUnit = _quantity.Dimension.Unit(_oldDisplayUnitName);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newDisplayUnit = null;
         _oldDisplayUnit = null;
      }
   }
}