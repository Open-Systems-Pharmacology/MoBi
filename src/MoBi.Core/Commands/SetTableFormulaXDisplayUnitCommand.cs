using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetTableFormulaXDisplayUnitCommand : SetTableFormulaDisplayUnitCommand
   {
      public SetTableFormulaXDisplayUnitCommand(Unit newDisplayUnit, Unit oldDisplayUnit, TableFormula tableFormula, IBuildingBlock buildingBlock)
         : base(newDisplayUnit, oldDisplayUnit, tableFormula, buildingBlock)
      {
         Description = AppConstants.Commands.SetTableFormulaXDisplayUnits(tableFormula.Name, oldDisplayUnit.ToString(), newDisplayUnit.ToString(), buildingBlock.Name);
      }

      protected override void SetUnitAndPoints()
      {
         _tableFormula.XDisplayUnit = _newDisplayUnit;
         foreach (var point in _tableFormula.AllPoints)
         {
            point.X = ConvertToNewUnit(_tableFormula.XDimension, point.X);
         }
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetTableFormulaXDisplayUnitCommand(_oldDisplayUnit, _newDisplayUnit, _tableFormula, _buildingBlock).AsInverseFor(this);
      }
   }
}