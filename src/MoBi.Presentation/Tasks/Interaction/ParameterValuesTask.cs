using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuesTask : IInteractionTasksForExtendablePathAndValueEntity<ParameterValuesBuildingBlock, ParameterValue>
   {
      void AddStartValueExpression(ParameterValuesBuildingBlock buildingBlock);
      IMoBiCommand SetFullPath(ParameterValue parameterValue, ObjectPath entityPath, ParameterValuesBuildingBlock buildingBlock);
   }

   public class ParameterValuesTask : InteractionTasksForExtendablePathAndValueEntity<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
   {
      private readonly IParameterValuesCreator _parameterValuesCreator;

      public ParameterValuesTask(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ParameterValuesBuildingBlock> editTask,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IImportedQuantityToParameterValueMapper dtoToQuantityToParameterValueMapper,
         IParameterValueBuildingBlockExtendManager parameterValuesExtendManager,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IParameterValuePathTask parameterValuePathTask,
         IParameterValuesCreator parameterValuesCreator,
         IParameterFactory parameterFactory, 
         IObjectTypeResolver objectTypeResolver)
         : base(interactionTaskContext, editTask, parameterValuesExtendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoToQuantityToParameterValueMapper, parameterValuePathTask, parameterFactory, objectTypeResolver)
      {
         _parameterValuesCreator = parameterValuesCreator;
      }

      protected override IReadOnlyList<ParameterValue> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ParameterValuesBuildingBlock buildingBlock)
      {
         return _parameterValuesCreator.CreateFrom(spatialStructure, molecules);
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

      public void AddStartValueExpression(ParameterValuesBuildingBlock buildingBlock)
      {
         var (organ, molecules) = selectOrganAndProteins(buildingBlock.Module);
         if (organ == null || molecules == null || !molecules.Any())
            return;

         var newParameterValues = createExpressionBasedOn(organ, molecules);

         AddCommand(ExtendBuildingBlockWith(buildingBlock, newParameterValues));
      }

      public IMoBiCommand SetFullPath(ParameterValue parameterValue, ObjectPath entityPath, ParameterValuesBuildingBlock buildingBlock)
      {
         var newName = entityPath.Last();
         entityPath.RemoveAt(entityPath.Count - 1);

         var objectType = new ObjectTypeResolver().TypeFor(parameterValue);
         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = objectType,
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.EditPathAndName(objectType, parameterValue.ContainerPath, entityPath, parameterValue.Name, newName)
         };
         // Run this first before creating the command for rename because the entity needs to be in the building block before the name can be changed
         macroCommand.Add(_entityPathTask.SetContainerPathCommand(buildingBlock, parameterValue, entityPath).Run(Context));
         macroCommand.Add(_entityPathTask.UpdateNameCommand(buildingBlock, parameterValue, newName).Run(Context));

         return macroCommand;
      }

      private IReadOnlyList<ParameterValue> createExpressionBasedOn(IContainer organ, IReadOnlyList<MoleculeBuilder> molecules) => _parameterValuesCreator.CreateExpressionFrom(organ, molecules);

      private (IContainer organ, IReadOnlyList<MoleculeBuilder> molecules) selectOrganAndProteins(Module commonModule)
      {
         var moleculeBlockCollection = _interactionTaskContext.BuildingBlockRepository.MoleculeBlockCollection;
         var spatialStructureCollection = _interactionTaskContext.BuildingBlockRepository.SpatialStructureCollection;

         if (moleculeBlockCollection.Count == 0 && spatialStructureCollection.Count == 0)
            return (null, Enumerable.Empty<MoleculeBuilder>().ToList());

         using (var selectorPresenter = Context.Resolve<ISelectOrganAndProteinsPresenter>())
         {
            selectorPresenter.SelectSelectOrganAndProteins(commonModule);
            return (selectorPresenter.SelectedOrgan, selectorPresenter.SelectedMolecules);
         }
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