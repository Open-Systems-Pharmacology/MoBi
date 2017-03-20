using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class SetTableFormulaDisplayUnitCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      protected readonly Unit _newDisplayUnit;
      protected readonly Unit _oldDisplayUnit;
      protected TableFormula _tableFormula;
      private readonly string _tableFormulaId;

      protected SetTableFormulaDisplayUnitCommand(Unit newDisplayUnit, Unit oldDisplayUnit, TableFormula tableFormula, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _newDisplayUnit = newDisplayUnit;
         _oldDisplayUnit = oldDisplayUnit;
         _tableFormula = tableFormula;
         _tableFormulaId = _tableFormula.Id;

         ObjectType = new ObjectTypeResolver().TypeFor(tableFormula);
         CommandType = AppConstants.Commands.EditCommand;
         
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         
         SetUnitAndPoints();

         context.PublishEvent(new TableFormulaUnitChangedEvent(_tableFormula));
      }

      protected abstract void SetUnitAndPoints();

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _tableFormula = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _tableFormula = context.Get<TableFormula>(_tableFormulaId);
      }

      protected double ConvertToNewUnit(IDimension dimension, double valueInBaseUnit)
      {
         var valueInDisplayUnit = dimension.BaseUnitValueToUnitValue(_oldDisplayUnit, valueInBaseUnit);
         return dimension.UnitValueToBaseUnitValue(_newDisplayUnit, valueInDisplayUnit);
      }
   }
}