using System;
using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.Services
{
   public interface IMoBiFormulaTask
   {
      bool EditNewFormula(IFormula formula, ICommandCollector command, IBuildingBlock buildingBlockWithFormulaCache, IParameter parameter);

      /// <summary>
      ///    Fixes up the formula references for an object which is going to be added to a building block.
      ///    if the same formula can be found already, the reference is modified on the object,
      ///    if not, the formula is added to the building block
      /// </summary>
      /// <param name="targetBuildingBlock">
      ///    The building block whose cache will be checked for existing equivalent formulas, or
      ///    added to if one is not found
      /// </param>
      /// <param name="usingFormulaObject">
      ///    The object with formula which needs to have it's reference modified, or it's formula
      ///    added to the cache
      /// </param>
      /// <returns>Any commands that were raised in the process</returns>
      IMoBiCommand AddFormulaToCacheOrFixReferenceCommand(IBuildingBlock targetBuildingBlock, IUsingFormula usingFormulaObject);

      /// <summary>
      ///    Fixes up the formula references for an object which is going to be added to a building block.
      ///    if the same formula can be found already, the reference is modified on the object,
      ///    if not, the formula is added to the building block
      /// </summary>
      /// <param name="targetBuildingBlock">
      ///    The building block whose cache will be checked for existing equivalent formulas, or
      ///    added to if one is not found
      /// </param>
      /// <param name="usingFormulaObject">
      ///    The object with formula which needs to have it's reference modified, or it's formula
      ///    added to the cache
      /// </param>
      /// <param name="decoder">Performs set action and get function on objects of type T</param>
      /// <returns>Any commands that were raised in the process</returns>
      IMoBiCommand AddFormulaToCacheOrFixReferenceCommand<T>(IBuildingBlock targetBuildingBlock, T usingFormulaObject, FormulaDecoder<T> decoder);

      /// <summary>
      ///    Gets a caption for the object using the formula
      /// </summary>
      /// <param name="usingObject">The object that is using the reaction</param>
      /// <param name="reactionMode">The reaction mode, either amount or concentration based</param>
      /// <param name="isRHS">indicates if the formula is right-hand-side</param>
      /// <returns>The caption. If a caption is not defined for the combination of parameters, then empty string is returned</returns>
      string GetFormulaCaption(IUsingFormula usingObject, ReactionDimensionMode reactionMode, bool isRHS);

      /// <summary>
      ///    Modifies the formula string for the attached formula
      /// </summary>
      /// <param name="formula">The formula being modified</param>
      /// <param name="newFormulaString">The new formula string</param>
     /// <param name="buildingBlock">The building block containing the formula</param>
      /// <returns></returns>
      IMoBiCommand SetFormulaString(FormulaWithFormulaString formula, string newFormulaString,  IBuildingBlock buildingBlock);

      /// <summary>
      ///    Changes the <paramref name="formulaUsablePath" /> in the <paramref name="formula" /> to <paramref name="newPath" />
      /// </summary>
      /// <returns>The command that was run to change the path</returns>
      IMoBiCommand ChangePathInFormula(IFormula formula, ObjectPath newPath, FormulaUsablePath formulaUsablePath, IBuildingBlock buildingBlock);

      IMoBiCommand AddFormulaUsablePath(IFormula formula, FormulaUsablePath path, IBuildingBlock buildingBlock);
      IMoBiCommand ChangeVariableName(SumFormula formula, string newVariableName,  IBuildingBlock buildingBlock);
      IMoBiCommand AddValuePoint(TableFormula formula, ValuePoint newValuePoint, IBuildingBlock buildingBlock);
      IMoBiCommand EditUseDerivedValues(TableFormula formula, bool newValue, bool oldValue, IBuildingBlock buildingBlock);
      IMoBiCommand RemoveValuePointFromTableFormula(TableFormula formula, ValuePoint valuePoint, IBuildingBlock buildingBlock);
      IMoBiCommand ChangeOffsetObject(TableFormulaWithOffset formula, FormulaUsablePath path, IBuildingBlock buildingBlock);
      IMoBiCommand ChangeTableObject(TableFormulaWithOffset formula, FormulaUsablePath path, IBuildingBlock buildingBlock);
      IMoBiCommand ChangeXArgumentObject(TableFormulaWithXArgument formula, FormulaUsablePath path, IBuildingBlock buildingBlock);
      IMoBiCommand ChangeTableObject(TableFormulaWithXArgument formula, FormulaUsablePath path, IBuildingBlock buildingBlock);
      IMoBiCommand SetConstantFormulaValue(ConstantFormula formula, double kernelValue, Unit newDisplayUnit, Unit oldDisplayUnit, IBuildingBlock buildingBlock, IEntity formulaOwner);

      IMoBiCommand UpdateFormula(IEntity usingFormula, IFormula oldFormula, IFormula newFormula, FormulaDecoder decoder, IBuildingBlock buildingBlock);

      (IMoBiCommand command, IFormula formula) CreateNewFormulaInBuildingBlock(Type formulaType, IDimension formulaDimension, IEnumerable<string> existingFormulaNames, IBuildingBlock buildingBlock);

      IFormula CreateNewFormula(Type formulaType, IDimension formulaDimension);
      TFormula CreateNewFormula<TFormula>(IDimension formulaDimension) where TFormula : IFormula;

      IMoBiCommand UpdateDistributedFormula(IDistributedParameter distributedParameter, IDistributionFormula newDistributedFormula, string formulaType, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Changes the <paramref name="oldAlias" /> to <paramref name="newAlias" /> in the <paramref name="formula" />. The
      ///    <paramref name="buildingBlock" />
      ///    is used to identify the location of the formula.
      /// </summary>
      /// <returns>The command that was run to change the alias</returns>
      IMoBiCommand EditAliasInFormula(IFormula formula, string newAlias, string oldAlias, FormulaUsablePath formulaUsablePath, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Sets the dimension for the formula usable path with <paramref name="alias" /> to <paramref name="newDimension" /> on
      ///    the <paramref name="formula" /> in the <paramref name="buildingBlock" />
      /// </summary>
      /// <returns>The command that was run to change the dimension</returns>
      IMoBiCommand SetFormulaPathDimension(IFormula formula, IDimension newDimension, string alias, IBuildingBlock buildingBlock);

      /// <summary>
      ///    Removes <paramref name="path" /> from the <paramref name="formula" />
      /// </summary>
      /// <returns>The command that was run to remove the path from the formula</returns>
      IMoBiCommand RemoveFormulaUsablePath(IFormula formula, FormulaUsablePath path, IBuildingBlock buildingBlock);

      (bool valid, string validationMessage) Validate(string formulaString, FormulaWithFormulaString formula, IBuildingBlock buildingBlock);
   }
}