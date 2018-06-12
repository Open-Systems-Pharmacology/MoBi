using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IStartValuesTask<TBuildingBlock, in TStartValue> : IInteractionTasksForBuildingBlock<TBuildingBlock>
      where TBuildingBlock : class, IStartValuesBuildingBlock<TStartValue>
      where TStartValue : class, IStartValue
   {
      void ExtendStartValues(TBuildingBlock startValuesBuildingBlock);
      TBuildingBlock CreateStartValuesForSimulation(IMoBiBuildConfiguration buildConfiguration);

      /// <summary>
      ///    Generates a command that will add the startValue to the building block
      /// </summary>
      /// <param name="buildingBlock">The building block being updated</param>
      /// <param name="startValue">The start value being added</param>
      /// <returns>A MoBiCommand that can be executed</returns>
      IMoBiCommand AddStartValueToBuildingBlock(TBuildingBlock buildingBlock, TStartValue startValue);

      /// <summary>
      ///    Generates a command that will add or update multiple parameter start values in the building block
      /// </summary>
      /// <param name="startValuesBuildingBlock">The building block being updated</param>
      /// <param name="startValues">The list of start values to be added or updated</param>
      /// <returns>The command used to update the building block</returns>
      IMoBiCommand ImportStartValuesToBuildingBlock(TBuildingBlock startValuesBuildingBlock, IEnumerable<ImportedQuantityDTO> startValues);

      /// <summary>
      ///    Sets the start value formula to a new value
      /// </summary>
      /// <param name="startValues">The building block which contains the start value</param>
      /// <param name="startValue">The start value being updated</param>
      /// <param name="formula">The new formula</param>
      /// <returns>The command used to execute the update</returns>
      IMoBiCommand ChangeStartValueFormulaCommand(TBuildingBlock startValues, TStartValue startValue, IFormula formula);

      /// <summary>
      ///    Modifies the value of the StartValue
      /// </summary>
      /// <param name="startValue">The start value being modified</param>
      /// <param name="newDisplayValue">The new display value of the start value</param>
      /// <param name="unit">The new unit of the start value</param>
      /// <param name="startValues">The start value building block that the start value is a member of</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand SetStartDisplayValueWithUnit(TStartValue startValue, double? newDisplayValue, Unit unit, TBuildingBlock startValues);

      /// <summary>
      ///    Returns a command that can be used to remove the start values contained in <paramref name="startValue"/> from <paramref name="buildingBlock"/>
      /// </summary>
      /// <returns>The command, which has not been run</returns>
      IMoBiCommand RemoveStartValueFromBuildingBlockCommand(TStartValue startValue, TBuildingBlock buildingBlock);

      /// <summary>
      ///    Renames a start value inside the list of start values
      /// </summary>
      /// <param name="startValues">The list of start values</param>
      /// <param name="startValue">The start value to be renamed</param>
      /// <param name="newValue">The new name to be used</param>
      /// <returns>The command that was run to rename the building block</returns>
      IMoBiCommand EditStartValueName(TBuildingBlock startValues, TStartValue startValue, string newValue);

      /// <summary>
      ///    Changes one element in the start value container path
      /// </summary>
      /// <param name="buildingBlock">The building block containing the start value</param>
      /// <param name="startValue">The start value that will be updated</param>
      /// <param name="indexToUpdate">The index of the value being modified</param>
      /// <param name="newValue">The new value for the path</param>
      /// <returns>The command that was run to edit the container path</returns>
      IMoBiCommand EditStartValueContainerPath(TBuildingBlock buildingBlock, TStartValue startValue, int indexToUpdate, string newValue);

      /// <summary>
      ///    Refreshes start values from original building blocks
      /// </summary>
      /// <param name="buildingBlock">The StartValueBuildingBlock being updated</param>
      /// <param name="startValuesToRefresh">The list of values that should be refreshed</param>
      /// <returns>The command that was run to refresh the values</returns>
      IMoBiCommand RefreshStartValuesFromBuildingBlocks(TBuildingBlock buildingBlock, IEnumerable<TStartValue> startValuesToRefresh);

      /// <summary>
      ///    Retrieves all the parent container path elements from the molecule building block and spatial structure
      /// </summary>
      /// <param name="buildingBlock">The start value building block</param>
      /// <returns>A list of possible valid container path elements</returns>
      IEnumerable<string> GetContainerPathItemsForBuildingBlock(TBuildingBlock buildingBlock);

      /// <summary>
      ///    Determines whether the start value contains equivalent information to the original builder that it is based on
      /// </summary>
      /// <param name="startValue">The start value being evaluated</param>
      /// <param name="buildingBlock"></param>
      /// <returns>True of the original builder values match the values in the startValue</returns>
      bool IsEquivalentToOriginal(TStartValue startValue, TBuildingBlock buildingBlock);

      /// <summary>
      ///    Updates a dimension for a parameter start value
      /// </summary>
      /// <param name="parameterStartValuesBuildingBlock">The building block containing the start value being updated</param>
      /// <param name="startValue">The start value being updated</param>
      /// <param name="newDimension">The new dimension for the start value</param>
      /// <returns>The command used to update the start value dimension</returns>
      IMoBiCommand UpdateStartValueDimension(TBuildingBlock parameterStartValuesBuildingBlock, TStartValue startValue, IDimension newDimension);

      /// <summary>
      /// Returns the default dimension for the start value type
      /// </summary>
      /// <returns>A dimension which can be used as default when creating empty start values</returns>
      IDimension GetDefaultDimension();

      /// <summary>
      /// Sets the value of a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the start value</param>
      /// <param name="valueInDisplayUnit">The new value in display units</param>
      /// <param name="startValue">The start value being modified</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand SetValue(TBuildingBlock buildingBlock, double? valueInDisplayUnit, TStartValue startValue);

      /// <summary>
      /// Sets the value of a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the start value</param>
      /// <param name="valueOrigin">The new value origin</param>
      /// <param name="startValue">The start value being modified</param>
      /// <returns>The command used to modify the start value</returns>
      ICommand SetValueOrigin(TBuildingBlock buildingBlock, ValueOrigin valueOrigin, TStartValue startValue);

      /// <summary>
      /// Sets the display unit of a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the start value</param>
      /// <param name="startValue">The start value being modified</param>
      /// <param name="newUnit">The new display unit</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand SetUnit(TBuildingBlock buildingBlock, TStartValue startValue, Unit newUnit);

      /// <summary>
      /// Sets the formula for a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the start value</param>
      /// <param name="startValue">The start value being modified</param>
      /// <param name="formula">The new formula for the start value</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand SetFormula(TBuildingBlock buildingBlock, TStartValue startValue, IFormula formula);

      /// <summary>
      /// Determines whether the source for the building block can be resolved
      /// </summary>
      /// <param name="buildingBlock">The building block containing the start value</param>
      /// <param name="startValue">The start value being resolved</param>
      /// <returns>true if the start value can be resolved, otherwise false</returns>
      bool CanResolve(TBuildingBlock buildingBlock, TStartValue startValue);
   }
}