using MoBi.Core.Commands;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks
{
   public interface ITableFormulaTask
   {
      /// <summary>
      /// Returns a command that can be run to set the X axis display unit of <paramref name="tableFormula"/>to <paramref name="newUnit"/>
      /// </summary>
      /// <returns>The un-run command</returns>
      IMoBiCommand SetXUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock);

      /// <summary>
      /// Returns a command that can be run to set the Y axis display unit of <paramref name="tableFormula"/>to <paramref name="newUnit"/>
      /// </summary>
      /// <returns>The un-run command</returns>
      IMoBiCommand SetYUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock);

      IMoBiCommand SetXValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock);
      IMoBiCommand SetYValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock);
      IMoBiCommand SetRestartSolver(TableFormula formula, ValuePoint valuePoint, bool newRestartSolverValue, IBuildingBlock buildingBlock);
   }

   public class TableFormulaTask : ITableFormulaTask
   {
      private IMoBiCommand setXValuePoint(ValuePoint valuePoint, TableFormula tableFormula,
         double newValueInBaseUnit,IBuildingBlock buildingBlock)
      {
         return new SetValuePointXValueCommand(tableFormula, valuePoint, newValueInBaseUnit, buildingBlock);
      }

      private IMoBiCommand setYValuePoint(ValuePoint valuePoint, TableFormula tableFormula,
          double newValueInBaseUnit, IBuildingBlock buildingBlock)
      {
         return new SetValuePointYValueCommand(tableFormula, valuePoint, newValueInBaseUnit, buildingBlock);
      }

      public IMoBiCommand SetXUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock)
      {
         return new SetTableFormulaXDisplayUnitCommand(newUnit, tableFormula.XDisplayUnit, tableFormula, buildingBlock);
      }

      public IMoBiCommand SetXValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock)
      {
         var newValueInBaseUnit = tableFormula.XBaseValueFor(newValueInDisplayUnit);
         return setXValuePoint(valuePoint, tableFormula, newValueInBaseUnit, buildingBlock);
      }

      public IMoBiCommand SetYValuePoint(TableFormula tableFormula, ValuePoint valuePoint, double newValueInDisplayUnit, IBuildingBlock buildingBlock)
      {
         var newValueInBaseUnit = tableFormula.YBaseValueFor(newValueInDisplayUnit);
         return setYValuePoint(valuePoint, tableFormula, newValueInBaseUnit, buildingBlock);
      }

      public IMoBiCommand SetYUnit(TableFormula tableFormula, Unit newUnit, IBuildingBlock buildingBlock)
      {
         return new SetTableFormulaYDisplayUnitCommand(newUnit, tableFormula.YDisplayUnit, tableFormula, buildingBlock);
      }

      public IMoBiCommand SetRestartSolver(TableFormula formula, ValuePoint valuePoint, bool newRestartSolverValue, IBuildingBlock buildingBlock)
      {
         return new SetRestartSolverInValuePointCommand(formula, valuePoint, newRestartSolverValue, buildingBlock);
      }
   }
}