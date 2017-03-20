using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class SetValuePointCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      protected ValuePoint _valuePoint;
      protected TableFormula _tableFormula;
      protected readonly double _newBaseValue;
      protected double _oldBaseValue;
      private readonly string _tableFormulaId;
      protected double _y;
      protected double _x;

      protected SetValuePointCommand(TableFormula tableFormula, ValuePoint valuePoint, double newBaseValue, IBuildingBlock buildingBlock)
         : base(buildingBlock)
      {
         _valuePoint = valuePoint;
         _tableFormula = tableFormula;
         _tableFormulaId = _tableFormula.Id;
         _newBaseValue = newBaseValue;

         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.ValuePoint;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var dimension = _tableFormula.Dimension;
         var oldY = getYInDisplayValueFromValuePoint(dimension, _valuePoint, _tableFormula.YDisplayUnit);
         var oldX = getXInDisplayValueFromValuePoint(dimension, _valuePoint, _tableFormula.XDisplayUnit);

         SetNewBaseValue(_valuePoint, _newBaseValue);
         context.PublishEvent(new TableFormulaValueChangedEvent(_tableFormula, _valuePoint));
         _x = _valuePoint.X;
         _y = _valuePoint.Y;

         var newY = getYInDisplayValueFromValuePoint(dimension, _valuePoint, _tableFormula.YDisplayUnit);
         var newX = getXInDisplayValueFromValuePoint(dimension, _valuePoint, _tableFormula.XDisplayUnit);
         Description = AppConstants.Commands.SetValuePointValueCommand(_tableFormula.Name, oldX, oldY, newX, newY, _buildingBlock.Name);
      }

      private string getXInDisplayValueFromValuePoint(IDimension dimension, ValuePoint valuePoint, Unit displayUnit)
      {
         return new UnitFormatter(displayUnit).Format(dimension.BaseUnitValueToUnitValue(displayUnit, valuePoint.X));
      }

      private string getYInDisplayValueFromValuePoint(IDimension dimension, ValuePoint valuePoint, Unit displayUnit)
      {
         return new UnitFormatter(displayUnit).Format(dimension.BaseUnitValueToUnitValue(displayUnit, valuePoint.Y));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _tableFormula = context.Get<TableFormula>(_tableFormulaId);
         _valuePoint = _tableFormula.GetPointWithCoordinates(_x, _y);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _valuePoint = null;
         _tableFormula = null;
      }

      protected abstract void SetNewBaseValue(ValuePoint valuePoint, double newBaseValue);
   }
}