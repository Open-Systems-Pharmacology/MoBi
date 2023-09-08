using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuesTask : IStartValuesTask<ParameterValuesBuildingBlock, ParameterValue>
   {
   }

   public class ParameterValuesTask : StartValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
   {
      public ParameterValuesTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ParameterValuesBuildingBlock> editTask,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IImportedQuantityToParameterValueMapper dtoToQuantityToParameterValueMapper,
         IParameterValueBuildingBlockExtendManager parameterValuesExtendManager,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IParameterValuePathTask parameterValuePathTask)
         : base(interactionTaskContext, editTask, parameterValuesExtendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoToQuantityToParameterValueMapper, parameterValuePathTask)
      {
      }

      public override void ExtendStartValueBuildingBlock(ParameterValuesBuildingBlock buildingBlock)
      {
         AddCommand(Extend(new List<ParameterValue>(), buildingBlock));
      }

      public override IMoBiCommand AddPathAndValueEntityToBuildingBlock(ParameterValuesBuildingBlock buildingBlock, ParameterValue pathAndValueEntity)
      {
         return GenerateAddCommand(buildingBlock, pathAndValueEntity).Run(Context);
      }

      protected override IMoBiCommand GetUpdatePathAndValueEntityInBuildingBlockCommand(ParameterValuesBuildingBlock buildingBlock, ImportedQuantityDTO dto)
      {
         return new UpdateParameterValueInBuildingBlockCommand(buildingBlock, dto.Path, dto.QuantityInBaseUnit);
      }

      public override IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(ParameterValuesBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportParameterValues,
            ObjectType = ObjectTypes.ParameterValue
         };

         GetImportPathAndValueEntityMacroCommand(buildingBlock, startQuantities, macroCommand);

         return macroCommand.Run(Context);
      }

      public override IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(ParameterValue pathAndValueEntity, ParameterValuesBuildingBlock buildingBlock)
      {
         return new RemoveParameterValueFromBuildingBlockCommand(buildingBlock, pathAndValueEntity.Path);
      }

      public override IDimension GetDefaultDimension()
      {
         return Constants.Dimension.NO_DIMENSION;
      }

      protected override bool CorrectName(ParameterValuesBuildingBlock buildingBlock, Module module)
      {
         var forbiddenNames = _editTask.GetForbiddenNames(buildingBlock, module.ParameterValuesCollection);
         return InteractionTask.CorrectName(buildingBlock, forbiddenNames);
      }

      protected override IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToRemove)
      {
         return new RemoveParameterValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToAdd)
      {
         return new AddParameterValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(ParameterValuesBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.ParameterValuesCollection;
      }
   }
}