using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IStartValuesTask<TBuildingBlock, in TPathAndValueEntity> : IInteractionTasksForBuildingBlock<Module, TBuildingBlock>, IInteractionTasksForPathAndValueEntity<Module, TBuildingBlock, TPathAndValueEntity>
      where TBuildingBlock : class, IBuildingBlock<TPathAndValueEntity>
      where TPathAndValueEntity : PathAndValueEntity
   {
      void ExtendStartValueBuildingBlock(TBuildingBlock initialConditionsBuildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock);

      /// <summary>
      ///    Generates a command that will add the pathAndValueEntity to the building block
      /// </summary>
      /// <param name="buildingBlock">The building block being updated</param>
      /// <param name="pathAndValueEntity">The start value being added</param>
      /// <returns>A MoBiCommand that can be executed</returns>
      IMoBiCommand AddPathAndValueEntityToBuildingBlock(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity);

      /// <summary>
      ///    Generates a command that will add or update multiple parameter start values in the building block
      /// </summary>
      /// <param name="buildingBlock">The building block being updated</param>
      /// <param name="startQuantities">The list of start values to be added or updated</param>
      /// <returns>The command used to update the building block</returns>
      IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(TBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities);

      /// <summary>
      ///    Sets the start value formula to a new value
      /// </summary>
      /// <param name="buildingBlock">The building block which contains the start value</param>
      /// <param name="pathAndValueEntity">The start value being updated</param>
      /// <param name="formula">The new formula</param>
      /// <returns>The command used to execute the update</returns>
      IMoBiCommand ChangeValueFormulaCommand(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, IFormula formula);

      /// <summary>
      ///    Modifies the value of the StartValue
      /// </summary>
      /// <param name="pathAndValueEntity">The path and value entity being modified</param>
      /// <param name="newDisplayValue">The new display value of the start value</param>
      /// <param name="unit">The new unit of the start value</param>
      /// <param name="buildingBlock">The building block that the entity is a member of</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand SetDisplayValueWithUnit(TPathAndValueEntity pathAndValueEntity, double? newDisplayValue, Unit unit, TBuildingBlock buildingBlock);

      /// <summary>
      ///    Returns a command that can be used to remove the start values contained in <paramref name="pathAndValueEntity" />
      ///    from
      ///    <paramref name="buildingBlock" />
      /// </summary>
      /// <returns>The command, which has not been run</returns>
      IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(TPathAndValueEntity pathAndValueEntity, TBuildingBlock buildingBlock);

      /// <summary>
      ///    Renames a start value inside the list of start values
      /// </summary>
      /// <param name="buildingBlock">The list of start values</param>
      /// <param name="pathAndValueEntity">The start value to be renamed</param>
      /// <param name="newValue">The new name to be used</param>
      /// <returns>The command that was run to rename the building block</returns>
      IMoBiCommand EditPathAndValueEntityName(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, string newValue);

      /// <summary>
      ///    Changes one element in the start value container path
      /// </summary>
      /// <param name="buildingBlock">The building block containing the start value</param>
      /// <param name="pathAndValueEntity">The start value that will be updated</param>
      /// <param name="indexToUpdate">The index of the value being modified</param>
      /// <param name="newValue">The new value for the path</param>
      /// <returns>The command that was run to edit the container path</returns>
      IMoBiCommand EditPathAndValueEntityContainerPath(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Refreshes start values from original building blocks
      /// </summary>
      /// <param name="buildingBlock">The StartValueBuildingBlock being updated</param>
      /// <param name="pathAndValueEntitiesToRefresh">The list of values that should be refreshed</param>
      /// <returns>The command that was run to refresh the values</returns>
      IMoBiCommand RefreshPathAndValueEntitiesFromBuildingBlocks(TBuildingBlock buildingBlock, IEnumerable<TPathAndValueEntity> pathAndValueEntitiesToRefresh);

      /// <summary>
      ///    Retrieves all the parent container path elements from the molecule building block and spatial structure
      /// </summary>
      /// <param name="buildingBlock">The start value building block</param>
      /// <returns>A list of possible valid container path elements</returns>
      IEnumerable<string> GetContainerPathItemsForBuildingBlock(TBuildingBlock buildingBlock);

      /// <summary>
      ///    Updates a dimension for a parameter start value
      /// </summary>
      /// <param name="pathAndValueEntitiesBuildingBlock">The building block containing the start value being updated</param>
      /// <param name="pathAndValueEntity">The start value being updated</param>
      /// <param name="newDimension">The new dimension for the start value</param>
      /// <returns>The command used to update the start value dimension</returns>
      IMoBiCommand UpdatePathAndValueEntityDimension(TBuildingBlock pathAndValueEntitiesBuildingBlock, TPathAndValueEntity pathAndValueEntity, IDimension newDimension);

      /// <summary>
      ///    Returns the default dimension for the start value type
      /// </summary>
      /// <returns>A dimension which can be used as default when creating empty start values</returns>
      IDimension GetDefaultDimension();

      /// <summary>
      ///    Sets the value of a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that contains the start value</param>
      /// <param name="valueOrigin">The new value origin</param>
      /// <param name="pathAndValueEntity">The start value being modified</param>
      /// <returns>The command used to modify the start value</returns>
      ICommand SetValueOrigin(TBuildingBlock buildingBlock, ValueOrigin valueOrigin, TPathAndValueEntity pathAndValueEntity);

      /// <summary>
      ///    Creates a clone of the <paramref name="buildingBlockToClone" /> and adds it to <paramref name="parentModule" />
      /// </summary>
      ICommand CloneAndAddToParent(TBuildingBlock buildingBlockToClone, Module parentModule);
   }
}