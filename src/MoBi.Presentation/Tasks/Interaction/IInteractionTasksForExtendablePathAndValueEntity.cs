using System.Collections.Generic;
using System.Data;
using MoBi.Core.Commands;
using MoBi.Presentation.DTO;
using NPOI.SS.Formula.Functions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, in TPathAndValueEntity> : IInteractionTasksForBuildingBlock<Module, TBuildingBlock>, IInteractionTasksForPathAndValueEntity<TBuildingBlock, TPathAndValueEntity>
      where TBuildingBlock : class, IBuildingBlock<TPathAndValueEntity>
      where TPathAndValueEntity : PathAndValueEntity
   {
      void ExtendPathAndValueEntityBuildingBlock(TBuildingBlock buildingBlock);

      /// <summary>
      ///    Generates a command that will add the pathAndValueEntity to the building block
      /// </summary>
      /// <param name="buildingBlock">The building block being updated</param>
      /// <param name="pathAndValueEntity">The path and value entity being added</param>
      /// <returns>A MoBiCommand that can be executed</returns>
      IMoBiCommand AddPathAndValueEntityToBuildingBlock(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity);

      /// <summary>
      ///    Generates a command that will add or update multiple parameter path and value entities in the building block
      /// </summary>
      /// <param name="buildingBlock">The building block being updated</param>
      /// <param name="startQuantities">The list of path and value entities to be added or updated</param>
      /// <returns>The command used to update the building block</returns>
      IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(TBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities);

      /// <summary>
      ///    Sets the path and value entity formula to a new value
      /// </summary>
      /// <param name="buildingBlock">The building block which contains the path and value entity</param>
      /// <param name="pathAndValueEntity">The path and value entity being updated</param>
      /// <param name="formula">The new formula</param>
      /// <returns>The command used to execute the update</returns>
      IMoBiCommand ChangeValueFormulaCommand(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, IFormula formula);

      /// <summary>
      ///    Modifies the value of the PathAndValueEntity
      /// </summary>
      /// <param name="pathAndValueEntity">The path and value entity being modified</param>
      /// <param name="newDisplayValue">The new display value of the path and value entity</param>
      /// <param name="unit">The new unit of the path and value entity</param>
      /// <param name="buildingBlock">The building block that the entity is a member of</param>
      /// <returns>The command used to modify the path and value entity</returns>
      IMoBiCommand SetDisplayValueWithUnit(TPathAndValueEntity pathAndValueEntity, double? newDisplayValue, Unit unit, TBuildingBlock buildingBlock);

      /// <summary>
      ///    Returns a command that can be used to remove the path and value entities contained in <paramref name="pathAndValueEntity" />
      ///    from
      ///    <paramref name="buildingBlock" />
      /// </summary>
      /// <returns>The command, which has not been run</returns>
      IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(TPathAndValueEntity pathAndValueEntity, TBuildingBlock buildingBlock);

      /// <summary>
      ///    Renames a path and value entity inside the list of path and value entities
      /// </summary>
      /// <param name="buildingBlock">The list of path and value entities</param>
      /// <param name="pathAndValueEntity">The path and value entity to be renamed</param>
      /// <param name="newValue">The new name to be used</param>
      /// <returns>The command that was run to rename the building block</returns>
      IMoBiCommand EditPathAndValueEntityName(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, string newValue);

      /// <summary>
      ///    Changes one element in the path and value entity container path
      /// </summary>
      /// <param name="buildingBlock">The building block containing the path and value entity</param>
      /// <param name="pathAndValueEntity">The path and value entity that will be updated</param>
      /// <param name="indexToUpdate">The index of the value being modified</param>
      /// <param name="newValue">The new value for the path</param>
      /// <returns>The command that was run to edit the container path</returns>
      IMoBiCommand EditPathAndValueEntityContainerPath(TBuildingBlock buildingBlock, TPathAndValueEntity pathAndValueEntity, int indexToUpdate, string newValue);

      /// <summary>
      ///    Retrieves all the parent container path elements from the molecule building block and spatial structure
      /// </summary>
      /// <param name="buildingBlock">The path and value entity building block</param>
      /// <returns>A list of possible valid container path elements</returns>
      IEnumerable<string> GetContainerPathItemsForBuildingBlock(TBuildingBlock buildingBlock);

      /// <summary>
      ///    Updates a dimension for a parameter path and value entity
      /// </summary>
      /// <param name="pathAndValueEntitiesBuildingBlock">The building block containing the path and value entity being updated</param>
      /// <param name="pathAndValueEntity">The path and value entity being updated</param>
      /// <param name="newDimension">The new dimension for the path and value entity</param>
      /// <returns>The command used to update the path and value entity dimension</returns>
      IMoBiCommand UpdatePathAndValueEntityDimension(TBuildingBlock pathAndValueEntitiesBuildingBlock, TPathAndValueEntity pathAndValueEntity, IDimension newDimension);

      /// <summary>
      ///    Returns the default dimension for the path and value entity type
      /// </summary>
      /// <returns>A dimension which can be used as default when creating empty path and value entities</returns>
      IDimension GetDefaultDimension();

      /// <summary>
      ///    Creates a clone of the <paramref name="buildingBlockToClone" /> and adds it to <paramref name="parentModule" />
      /// </summary>
      ICommand CloneAndAddToParent(TBuildingBlock buildingBlockToClone, Module parentModule);

      IMoBiCommand AddOrExtendWith(TBuildingBlock buildingBlock, Module spatialStructureModule);
   }
}