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
      IParameter GetPossibleParameterFromProject(ObjectPath parameterPath);
   }

   public class ParameterValuesTask : StartValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
   {
      private readonly IParameterValuesCreator _startValuesCreator;
      private readonly IParameterResolver _parameterResolver;

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
         _startValuesCreator = startValuesCreator;
         _parameterResolver = parameterResolver;
      }

      private ParameterValuesBuildingBlock createTempStartValues(ParameterValuesBuildingBlock parameterValues)
      {
         return new ParameterValuesBuildingBlock();
         // var molecules = BuildingBlockById<MoleculeBuildingBlock>(parameterStartValues.MoleculeBuildingBlockId);
         // var spatialStructure = BuildingBlockById<SpatialStructure>(parameterStartValues.SpatialStructureId);
         // return _startValuesCreator.CreateFrom(spatialStructure, molecules);
      }

      public override void ExtendStartValueBuildingBlock(ParameterValuesBuildingBlock buildingBlock, SpatialStructure spatialStructure, MoleculeBuildingBlock moleculeBuildingBlock)
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

      public override IMoBiCommand RefreshPathAndValueEntitiesFromBuildingBlocks(ParameterValuesBuildingBlock buildingBlock, IEnumerable<ParameterValue> pathAndValueEntitiesToRefresh)
      {
         // TODO OSMOSES
         // var spatialStructure = SpatialStructureReferencedBy(buildingBlock);
         // var moleculeBuildingBlock = MoleculeBuildingBlockReferencedBy(buildingBlock);

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshParameterValuesFromBuildingBlocks,
            ObjectType = ObjectTypes.ParameterValue
         };

         // pathAndValueEntitiesToRefresh.Each(pathAndValueEntity =>
         // {
         //    var parameter = _parameterResolver.Resolve(pathAndValueEntity.ContainerPath, pathAndValueEntity.Name, spatialStructure, moleculeBuildingBlock);
         //    if (parameter == null)
         //       return;
         //
         //    if (!HasEquivalentDimension(pathAndValueEntity, parameter))
         //       macroCommand.Add(UpdatePathAndValueEntityDimension(buildingBlock, pathAndValueEntity, parameter.Dimension));
         //
         //    if (!HasEquivalentPathAndValueEntity(pathAndValueEntity, parameter))
         //       macroCommand.Add(SetDisplayValueWithUnit(pathAndValueEntity, parameter.ConvertToDisplayUnit(parameter.Value), parameter.DisplayUnit, buildingBlock));
         //
         //    // Evaluating the pathAndValueEntity before the formula is important if the pathAndValueEntity is a constant and the original building block uses a constant formula
         //    if (!HasEquivalentFormula(pathAndValueEntity, parameter.Formula))
         //       macroCommand.Add(ChangeValueFormulaCommand(buildingBlock, pathAndValueEntity, parameter.Formula.IsConstant() ? null : _cloneManagerForBuildingBlock.Clone(parameter.Formula, buildingBlock.FormulaCache)));
         // });

         return macroCommand;
      }

      protected override IMoBiCommand GenerateRemoveCommand(ParameterValuesBuildingBlock targetBuildingBlock, ParameterValue startValueToRemove)
      {
         return new RemoveParameterValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IMoBiCommand GenerateAddCommand(ParameterValuesBuildingBlock targetBuildingBlock, ParameterValue startValueToAdd)
      {
         return new AddParameterValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(ParameterValuesBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.ParameterValuesCollection;
      }
   }
}