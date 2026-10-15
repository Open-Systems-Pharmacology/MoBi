using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuesTask : IInteractionTasksForExtendablePathAndValueEntity<ParameterValuesBuildingBlock, ParameterValue>
   {
      void AddStartValueExpression(ParameterValuesBuildingBlock buildingBlock);
      IReadOnlyList<ObjectPath> GetNewPaths();
      bool CorrectName(ParameterValuesBuildingBlock buildingBlock, Module module);
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
         IParameterValuePathTask parameterValuePathTask,
         IParameterValuesCreator parameterValuesCreator,
         IObjectTypeResolver objectTypeResolver,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         IParameterValuesToParameterValuesDataTableMapper dataTableMapper,
         IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper)
         : base(interactionTaskContext,
            editTask,
            parameterValuesExtendManager,
            cloneManagerForBuildingBlock,
            moBiFormulaTask,
            dtoToQuantityToParameterValueMapper,
            parameterValuePathTask,
            objectTypeResolver,
            exportDataTableToExcelTask,
            dataTableMapper,
            pathAndValueEntityToDistributedParameterMapper)
      {
         _parameterValuesCreator = parameterValuesCreator;
      }

      protected override IMoBiCommand GetUpdatePathAndValueEntityInBuildingBlockCommand(ParameterValuesBuildingBlock buildingBlock, ImportedQuantityDTO dto)
      {
         return new UpdateParameterValueInBuildingBlockCommand(buildingBlock, dto.Path, dto.QuantityInBaseUnit);
      }

      public override IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(ParameterValuesBuildingBlock buildingBlock, IReadOnlyList<ImportedQuantityDTO> startQuantities)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportParameterValues,
            ObjectType = ObjectTypes.ParameterValue
         };

         GetImportPathAndValueEntityMacroCommand(buildingBlock, startQuantities, macroCommand);

         return macroCommand.RunCommand(Context);
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
         var (organs, molecules) = selectOrganAndProteins(buildingBlock.Module);
         if (organs == null || molecules == null || !molecules.Any())
            return;

         var newParameterValues = createExpressionBasedOn(organs, molecules);

         var allSkipped = newParameterValues.Where(x => buildingBlock.FindByPath(x.Path) != null).ToList();

         if (allSkipped.Any())
            _interactionTaskContext.DialogCreator.MessageBoxInfo(AppConstants.Captions.BuildingBlockAlreadyContains(allSkipped.Select(x => x.Path.PathAsString).ToList()));

         AddCommand(ExtendBuildingBlockWith(buildingBlock, newParameterValues.Except(allSkipped).ToList()));
      }

      public IReadOnlyList<ObjectPath> GetNewPaths()
      {
         var selections = SelectNewPaths<ICreateObjectPathsFromReferencesPresenter>(AppConstants.Captions.SelectParameter);
         var invalidSelections = selections.Where(pathContainsIllegalCharacters).ToList();
         var validSelections = selections.Except(invalidSelections).ToList();

         if (invalidSelections.Any())
            _interactionTaskContext.DialogCreator.MessageBoxInfo(AppConstants.Captions.PathsContainIllegalCharacters(invalidSelections.Select(x => x.PathAsString).ToList(), Constants.ILLEGAL_CHARACTERS));

         return validSelections;
      }

      private bool pathContainsIllegalCharacters(ObjectPath objectPath) => objectPath.Any(element => Constants.ILLEGAL_CHARACTERS.Any(element.Contains));

      private IReadOnlyList<ParameterValue> createExpressionBasedOn(IReadOnlyList<IContainer> organs, IReadOnlyList<MoleculeBuilder> molecules) => organs.SelectMany(x => _parameterValuesCreator.CreateExpressionFrom(x, molecules)).ToList();

      private (IReadOnlyList<IContainer> organs, IReadOnlyList<MoleculeBuilder> molecules) selectOrganAndProteins(Module commonModule)
      {
         var moleculeBlockCollection = _interactionTaskContext.BuildingBlockRepository.MoleculeBlockCollection;
         var spatialStructureCollection = _interactionTaskContext.BuildingBlockRepository.SpatialStructureCollection;

         if (moleculeBlockCollection.Count == 0 && spatialStructureCollection.Count == 0)
            return (null, Enumerable.Empty<MoleculeBuilder>().ToList());

         using (var selectorPresenter = Context.Resolve<ISelectOrganAndProteinsPresenter>())
         {
            selectorPresenter.SelectSelectOrganAndProteins(commonModule);
            return (selectorPresenter.SelectedContainers, selectorPresenter.SelectedMolecules);
         }
      }

      public override bool CorrectName(ParameterValuesBuildingBlock buildingBlock, Module module)
      {
         var forbiddenNames = _editTask.GetForbiddenNames(buildingBlock, module.ParameterValuesCollection);
         return InteractionTask.CorrectName(buildingBlock, forbiddenNames);
      }

      protected override IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToAdd) => new AddParameterValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(ParameterValuesBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.ParameterValuesCollection;
      }
   }
}