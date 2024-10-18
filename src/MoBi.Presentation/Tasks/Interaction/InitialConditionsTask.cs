using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInitialConditionsTask<TBuildingBlock> : IInteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, InitialCondition> where TBuildingBlock : class, IBuildingBlock<InitialCondition>
   {
      IMoBiCommand SetIsPresent(TBuildingBlock initialConditions, IEnumerable<InitialCondition> pathAndValueEntities, bool isPresent);

      IMoBiCommand SetNegativeValuesAllowed(TBuildingBlock initialConditions, IEnumerable<InitialCondition> pathAndValueEntities, bool negativeValuesAllowed);

      /// <summary>
      ///    Updates the scale divisor for a initial condition
      /// </summary>
      /// <param name="buildingBlock">The building block that this initial condition is part of</param>
      /// <param name="initialCondition">The initial condition being updated</param>
      /// <param name="newScaleDivisor">The new value of the scale divisor</param>
      /// <param name="oldScaleDivisor">The old value of the scale divisor</param>
      /// <returns>The command used to modify the initial condition</returns>
      IMoBiCommand UpdateInitialConditionScaleDivisor(TBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor);

      IMoBiCommand RefreshInitialConditionsFromBuildingBlocks(TBuildingBlock buildingBlock, IReadOnlyList<InitialCondition> initialConditions);

      bool CorrectName(TBuildingBlock buildingBlock, Module module);
   }

   public class InitialConditionsTask<TBuildingBlock> : InteractionTasksForExtendablePathAndValueEntity<TBuildingBlock, InitialCondition>, IInitialConditionsTask<TBuildingBlock> where TBuildingBlock : class, ILookupBuildingBlock<InitialCondition>, new()
   {
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      protected readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly INameCorrector _nameCorrector;

      public InitialConditionsTask(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IInitialConditionsBuildingBlockExtendManager extendManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IImportedQuantityToInitialConditionMapper dtoMapper,
         IInitialConditionPathTask initialConditionPathTask,
         IReactionDimensionRetriever dimensionRetriever,
         IInitialConditionsCreator initialConditionsCreator,
         IParameterFactory parameterFactory,
         IObjectTypeResolver objectTypeResolver,
         INameCorrector nameCorrector,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         IInitialConditionsToDataTableMapper dataTableMapper) :
         base(interactionTaskContext,
            editTask,
            extendManager,
            cloneManagerForBuildingBlock,
            moBiFormulaTask,
            spatialStructureFactory,
            dtoMapper,
            initialConditionPathTask,
            parameterFactory,
            objectTypeResolver,
            exportDataTableToExcelTask,
            dataTableMapper)
      {
         _dimensionRetriever = dimensionRetriever;
         _initialConditionsCreator = initialConditionsCreator;
         _nameCorrector = nameCorrector;
      }

      public IMoBiCommand SetIsPresent(TBuildingBlock initialConditions, IEnumerable<InitialCondition> pathAndValueEntities, bool isPresent)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingIsPresentCommandDescription(isPresent),
            ObjectType = ObjectTypes.InitialCondition,
         };

         pathAndValueEntities.Where(x => x.IsPresent != isPresent).Each(msv =>
            macroCommand.Add(updateIsPresent(initialConditions, msv, isPresent)));

         return macroCommand;
      }

      private IMoBiCommand updateIsPresent(TBuildingBlock initialConditions, InitialCondition msv, bool isPresent)
      {
         return new UpdateInitialConditionIsPresentCommand(initialConditions, msv, isPresent).RunCommand(Context);
      }

      public IMoBiCommand SetNegativeValuesAllowed(TBuildingBlock initialConditions, IEnumerable<InitialCondition> pathAndValueEntities, bool negativeValuesAllowed)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingNegativeValuesAllowedCommandDescription(negativeValuesAllowed),
            ObjectType = ObjectTypes.InitialCondition,
         };

         pathAndValueEntities.Where(x => x.NegativeValuesAllowed != negativeValuesAllowed).Each(msv =>
            macroCommand.Add(updateNegativeValuesAllowed(initialConditions, msv, negativeValuesAllowed)));

         return macroCommand;
      }

      public override bool CorrectName(TBuildingBlock buildingBlock, Module module)
      {
         // If this is an ExpressionProfileBuildingBlock, then the names of existing
         // building blocks in the module InitialConditionsCollection are not forbidden.
         if (buildingBlock is ExpressionProfileBuildingBlock)
            return base.CorrectName(buildingBlock, module);

         // This is an InitialConditionsBuildingBlock. Prevent renaming to the same name as
         // an existing building block in the InitialConditionsCollection
         var forbiddenNames = _editTask.GetForbiddenNames(buildingBlock, module.InitialConditionsCollection);
         return InteractionTask.CorrectName(buildingBlock, forbiddenNames);
      }

      private IMoBiCommand updateNegativeValuesAllowed(TBuildingBlock initialConditions, InitialCondition msv, bool negativeValuesAllowed)
      {
         return new UpdateInitialConditionNegativeValuesAllowedCommand(initialConditions, msv, negativeValuesAllowed).RunCommand(Context);
      }

      public override IMoBiCommand AddPathAndValueEntityToBuildingBlock(TBuildingBlock buildingBlock, InitialCondition initialCondition)
      {
         return GenerateAddCommand(buildingBlock, initialCondition).RunCommand(Context);
      }

      public override IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(TBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportInitialConditions,
            ObjectType = ObjectTypes.InitialCondition
         };

         GetImportPathAndValueEntityMacroCommand(buildingBlock, startQuantities, macroCommand);

         return macroCommand.RunCommand(Context);
      }

      public IMoBiCommand UpdateInitialConditionScaleDivisor(TBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor)
      {
         return new UpdateInitialConditionScaleDivisorCommand(buildingBlock, initialCondition, newScaleDivisor, oldScaleDivisor).RunCommand(Context);
      }

      public IMoBiCommand RefreshInitialConditionsFromBuildingBlocks(TBuildingBlock buildingBlock, IReadOnlyList<InitialCondition> initialConditions)
      {
         var initialConditionsModule = buildingBlock.Module;

         // When refreshing initial conditions from an expression profile, a default module will not be available
         var (_, molecules) = SelectBuildingBlocksForRefresh(initialConditionsModule?.Molecules, initialConditionsModule?.SpatialStructure, initialConditions.Select(x => x.MoleculeName).Distinct().ToList());

         if (molecules == null || !molecules.Any())
            return new MoBiEmptyCommand();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshInitialConditionsFromBuildingBlocks,
            ObjectType = ObjectTypes.InitialCondition
         };

         initialConditions.Each(initialCondition =>
         {
            var moleculeBuilder = molecules.FindByName(initialCondition.MoleculeName);
            if (moleculeBuilder == null)
               return;

            var originalInitialCondition = moleculeBuilder.GetDefaultInitialCondition();
            var originalUnit = moleculeBuilder.DisplayUnit;

            if (!ValueComparer.AreValuesEqual(Constants.DEFAULT_SCALE_DIVISOR, initialCondition.ScaleDivisor))
            {
               macroCommand.Add(UpdateInitialConditionScaleDivisor(buildingBlock, initialCondition, Constants.DEFAULT_SCALE_DIVISOR, initialCondition.ScaleDivisor));
            }

            if (!HasEquivalentDimension(initialCondition, moleculeBuilder))
            {
               macroCommand.Add(UpdatePathAndValueEntityDimension(buildingBlock, initialCondition, moleculeBuilder.Dimension));
            }

            if (!HasEquivalentPathAndValueEntity(initialCondition, originalInitialCondition))
            {
               macroCommand.Add(SetValueWithUnit(initialCondition, originalInitialCondition, originalUnit, buildingBlock));
            }

            if (!HasEquivalentFormula(initialCondition, moleculeBuilder.DefaultStartFormula))
            {
               updateFormulaUsages(buildingBlock, initialConditions, initialCondition, macroCommand, moleculeBuilder);
            }
         });

         return macroCommand;
      }

      private void updateFormulaUsages(TBuildingBlock buildingBlock, IReadOnlyList<InitialCondition> initialConditions, InitialCondition initialCondition, MoBiMacroCommand macroCommand, MoleculeBuilder moleculeBuilder)
      {
         // If all usages of a formula are being refreshed, then we can remove the existing formula from the formula cache
         if (canRemoveFormula(buildingBlock, initialConditions, initialCondition.Formula))
            macroCommand.Add(new RemoveFormulaFromFormulaCacheCommand(buildingBlock, initialCondition.Formula).RunCommand(Context));

         // The clone manager can return an existing formula from the formula cache if it is present and equivalent to the formula being cloned
         // This is important for renaming step. We can't just rename this formula if one already exists in the cache with the same name because
         // it might actually be the same one.
         var formulaCount = buildingBlock.FormulaCache.Count;
         var clonedFormula = _cloneManagerForBuildingBlock.Clone(moleculeBuilder.DefaultStartFormula, buildingBlock.FormulaCache);

         //This condition is to ensure we are not executing this AddEvent twice for the same formula
         //Since _cloneManagerForBuildingBlock.Clone(moleculeBuilder.DefaultStartFormula, buildingBlock.FormulaCache)
         //has the Cache as input parameter, and it is adding the formula if it is not already there
         //We need to check if the count has changed to know if the formula was added
         //If the count has changed, then we need to publish the event
         if (formulaCount != buildingBlock.FormulaCache.Count)
            Context.PublishEvent(new AddedEvent<IFormula>(clonedFormula, buildingBlock));

         // Update this formula in the initial condition
         macroCommand.Add(ChangeValueFormulaCommand(buildingBlock, initialCondition,
            moleculeBuilder.DefaultStartFormula.IsConstant() ? null : clonedFormula));

         // After the formula has been updated, if there are two formulas with the same name, then we can rename this one, so it does not collide
         if (buildingBlock.FormulaCache.Count(x => Equals(x.Name, clonedFormula.Name)) > 1)
            _nameCorrector.AutoCorrectName(buildingBlock.FormulaCache.AllNames(), clonedFormula);
      }

      private bool canRemoveFormula(TBuildingBlock buildingBlock, IReadOnlyList<InitialCondition> initialConditions, IFormula formula)
      {
         var allUsingFormula = buildingBlock.Where(x => HasEquivalentFormula(x, formula));
         return initialConditions.ContainsAll(allUsingFormula) && buildingBlock.FormulaCache.Contains(formula);
      }

      public override IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(InitialCondition pathAndValueEntity, TBuildingBlock buildingBlock)
      {
         return new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, pathAndValueEntity.Path);
      }

      protected override IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<InitialCondition> targetBuildingBlock, InitialCondition initialCondition)
      {
         return new AddInitialConditionToBuildingBlockCommand(targetBuildingBlock, initialCondition);
      }

      protected override IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<InitialCondition> targetBuildingBlock, InitialCondition initialCondition)
      {
         return new RemoveInitialConditionFromBuildingBlockCommand(targetBuildingBlock, initialCondition.Path);
      }

      public override IDimension GetDefaultDimension()
      {
         return _dimensionRetriever.MoleculeDimension;
      }

      protected override IMoBiCommand GetUpdatePathAndValueEntityInBuildingBlockCommand(TBuildingBlock buildingBlock, ImportedQuantityDTO dto)
      {
         var scaleDivisor = dto.IsScaleDivisorSpecified ? dto.ScaleDivisor : buildingBlock.Single(x => Equals(x.Path, dto.Path)).ScaleDivisor;
         var pathAndValueEntity = dto.IsQuantitySpecified ? dto.QuantityInBaseUnit : buildingBlock.Single(x => Equals(x.Path, dto.Path)).Value;

         return new UpdateInitialConditionInBuildingBlockCommand(buildingBlock, dto.Path, pathAndValueEntity, dto.IsPresent, scaleDivisor, dto.NegativeValuesAllowed);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(TBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.InitialConditionsCollection;
      }

      private void updateDefaultIsPresentToFalseForSpecificExtendedValues(IReadOnlyList<InitialCondition> allInitialConditions, IReadOnlyList<InitialCondition> templateValues)
      {
         var templateInitialConditions = templateValues.ToCache();
         var entitiesThatShouldPotentiallyNotBePresent = allInitialConditions.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
         var newInitialConditionsToUpdate = entitiesThatShouldPotentiallyNotBePresent.Where(x => !templateInitialConditions.Contains(x.Key));
         newInitialConditionsToUpdate.Each(x => x.Value.IsPresent = false);
      }

      protected override IReadOnlyList<InitialCondition> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, TBuildingBlock initialConditionsBuildingBlock)
      {
         var newEntities = _initialConditionsCreator.CreateFrom(spatialStructure, molecules).ToList();
         updateDefaultIsPresentToFalseForSpecificExtendedValues(newEntities, initialConditionsBuildingBlock.ToList());
         return newEntities;
      }
   }
}