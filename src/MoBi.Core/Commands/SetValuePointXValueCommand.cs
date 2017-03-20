using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class SetValuePointXValueCommand : SetValuePointCommand
   {
      public SetValuePointXValueCommand(TableFormula tableFormula, ValuePoint valuePoint, double newBaseValue, IBuildingBlock buildingBlock)
         : base(tableFormula, valuePoint, newBaseValue, buildingBlock)
      {
         _oldBaseValue = valuePoint.X;
      }

      protected override void SetNewBaseValue(ValuePoint valuePoint, double newBaseValue)
      {
         valuePoint.X = newBaseValue;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetValuePointXValueCommand(_tableFormula, _valuePoint, _oldBaseValue, _buildingBlock).AsInverseFor(this);
      }
   }
}