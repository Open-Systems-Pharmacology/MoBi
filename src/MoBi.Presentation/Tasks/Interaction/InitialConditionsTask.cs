using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Extensions;
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
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInitialConditionsTask<TBuildingBlock> : IStartValuesTask<TBuildingBlock, InitialCondition> where TBuildingBlock : class, IBuildingBlock<InitialCondition>
   {
      IMoBiCommand SetIsPresent(TBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool isPresent);

      IMoBiCommand SetNegativeValuesAllowed(TBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed);

      /// <summary>
      ///    Updates the scale divisor for a initial condition
      /// </summary>
      /// <param name="buildingBlock">The building block that this initial condition is part of</param>
      /// <param name="initialCondition">The initial condition being updated</param>
      /// <param name="newScaleDivisor">The new value of the scale divisor</param>
      /// <param name="oldScaleDivisor">The old value of the scale divisor</param>
      /// <returns>The command used to modify the initial condition</returns>
      IMoBiCommand UpdateInitialConditionScaleDivisor(TBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor);

      void ExtendExpressionProfileInitialConditions(ExpressionProfileBuildingBlock buildingBlock);
   }

   public class InitialConditionsTask<TBuildingBlock> : StartValuesTask<TBuildingBlock, InitialCondition>, IInitialConditionsTask<TBuildingBlock> where TBuildingBlock : class, ILookupBuildingBlock<InitialCondition>
   {
      private readonly IMoleculeResolver _moleculeResolver;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      protected readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly IInteractionTasksForMoleculeBuilder _moleculeBuilderTask;

      public InitialConditionsTask(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IInitialConditionsBuildingBlockExtendManager extendManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory,
         IImportedQuantityToInitialConditionMapper dtoMapper,
         IInitialConditionPathTask initialConditionPathTask,
         IMoleculeResolver moleculeResolver,
         IReactionDimensionRetriever dimensionRetriever,
         IInitialConditionsCreator initialConditionsCreator,
         IInteractionTasksForMoleculeBuilder moleculeBuilderTask) : base(interactionTaskContext, editTask, extendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoMapper, initialConditionPathTask)
      {
         _moleculeResolver = moleculeResolver;
         _dimensionRetriever = dimensionRetriever;
         _initialConditionsCreator = initialConditionsCreator;
         _moleculeBuilderTask = moleculeBuilderTask;
      }

      public IMoBiCommand SetIsPresent(TBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool isPresent)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingIsPresentCommandDescription(isPresent),
            ObjectType = ObjectTypes.InitialCondition,
         };

         startValues.Where(x => x.IsPresent != isPresent).Each(msv =>
            macroCommand.Add(updateIsPresent(initialConditions, msv, isPresent)));

         return macroCommand;
      }

      private IMoBiCommand updateIsPresent(TBuildingBlock initialConditions, InitialCondition msv, bool isPresent)
      {
         return new UpdateInitialConditionIsPresentCommand(initialConditions, msv, isPresent).Run(Context);
      }

      public IMoBiCommand SetNegativeValuesAllowed(TBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingNegativeValuesAllowedCommandDescription(negativeValuesAllowed),
            ObjectType = ObjectTypes.InitialCondition,
         };

         startValues.Where(x => x.NegativeValuesAllowed != negativeValuesAllowed).Each(msv =>
            macroCommand.Add(updateNegativeValuesAllowed(initialConditions, msv, negativeValuesAllowed)));

         return macroCommand;
      }

      private IMoBiCommand updateNegativeValuesAllowed(TBuildingBlock initialConditions, InitialCondition msv, bool negativeValuesAllowed)
      {
         return new UpdateInitialConditionNegativeValuesAllowedCommand(initialConditions, msv, negativeValuesAllowed).Run(Context);
      }

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(TBuildingBlock buildingBlock, InitialCondition initialCondition)
      {
         return AddNewFormulaAtBuildingBlock(buildingBlock, initialCondition, referenceParameter: null);
      }

      public override IMoBiCommand AddPathAndValueEntityToBuildingBlock(TBuildingBlock buildingBlock, InitialCondition initialCondition)
      {
         return GenerateAddCommand(buildingBlock, initialCondition).Run(Context);
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

         return macroCommand.Run(Context);
      }

      public IMoBiCommand UpdateInitialConditionScaleDivisor(TBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor)
      {
         return new UpdateInitialConditionScaleDivisorCommand(buildingBlock, initialCondition, newScaleDivisor, oldScaleDivisor).Run(Context);
      }

      public void ExtendExpressionProfileInitialConditions(ExpressionProfileBuildingBlock buildingBlock)
      {
         var (spatialStructure, _) = SelectBuildingBlocksForExtend(moleculeRequired: false);
         if (spatialStructure == null)
            return;

         // Create a temporary molecule builder with the correct name, dimensions and formula to use when constructing Initial Conditions
         var moleculeBuilder = _moleculeBuilderTask.CreateDefault(buildingBlock.MoleculeName, mostUsedFormula(buildingBlock.InitialConditions));
         var newInitialConditions = spatialStructure.PhysicalContainers.Select(container => _initialConditionsCreator.CreateInitialCondition(container, moleculeBuilder)).ToList();

         updateDefaultIsPresentToFalseForSpecificExtendedValues(newInitialConditions, buildingBlock.InitialConditions.ToList());
         AddCommand(Extend(newInitialConditions, buildingBlock));
      }

      private IFormula mostUsedFormula(IReadOnlyCollection<InitialCondition> initialConditions)
      {
         if (!initialConditions.Any())
            return null;
         
         return initialConditions.GroupBy(x => x.Formula).OrderBy(x => x.Count()).First().Key;
      }

      public override IMoBiCommand RefreshPathAndValueEntitiesFromBuildingBlocks(TBuildingBlock buildingBlock, IEnumerable<InitialCondition> initialConditionsToRefresh)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshInitialConditionsFromBuildingBlocks,
            ObjectType = ObjectTypes.InitialCondition
         };

         // TODO OSMOSES
         initialConditionsToRefresh.Each(pathAndValueEntity =>
         {
            var moleculeBuilder = _moleculeResolver.Resolve(pathAndValueEntity.ContainerPath, pathAndValueEntity.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock));
            if (moleculeBuilder == null) return;

            var originalInitialCondition = moleculeBuilder.GetDefaultInitialCondition();
            var originalUnit = moleculeBuilder.DisplayUnit;

            if (!ValueComparer.AreValuesEqual(Constants.DEFAULT_SCALE_DIVISOR, pathAndValueEntity.ScaleDivisor))
            {
               macroCommand.Add(UpdateInitialConditionScaleDivisor(buildingBlock, pathAndValueEntity, Constants.DEFAULT_SCALE_DIVISOR, pathAndValueEntity.ScaleDivisor));
            }

            if (!HasEquivalentDimension(pathAndValueEntity, moleculeBuilder))
            {
               macroCommand.Add(UpdatePathAndValueEntityDimension(buildingBlock, pathAndValueEntity, moleculeBuilder.Dimension));
            }

            if (!HasEquivalentPathAndValueEntity(pathAndValueEntity, originalInitialCondition))
            {
               macroCommand.Add(SetValueWithUnit(pathAndValueEntity, originalInitialCondition, originalUnit, buildingBlock));
            }

            if (!HasEquivalentFormula(pathAndValueEntity, moleculeBuilder.DefaultStartFormula))
            {
               macroCommand.Add(ChangeValueFormulaCommand(buildingBlock, pathAndValueEntity,
                  moleculeBuilder.DefaultStartFormula.IsConstant() ? null : _cloneManagerForBuildingBlock.Clone(moleculeBuilder.DefaultStartFormula, buildingBlock.FormulaCache)));
            }
         });

         return macroCommand;
      }

      private MoleculeBuildingBlock MoleculeBuildingBlockReferencedBy(TBuildingBlock buildingBlock)
      {
         return new MoleculeBuildingBlock();
      }

      private SpatialStructure SpatialStructureReferencedBy(TBuildingBlock buildingBlock)
      {
         return new MoBiSpatialStructure();
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
         var startValuesThatShouldPotentiallyNotBePresent = allInitialConditions.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
         var newInitialConditionsToUpdate = startValuesThatShouldPotentiallyNotBePresent.Where(x => !templateInitialConditions.Contains(x.Key));
         newInitialConditionsToUpdate.Each(x => x.Value.IsPresent = false);
      }

      public override void ExtendStartValueBuildingBlock(TBuildingBlock initialConditionsBuildingBlock)
      {
         var (spatialStructure, moleculeBuildingBlock) = SelectBuildingBlocksForExtend();
         if (spatialStructure == null || moleculeBuildingBlock == null)
            return;

         var newStartValues = createStartValuesBasedOnUsedTemplates(spatialStructure, moleculeBuildingBlock.Where(x => !x.QuantityType.Is(QuantityType.Protein)).ToList());
         updateDefaultIsPresentToFalseForSpecificExtendedValues(newStartValues.ToList(), initialConditionsBuildingBlock.ToList());
         AddCommand(Extend(newStartValues.ToList(), initialConditionsBuildingBlock));
      }

      private InitialConditionsBuildingBlock createStartValuesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules)
      {
         return _initialConditionsCreator.CreateFrom(spatialStructure, molecules);
      }
   }
}