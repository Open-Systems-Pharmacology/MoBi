using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class SetQuantityValueInBuildingBlockCommand : EditQuantityInBuildingBlockCommand<IQuantity>
   {
      protected double _valueToSet;
      private double _oldValue;

      public SetQuantityValueInBuildingBlockCommand(IQuantity quantity, double valueToSet, IBuildingBlock buildingBlock)
         : base(quantity, buildingBlock)
      {
         _valueToSet = valueToSet;
      }

      protected SetQuantityValueInBuildingBlockCommand(IQuantity quantity, IBuildingBlock buildingBlock): base(quantity, buildingBlock)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldValue = _quantity.Value;
         _quantity.UpdateQuantityValue(_valueToSet);
         var newDisplayValue = _quantity.ValueInDisplayUnit;
         var oldDisplayValue = _quantity.ConvertToDisplayUnit(_oldValue);
         var unitName = _quantity.DisplayUnit.Name;
         Description = AppConstants.Commands.SetQuantityValueInBuildingBlock(ObjectType, newDisplayValue, unitName, oldDisplayValue, unitName, _quantity.EntityPath(), _buildingBlock.Name);
         context.PublishEvent(new QuantityValueChangedEvent(_quantity));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetQuantityValueInBuildingBlockCommand(_quantity, _oldValue, _buildingBlock).AsInverseFor(this);
      }
   }
}