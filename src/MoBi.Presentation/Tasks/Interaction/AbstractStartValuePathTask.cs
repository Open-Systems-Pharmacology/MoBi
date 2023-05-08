using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IStartValuePathTask<TBuildingBlock, TStartValue> where TBuildingBlock : PathAndValueEntityBuildingBlock<TStartValue>, IBuildingBlock where TStartValue : PathAndValueEntity
   {
      /// <summary>
      ///    Updates the name of the <paramref name="startValue" /> to <paramref name="newName" /> and returns the executed
      ///    command
      /// </summary>
      /// <returns>The executed command corresponding to the rename of the start value</returns>
      IMoBiCommand UpdateStartValueName(TBuildingBlock startValues, TStartValue startValue, string newName);

      /// <summary>
      ///    Updates the entry at <paramref name="indexToUpdate" /> of the <paramref name="startValue" /> path to
      ///    <paramref name="newValue" /> and returns the executed command
      /// </summary>
      /// <returns>The executed command corresponding to the path update at <paramref name="indexToUpdate" /> of the start value</returns>
      IMoBiCommand UpdateStartValueContainerPath(TBuildingBlock startValues, TStartValue startValue, int indexToUpdate, string newValue);

      /// <summary>
      ///    Creates a command to update the name of the <paramref name="startValue" /> to <paramref name="newName" /> and
      ///    returns the non executed command
      /// </summary>
      /// <returns>The command corresponding to the rename of the start value. The command was not run yet</returns>
      IMoBiCommand UpdateStartValueNameCommand(TBuildingBlock startValues, TStartValue startValue, string newName);

      /// <summary>
      ///    Creates a command correspondign to the update of the path entry at <paramref name="indexToUpdate" /> of the
      ///    <paramref name="startValue" /> to <paramref name="newValue" /> and returns the non executed command
      /// </summary>
      /// <returns>
      ///    The command corresponding to the path update at <paramref name="indexToUpdate" /> of the start value. The
      ///    command was not run yet
      /// </returns>
      IMoBiCommand UpdateStartValueContainerPathCommand(TBuildingBlock buildingBlock, TStartValue startValue, int indexToUpdate, string newValue);

      /// <summary>
      ///    Checks that the formula is equivalent for the start value. This includes evaluation of constant formula to a double
      /// </summary>
      /// <param name="startValue">The start value to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is equivalent to the start value formula</returns>
      bool HasEquivalentFormula(PathAndValueEntity startValue, IFormula targetFormula);
   }

   public abstract class AbstractStartValuePathTask<TBuildingBlock, TStartValue> : IStartValuePathTask<TBuildingBlock, TStartValue> where TBuildingBlock : PathAndValueEntityBuildingBlock<TStartValue> where TStartValue : PathAndValueEntity
   {
      private readonly IFormulaTask _formulaTask;
      private readonly IMoBiContext _context;

      protected AbstractStartValuePathTask(IFormulaTask formulaTask, IMoBiContext context)
      {
         _formulaTask = formulaTask;
         _context = context;
      }

      public IMoBiCommand UpdateStartValueName(TBuildingBlock startValues, TStartValue startValue, string newValue)
      {
         return UpdateStartValueNameCommand(startValues, startValue, newValue).Run(_context);
      }

      public IMoBiCommand UpdateStartValueContainerPath(TBuildingBlock startValues, TStartValue startValue, int indexToUpdate, string newValue)
      {
         return UpdateStartValueContainerPathCommand(startValues, startValue, indexToUpdate, newValue).Run(_context);
      }

      public abstract IMoBiCommand UpdateStartValueNameCommand(TBuildingBlock startValues, TStartValue startValue, string newName);
      public abstract IMoBiCommand UpdateStartValueContainerPathCommand(TBuildingBlock buildingBlock, TStartValue startValue, int indexToUpdate, string newValue);

      public static void ConfigureTargetPath(int indexToUpdate, string newValue, ObjectPath targetPath)
      {
         if (targetPath.Count == indexToUpdate)
            targetPath.Add(newValue);

         else if (string.IsNullOrEmpty(newValue))
            targetPath.RemoveAt(indexToUpdate);

         else
            targetPath[indexToUpdate] = newValue;
      }

      /// <summary>
      ///    Checks that the formula is equivalent for the start value. This includes evaluation of constant formula to a double
      /// </summary>
      /// <param name="startValue">The start value to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is equivalent to the start value formula</returns>
      public bool HasEquivalentFormula(PathAndValueEntity startValue, IFormula targetFormula)
      {
         var startValueFormula = startValue.Formula;
         if (startValueFormula == null && targetFormula == null)
            return true;

         if ((startValueFormula == null || startValueFormula.IsConstant()) && targetFormula.IsConstant())
            return isConstantFormulaEqualToStartValue(startValue, targetFormula.DowncastTo<ConstantFormula>());

         return _formulaTask.FormulasAreTheSame(startValue.Formula, targetFormula);
      }

      /// <summary>
      ///    Checks to see if the constant formula equals the double StartValue
      /// </summary>
      /// <param name="startValue">The start value to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is constant and evaluates to the same value as startValue.StartValue</returns>
      private static bool isConstantFormulaEqualToStartValue(PathAndValueEntity startValue, ConstantFormula targetFormula)
      {
         return startValue.Value.HasValue && ValueComparer.AreValuesEqual(startValue.Value.Value, targetFormula.Calculate(null));
      }
   }
}