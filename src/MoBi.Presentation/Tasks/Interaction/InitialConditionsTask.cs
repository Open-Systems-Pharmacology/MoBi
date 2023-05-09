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
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInitialConditionsTask : IStartValuesTask<InitialConditionsBuildingBlock, InitialCondition>
   {
      IMoBiCommand SetIsPresent(InitialConditionsBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool isPresent);

      IMoBiCommand SetNegativeValuesAllowed(InitialConditionsBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed);

      /// <summary>
      ///    Updates the scale divisor for a initial condition
      /// </summary>
      /// <param name="buildingBlock">The building block that this initial condition is part of</param>
      /// <param name="initialCondition">The initial condition being updated</param>
      /// <param name="newScaleDivisor">The new value of the scale divisor</param>
      /// <param name="oldScaleDivisor">The old value of the scale divisor</param>
      /// <returns>The command used to modify the initial condition</returns>
      IMoBiCommand UpdateInitialConditionScaleDivisor(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor);
   }

   public class InitialConditionsTask : StartValuesTask<InitialConditionsBuildingBlock, InitialCondition>, IInitialConditionsTask
   {
      private readonly IInitialConditionsCreator _initialConditionsCreator;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private readonly IMoleculeResolver _moleculeResolver;

      public InitialConditionsTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<InitialConditionsBuildingBlock> editTask,
         IInitialConditionsCreator initialConditionsCreator,
         IImportedQuantityToInitialConditionMapper dtoMapper,
         IInitialConditionsBuildingBlockExtendManager extendManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IReactionDimensionRetriever dimensionRetriever,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory, IInitialConditionPathTask initialConditionPathTask, IMoleculeResolver moleculeResolver)
         : base(interactionTaskContext, editTask, extendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoMapper, initialConditionPathTask)
      {
         _initialConditionsCreator = initialConditionsCreator;
         _dimensionRetriever = dimensionRetriever;
         _moleculeResolver = moleculeResolver;
      }

      public override void ExtendStartValueBuildingBlock(InitialConditionsBuildingBlock buildingBlock)
      {
         var newStartValues = createStartValuesBasedOnUsedTemplates(buildingBlock);
         updateDefaultIsPresentToFalseForSpecificExtendedValues(newStartValues, buildingBlock.ToCache());
         AddCommand(Extend(newStartValues, buildingBlock));
      }

      private InitialConditionsBuildingBlock createStartValuesBasedOnUsedTemplates(InitialConditionsBuildingBlock initialConditions)
      {
         var molecules = BuildingBlockById<MoleculeBuildingBlock>(initialConditions.MoleculeBuildingBlockId);
         var spatialStructure = BuildingBlockById<SpatialStructure>(initialConditions.SpatialStructureId);
         return _initialConditionsCreator.CreateFrom(spatialStructure, molecules);
      }

      public override InitialConditionsBuildingBlock CreatePathAndValueEntitiesForSimulation(SimulationConfiguration simulationConfiguration)
      {
         //TODO OSMOSES combining multiple spatial structures and molecule building blocks is not supported yet
         var simulationInitialConditions = _initialConditionsCreator.CreateFrom(simulationConfiguration.All<SpatialStructure>().First(), simulationConfiguration.All<MoleculeBuildingBlock>().First())
            .WithName(simulationConfiguration.All<InitialConditionsBuildingBlock>().First().Name);
         
         var templateValues = UpdateValuesFromTemplate(simulationInitialConditions, simulationConfiguration.All<InitialConditionsBuildingBlock>().First());
         updateDefaultIsPresentToFalseForSpecificExtendedValues(simulationInitialConditions, templateValues);
         return simulationInitialConditions;
      }

      private void updateDefaultIsPresentToFalseForSpecificExtendedValues(InitialConditionsBuildingBlock startValues, ICache<string, InitialCondition> templateValues)
      {
         var startValuesThatShouldPotentiallyNotBePresent = startValues.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
         var extendedStartValuesThatShouldNotBePresent = startValuesThatShouldPotentiallyNotBePresent.Where(x => !templateValues.Contains(x.Key));
         extendedStartValuesThatShouldNotBePresent.Each(x => x.Value.IsPresent = false);
      }

      public IMoBiCommand SetIsPresent(InitialConditionsBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool isPresent)
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

      private IMoBiCommand updateIsPresent(InitialConditionsBuildingBlock initialConditions, InitialCondition msv, bool isPresent)
      {
         return new UpdateInitialConditionIsPresentCommand(initialConditions, msv, isPresent).Run(Context);
      }

      public IMoBiCommand SetNegativeValuesAllowed(InitialConditionsBuildingBlock initialConditions, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed)
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

      private IMoBiCommand updateNegativeValuesAllowed(InitialConditionsBuildingBlock initialConditions, InitialCondition msv, bool negativeValuesAllowed)
      {
         return new UpdateInitialConditionNegativeValuesAllowedCommand(initialConditions, msv, negativeValuesAllowed).Run(Context);
      }

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition)
      {
         return AddNewFormulaAtBuildingBlock(buildingBlock, initialCondition, referenceParameter: null);
      }

      public override IMoBiCommand AddPathAndValueEntityToBuildingBlock(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition)
      {
         return GenerateAddCommand(buildingBlock, initialCondition).Run(Context);
      }

      public override IMoBiCommand ImportPathAndValueEntitiesToBuildingBlock(InitialConditionsBuildingBlock buildingBlock, IEnumerable<ImportedQuantityDTO> startQuantities)
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

      public override IMoBiCommand RemovePathAndValueEntityFromBuildingBlockCommand(InitialCondition pathAndValueEntity, InitialConditionsBuildingBlock buildingBlock)
      {
         return new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, pathAndValueEntity.Path);
      }

      public override IMoBiCommand RefreshPathAndValueEntitiesFromBuildingBlocks(InitialConditionsBuildingBlock buildingBlock, IEnumerable<InitialCondition> initialConditionsToRefresh)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshInitialConditionsFromBuildingBlocks,
            ObjectType = ObjectTypes.InitialCondition
         };

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

      public IMoBiCommand UpdateInitialConditionScaleDivisor(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor)
      {
         return new UpdateInitialConditionScaleDivisorCommand(buildingBlock, initialCondition, newScaleDivisor, oldScaleDivisor).Run(Context);
      }

      protected override SpatialStructure SpatialStructureReferencedBy(InitialConditionsBuildingBlock buildingBlock)
      {
         return Context.Get<SpatialStructure>(buildingBlock.SpatialStructureId) ?? _spatialStructureFactory.Create();
      }

      protected override MoleculeBuildingBlock MoleculeBuildingBlockReferencedBy(InitialConditionsBuildingBlock buildingBlock)
      {
         return Context.Get<MoleculeBuildingBlock>(buildingBlock.MoleculeBuildingBlockId) ?? Context.Create<MoleculeBuildingBlock>(buildingBlock.MoleculeBuildingBlockId);
      }

      public override bool IsEquivalentToOriginal(InitialCondition pathAndValueEntity, InitialConditionsBuildingBlock buildingBlock)
      {
         var moleculeBuilder = _moleculeResolver.Resolve(pathAndValueEntity.ContainerPath, pathAndValueEntity.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock));

         if (moleculeBuilder == null)
            return false;

         return HasEquivalentDimension(pathAndValueEntity, moleculeBuilder) &&
                HasEquivalentFormula(pathAndValueEntity, moleculeBuilder.DefaultStartFormula) &&
                HasEquivalentPathAndValueEntity(pathAndValueEntity, moleculeBuilder.GetDefaultInitialCondition());
      }

      public override IDimension GetDefaultDimension()
      {
         return _dimensionRetriever.MoleculeDimension;
      }

      public override bool CanResolve(InitialConditionsBuildingBlock buildingBlock, InitialCondition pathAndValueEntity)
      {
         return _moleculeResolver.Resolve(pathAndValueEntity.ContainerPath, pathAndValueEntity.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock)) != null;
      }

      protected override IMoBiCommand GetUpdatePathAndValueEntityInBuildingBlockCommand(InitialConditionsBuildingBlock buildingBlock, ImportedQuantityDTO dto)
      {
         var scaleDivisor = dto.IsScaleDivisorSpecified ? dto.ScaleDivisor : buildingBlock[dto.Path].ScaleDivisor;
         var pathAndValueEntity = dto.IsQuantitySpecified ? dto.QuantityInBaseUnit : buildingBlock[dto.Path].Value;

         return new UpdateInitialConditionInBuildingBlockCommand(buildingBlock, dto.Path, pathAndValueEntity, dto.IsPresent, scaleDivisor, dto.NegativeValuesAllowed);
      }

      protected override IMoBiCommand GenerateAddCommand(InitialConditionsBuildingBlock targetBuildingBlock, InitialCondition initialCondition)
      {
         return new AddInitialConditionToBuildingBlockCommand(targetBuildingBlock, initialCondition);
      }

      protected override IMoBiCommand GenerateRemoveCommand(InitialConditionsBuildingBlock targetBuildingBlock, InitialCondition initialCondition)
      {
         return new RemoveInitialConditionFromBuildingBlockCommand(targetBuildingBlock, initialCondition.Path);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(InitialConditionsBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.InitialConditionsCollection;
      }
   }
}