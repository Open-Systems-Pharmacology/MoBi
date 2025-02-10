using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetTableFormulaYDisplayUnitCommand : SetTableFormulaDisplayUnitCommand
   {
      public SetTableFormulaYDisplayUnitCommand(Unit newDisplayUnit, Unit oldDisplayUnit, TableFormula tableFormula, IBuildingBlock buildingBlock)
         : base(newDisplayUnit, oldDisplayUnit, tableFormula, buildingBlock)
      {
         Description = AppConstants.Commands.SetTableFormulaYDisplayUnits(tableFormula.Name, oldDisplayUnit.ToString(), newDisplayUnit.ToString(), buildingBlock.Name);
      }

      protected override void SetUnitAndPoints()
      {
         _tableFormula.YDisplayUnit = _newDisplayUnit;
         foreach (var point in _tableFormula.AllPoints)
         {
            point.Y = ConvertToNewUnit(_tableFormula.Dimension, point.Y);
         }
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetTableFormulaYDisplayUnitCommand(_oldDisplayUnit, _newDisplayUnit, _tableFormula, _buildingBlock).AsInverseFor(this);
      }
   }
}