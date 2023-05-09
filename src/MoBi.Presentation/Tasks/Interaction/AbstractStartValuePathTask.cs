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
   public interface IStartValuePathTask<TBuildingBlock, TPathAndValueEntity> where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity>, IBuildingBlock where TPathAndValueEntity : PathAndValueEntity
   {
      /// <summary>
      ///    Updates the name of the <paramref name="pathAndValueEntity" /> to <paramref name="newName" /> and returns the executed
      ///    command
      /// </summary>
      /// <returns>The executed command corresponding to the rename of the start value</returns>
      IMoBiCommand UpdateName(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, string newName);

      /// <summary>
      ///    Updates the entry at <paramref name="indexToUpdate" /> of the <paramref name="pathAndValueEntity" /> path to
      ///    <paramref name="newValue" /> and returns the executed command
      /// </summary>
      /// <returns>The executed command corresponding to the path update at <paramref name="indexToUpdate" /> of the start value</returns>
      IMoBiCommand UpdateContainerPath(TBuildingBlock startValues, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Creates a command to update the name of the <paramref name="pathAndValueEntity" /> to <paramref name="newName" /> and
      ///    returns the non executed command
      /// </summary>
      /// <returns>The command corresponding to the rename of the start value. The command was not run yet</returns>
      IMoBiCommand UpdateNameCommand(TBuildingBlock startValues, TPathAndValueEntity pathAndValueEntity, string newName);

      /// <summary>
      ///    Creates a command corresponding to the update of the path entry at <paramref name="indexToUpdate" /> of the
      ///    <paramref name="pathAndValueEntity" /> to <paramref name="newValue" /> and returns the non executed command
      /// </summary>
      /// <returns>
      ///    The command corresponding to the path update at <paramref name="indexToUpdate" /> of the start value. The
      ///    command was not run yet
      /// </returns>
      IMoBiCommand UpdateContainerPathCommand(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Checks that the formula is equivalent for the start value. This includes evaluation of constant formula to a double
      /// </summary>
      /// <param name="pathAndValueEntity">The start value to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is equivalent to the start value formula</returns>
      bool HasEquivalentFormula(PathAndValueEntity pathAndValueEntity, IFormula targetFormula);
   }

   public abstract class AbstractStartValuePathTask<TBuildingBlock, TPathAndValueEntity> : IStartValuePathTask<TBuildingBlock, TPathAndValueEntity> where TBuildingBlock : PathAndValueEntityBuildingBlock<TPathAndValueEntity> where TPathAndValueEntity : PathAndValueEntity
   {
      private readonly IFormulaTask _formulaTask;
      private readonly IMoBiContext _context;

      protected AbstractStartValuePathTask(IFormulaTask formulaTask, IMoBiContext context)
      {
         _formulaTask = formulaTask;
         _context = context;
      }

      public IMoBiCommand UpdateName(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, string newValue)
      {
         return UpdateNameCommand(buildingBlock, pathAndValueEntity, newValue).Run(_context);
      }

      public IMoBiCommand UpdateContainerPath(TBuildingBlock startValues, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue)
      {
         return UpdateContainerPathCommand(startValues, pathAndValueEntity, indexToUpdate, newValue).Run(_context);
      }

      public abstract IMoBiCommand UpdateNameCommand(TBuildingBlock startValues, TPathAndValueEntity pathAndValueEntity, string newName);
      public abstract IMoBiCommand UpdateContainerPathCommand(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue);

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
      /// <param name="pathAndValueEntity">The path and value entity to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is equivalent to the path and value entity formula</returns>
      public bool HasEquivalentFormula(PathAndValueEntity pathAndValueEntity, IFormula targetFormula)
      {
         var startValueFormula = pathAndValueEntity.Formula;
         if (startValueFormula == null && targetFormula == null)
            return true;

         if ((startValueFormula == null || startValueFormula.IsConstant()) && targetFormula.IsConstant())
            return isConstantFormulaEqualToStartValue(pathAndValueEntity, targetFormula.DowncastTo<ConstantFormula>());

         return _formulaTask.FormulasAreTheSame(pathAndValueEntity.Formula, targetFormula);
      }

      /// <summary>
      ///    Checks to see if the constant formula equals the double value of <paramref name="pathAndValueEntity"/>
      /// </summary>
      /// <param name="pathAndValueEntity">The path and value entity to check</param>
      /// <param name="targetFormula">The formula being evaluated</param>
      /// <returns>True if the formula is constant and evaluates to the same value as pathAndValueEntity.Value</returns>
      private static bool isConstantFormulaEqualToStartValue(PathAndValueEntity pathAndValueEntity, ConstantFormula targetFormula)
      {
         return pathAndValueEntity.Value.HasValue && ValueComparer.AreValuesEqual(pathAndValueEntity.Value.Value, targetFormula.Calculate(null));
      }
   }
}