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
      IMoBiCommand UpdateStartValueScaleDivisor(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor);
   }

   public class InitialConditionsTask : AbstractStartValuesTask<InitialConditionsBuildingBlock, InitialCondition>, IInitialConditionsTask
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

      public override void ExtendStartValues(InitialConditionsBuildingBlock initialConditionsBuildingBlock)
      {
         var newStartValues = createStartValuesBasedOnUsedTemplates(initialConditionsBuildingBlock);
         updateDefaultIsPresentToFalseForSpecificExtendedValues(newStartValues, initialConditionsBuildingBlock.ToCache());
         AddCommand(Extend(newStartValues, initialConditionsBuildingBlock));
      }

      private InitialConditionsBuildingBlock createStartValuesBasedOnUsedTemplates(InitialConditionsBuildingBlock initialConditions)
      {
         var molecules = BuildingBlockById<MoleculeBuildingBlock>(initialConditions.MoleculeBuildingBlockId);
         var spatialStructure = BuildingBlockById<SpatialStructure>(initialConditions.SpatialStructureId);
         return _initialConditionsCreator.CreateFrom(spatialStructure, molecules);
      }

      public override InitialConditionsBuildingBlock CreateStartValuesForSimulation(SimulationConfiguration simulationConfiguration)
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

      public override IMoBiCommand AddStartValueToBuildingBlock(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition)
      {
         return GenerateAddCommand(buildingBlock, initialCondition).Run(Context);
      }

      public override IMoBiCommand ImportStartValuesToBuildingBlock(InitialConditionsBuildingBlock startValuesBuildingBlock, IEnumerable<ImportedQuantityDTO> startValues)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportInitialConditions,
            ObjectType = ObjectTypes.InitialCondition
         };

         GetImportStartValuesMacroCommand(startValuesBuildingBlock, startValues, macroCommand);

         return macroCommand.Run(Context);
      }

      public override IMoBiCommand RemoveStartValueFromBuildingBlockCommand(InitialCondition startValue, InitialConditionsBuildingBlock buildingBlock)
      {
         return new RemoveInitialConditionFromBuildingBlockCommand(buildingBlock, startValue.Path);
      }

      public override IMoBiCommand RefreshStartValuesFromBuildingBlocks(InitialConditionsBuildingBlock buildingBlock, IEnumerable<InitialCondition> startValuesToRefresh)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshStartValuesFromBuildingBlocks,
            ObjectType = ObjectTypes.InitialCondition
         };

         startValuesToRefresh.Each(startValue =>
         {
            var moleculeBuilder = _moleculeResolver.Resolve(startValue.ContainerPath, startValue.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock));
            if (moleculeBuilder == null) return;

            var originalStartValue = moleculeBuilder.GetDefaultInitialCondition();
            var originalUnit = moleculeBuilder.DisplayUnit;

            if (!ValueComparer.AreValuesEqual(Constants.DEFAULT_SCALE_DIVISOR, startValue.ScaleDivisor))
            {
               macroCommand.Add(UpdateStartValueScaleDivisor(buildingBlock, startValue, Constants.DEFAULT_SCALE_DIVISOR, startValue.ScaleDivisor));
            }

            if (!HasEquivalentDimension(startValue, moleculeBuilder))
            {
               macroCommand.Add(UpdateStartValueDimension(buildingBlock, startValue, moleculeBuilder.Dimension));
            }

            if (!HasEquivalentStartValue(startValue, originalStartValue))
            {
               macroCommand.Add(SetValueWithUnit(startValue, originalStartValue, originalUnit, buildingBlock));
            }

            if (!HasEquivalentFormula(startValue, moleculeBuilder.DefaultStartFormula))
            {
               macroCommand.Add(ChangeValueFormulaCommand(buildingBlock, startValue,
                  moleculeBuilder.DefaultStartFormula.IsConstant() ? null : _cloneManagerForBuildingBlock.Clone(moleculeBuilder.DefaultStartFormula, buildingBlock.FormulaCache)));
            }
         });

         return macroCommand;
      }

      public IMoBiCommand UpdateStartValueScaleDivisor(InitialConditionsBuildingBlock buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor)
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

      public override bool IsEquivalentToOriginal(InitialCondition startValue, InitialConditionsBuildingBlock buildingBlock)
      {
         var moleculeBuilder = _moleculeResolver.Resolve(startValue.ContainerPath, startValue.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock));

         if (moleculeBuilder == null)
            return false;

         return HasEquivalentDimension(startValue, moleculeBuilder) &&
                HasEquivalentFormula(startValue, moleculeBuilder.DefaultStartFormula) &&
                HasEquivalentStartValue(startValue, moleculeBuilder.GetDefaultInitialCondition());
      }

      public override IDimension GetDefaultDimension()
      {
         return _dimensionRetriever.MoleculeDimension;
      }

      public override bool CanResolve(InitialConditionsBuildingBlock buildingBlock, InitialCondition startValue)
      {
         return _moleculeResolver.Resolve(startValue.ContainerPath, startValue.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock)) != null;
      }

      protected override IMoBiCommand GetUpdateStartValueInBuildingBlockCommand(InitialConditionsBuildingBlock startValuesBuildingBlock, ImportedQuantityDTO dto)
      {
         var scaleDisivor = dto.IsScaleDivisorSpecified ? dto.ScaleDivisor : startValuesBuildingBlock[dto.Path].ScaleDivisor;
         var startValue = dto.IsQuantitySpecified ? dto.QuantityInBaseUnit : startValuesBuildingBlock[dto.Path].Value;

         return new UpdateInitialConditionInBuildingBlockCommand(startValuesBuildingBlock, dto.Path, startValue, dto.IsPresent, scaleDisivor, dto.NegativeValuesAllowed);
      }

      protected override IMoBiCommand GenerateAddCommand(InitialConditionsBuildingBlock targetBuildingBlock, InitialCondition startValueToAdd)
      {
         return new AddInitialConditionToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }

      protected override IMoBiCommand GenerateRemoveCommand(InitialConditionsBuildingBlock targetBuildingBlock, InitialCondition startValueToRemove)
      {
         return new RemoveInitialConditionFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(InitialConditionsBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.InitialConditionsCollection;
      }
   }
}