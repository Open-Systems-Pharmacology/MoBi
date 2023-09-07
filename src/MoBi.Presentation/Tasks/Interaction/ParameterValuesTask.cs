using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
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
         IParameterValuesCreator startValuesCreator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IImportedQuantityToParameterValueMapper dtoToQuantityToParameterValueMapper,
         IParameterResolver parameterResolver,
         IParameterValueBuildingBlockExtendManager parameterValuesExtendManager,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IParameterValuePathTask parameterValuePathTask)
         : base(interactionTaskContext, editTask, parameterValuesExtendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoToQuantityToParameterValueMapper, parameterValuePathTask)
      {
      }

      private IReadOnlyList<ParameterValue> createTempStartValues(ParameterValuesBuildingBlock parameterValues)
      {
         return new List<ParameterValue>();
      }

      public override void ExtendStartValueBuildingBlock(ParameterValuesBuildingBlock buildingBlock)
      {
         var newStartValues = createTempStartValues(buildingBlock);
         AddCommand(Extend(newStartValues, buildingBlock));
      }

      public override IMoBiCommand AddPathAndValueEntityToBuildingBlock(ParameterValuesBuildingBlock buildingBlock, ParameterValue pathAndValueEntity)
      {
         return GenerateAddCommand(buildingBlock, pathAndValueEntity).Run(Context);
      }

      protected override IMoBiCommand GetUpdatePathAndValueEntityInBuildingBlockCommand(ParameterValuesBuildingBlock buildingBlock, ImportedQuantityDTO dto)
      {
         return new UpdateParameterValueInBuildingBlockCommand(buildingBlock, dto.Path, dto.QuantityInBaseUnit);
      }

      public IParameter GetPossibleParameterFromProject(ObjectPath parameterPath)
      {
         foreach (var topContainer in BuildingBlockRepository.SpatialStructureCollection.SelectMany(spatialStructure => spatialStructure.TopContainers))
         {
            var parameter = parameterPath.TryResolve<IParameter>(topContainer, out _);
            if (parameter != null)
               return parameter;
         }

         return null;
      }

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(ParameterValuesBuildingBlock buildingBlock, ParameterValue parameterValue)
      {
         var parameter = GetPossibleParameterFromProject(parameterValue.Path);

         return AddNewFormulaAtBuildingBlock(buildingBlock, parameterValue, parameter);
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