using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class AddValuePointCommand : AddItemCommand<ValuePoint, TableFormula, IBuildingBlock>
   {
      private ValuePoint _valuePoint;

      public AddValuePointCommand(TableFormula tableFormula, ValuePoint valuePoint, IBuildingBlock changedBuildingBlock) : base(tableFormula, valuePoint, changedBuildingBlock)
      {
         _valuePoint = valuePoint;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveValuePointFromTableFormulaCommand(_parent, _valuePoint, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parent.AddPoint(_valuePoint);
         double xValueInDisplayUnit = _parent.XDisplayValueFor(_valuePoint.X);
         double yValueInDisplayUnit = _parent.YDisplayValueFor(_valuePoint.Y);
         Description = AppConstants.Commands.AddValuePointDescription(xValueInDisplayUnit, yValueInDisplayUnit, _parent.Name, _parent.XDisplayUnit, _parent.YDisplayUnit);
         context.PublishEvent(new AddedValuePointEvent(_parent, _valuePoint));
      }
   }

   public class RemoveValuePointFromTableFormulaCommand : RemoveItemCommand<ValuePoint, TableFormula, IBuildingBlock>
   {
      public RemoveValuePointFromTableFormulaCommand(TableFormula parent, ValuePoint itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddValuePointCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parent.RemovePoint(_itemToRemove);
         double xValueInDisplayUnit = _parent.XDisplayValueFor(_itemToRemove.X);
         double yValueInDisplayUnit = _parent.YDisplayValueFor(_itemToRemove.Y);

         Description = AppConstants.Commands.RemoveValuePointDescription(xValueInDisplayUnit, yValueInDisplayUnit, _parent.Name, _parent.XDisplayUnit, _parent.YDisplayUnit);
        context.PublishEvent(new RemovedValuePointEvent(_parent, _itemToRemove));
      }

    
   }
}