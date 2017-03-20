using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class SetRestartSolverInValuePointCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly string _tableFormulaId;
      private TableFormula _tableFormula;
      private ValuePoint _valuePoint;
      private readonly bool _newRestartSolverValue;
      private readonly bool _oldRestartSolverValue;
      private readonly double _y;
      private readonly double _x;

      public SetRestartSolverInValuePointCommand(TableFormula tableFormula, ValuePoint valuePoint, bool newRestartSolverValue, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _tableFormulaId = tableFormula.Id;
         _tableFormula = tableFormula;
         _valuePoint = valuePoint;
         _newRestartSolverValue = newRestartSolverValue;
         _oldRestartSolverValue = _valuePoint.RestartSolver;
         _x = _valuePoint.X;
         _y = _valuePoint.Y;
         var unitFormatter = new UnitFormatter();
         CommandType = AppConstants.Commands.EditCommand;
         var dimension = tableFormula.XDimension;
         var xCoordinate = dimension.BaseUnitValueToUnitValue(tableFormula.XDisplayUnit, valuePoint.X);
         dimension = tableFormula.Dimension;
         var yCoordinate = dimension.BaseUnitValueToUnitValue(tableFormula.YDisplayUnit, valuePoint.Y);

         Description = AppConstants.Commands.SetRestartSolverInValuePoint(_tableFormula.Name, 
            unitFormatter.Format(xCoordinate, tableFormula.XDisplayUnit),
            unitFormatter.Format(yCoordinate, tableFormula.YDisplayUnit), 
            newRestartSolverValue, _oldRestartSolverValue, buildingBlock.Name);
         ObjectType = ObjectTypes.ValuePoint;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _valuePoint.RestartSolver = _newRestartSolverValue;
         context.PublishEvent(new TableFormulaRestartSolverChangedEvent(_tableFormula, _valuePoint));
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _tableFormula = null;
         _valuePoint = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _tableFormula = context.Get<TableFormula>(_tableFormulaId);
         _valuePoint = _tableFormula.GetPointWithCoordinates(_x, _y);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetRestartSolverInValuePointCommand(_tableFormula, _valuePoint, _oldRestartSolverValue,  _buildingBlock).AsInverseFor(this);
      }
   }
}