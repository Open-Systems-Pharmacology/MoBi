using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class SetValuePointYValueCommand : SetValuePointCommand
   {
      public SetValuePointYValueCommand(TableFormula tableFormula, ValuePoint valuePoint, double newBaseValue, IBuildingBlock buildingBlock) : base(tableFormula, valuePoint, newBaseValue, buildingBlock)
      {
         _oldBaseValue = valuePoint.Y;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetValuePointYValueCommand(_tableFormula, _valuePoint, _oldBaseValue, _buildingBlock).AsInverseFor(this);
      }

      protected override void SetNewBaseValue(ValuePoint valuePoint, double newBaseValue)
      {
         valuePoint.Y = newBaseValue;
      }
   }
}