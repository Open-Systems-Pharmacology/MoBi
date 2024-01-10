using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Services;
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
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuesTask : IStartValuesTask<ParameterValuesBuildingBlock, ParameterValue>
   {
      void AddStartValueExpression(ParameterValuesBuildingBlock buildingBlock);
   }

   public class ParameterValuesTask : StartValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
   {
      private readonly IParameterValuesCreator _parameterValuesCreator;
      private readonly IDialogCreator _dialogCreator;

      public ParameterValuesTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ParameterValuesBuildingBlock> editTask,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IImportedQuantityToParameterValueMapper dtoToQuantityToParameterValueMapper,
         IParameterValueBuildingBlockExtendManager parameterValuesExtendManager,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IParameterValuePathTask parameterValuePathTask,
         IParameterValuesCreator parameterValuesCreator,
         IDialogCreator dialogCreator)
         : base(interactionTaskContext, editTask, parameterValuesExtendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoToQuantityToParameterValueMapper, parameterValuePathTask)
      {
         _parameterValuesCreator = parameterValuesCreator;
         _dialogCreator = dialogCreator;
      }

      protected override IReadOnlyList<ParameterValue> CreateStartValuesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ParameterValuesBuildingBlock buildingBlock)
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

         var newStartValues = createExpressionBasedOn(organ, molecules);
         adviseExistingPaths(buildingBlock, newStartValues);

         AddCommand(Extend(newStartValues, buildingBlock));
      }

      private void adviseExistingPaths(ParameterValuesBuildingBlock buildingBlock, IReadOnlyList<ParameterValue> newStartValues)
      {
         var newPathsToAdd = newStartValues.Select(newStartValue => newStartValue.Path.PathAsString);
         var pathsThatAlreadyExist = buildingBlock.Select(x => x.Path.PathAsString).Where(x => newPathsToAdd.Contains(x)).ToList();

         if (pathsThatAlreadyExist.Any())
            _dialogCreator.MessageBoxInfo(AppConstants.Captions.ExistingParameterValuesWillNotBeReplaced(pathsThatAlreadyExist));
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