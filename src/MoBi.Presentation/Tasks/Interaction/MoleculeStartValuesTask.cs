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
   public interface IMoleculeStartValuesTask : IStartValuesTask<MoleculeStartValuesBuildingBlock, MoleculeStartValue>
   {
      IMoBiCommand SetIsPresent(MoleculeStartValuesBuildingBlock moleculeStartValues, IEnumerable<MoleculeStartValue> startValues, bool isPresent);

      IMoBiCommand SetNegativeValuesAllowed(MoleculeStartValuesBuildingBlock moleculeStartValues, IEnumerable<MoleculeStartValue> startValues, bool negativeValuesAllowed);

      /// <summary>
      ///    Updates the scale divisor for a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that this start value is part of</param>
      /// <param name="startValue">The start value being updated</param>
      /// <param name="newScaleDivisor">The new value of the scale divisor</param>
      /// <param name="oldScaleDivisor">The old value of the scale divisor</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand UpdateStartValueScaleDivisor(MoleculeStartValuesBuildingBlock buildingBlock, MoleculeStartValue startValue, double newScaleDivisor, double oldScaleDivisor);
   }

   public class MoleculeStartValuesTask : AbstractStartValuesTask<MoleculeStartValuesBuildingBlock, MoleculeStartValue>, IMoleculeStartValuesTask
   {
      private readonly IMoleculeStartValuesCreator _startValuesCreator;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private readonly IMoleculeResolver _moleculeResolver;

      public MoleculeStartValuesTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<MoleculeStartValuesBuildingBlock> editTask,
         IMoleculeStartValuesCreator startValuesCreator,
         IImportedQuantityToMoleculeStartValueMapper dtoMapper,
         IMoleculeStartValueBuildingBlockMergeManager startValueBuildingBlockMergeManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IReactionDimensionRetriever dimensionRetriever,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory, IMoleculeStartValuePathTask moleculeStartValuePathTask, IMoleculeResolver moleculeResolver)
         : base(interactionTaskContext, editTask, startValueBuildingBlockMergeManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoMapper, moleculeStartValuePathTask)
      {
         _startValuesCreator = startValuesCreator;
         _dimensionRetriever = dimensionRetriever;
         _moleculeResolver = moleculeResolver;
      }

      public override void ExtendStartValues(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock)
      {
         var newStartValues = createStartValuesBasedOnUsedTemplates(moleculeStartValuesBuildingBlock);
         updateDefaultIsPresentToFalseForSpecificExtendedValues(newStartValues, moleculeStartValuesBuildingBlock.ToCache());
         AddCommand(Extend(newStartValues, moleculeStartValuesBuildingBlock));
      }

      private MoleculeStartValuesBuildingBlock createStartValuesBasedOnUsedTemplates(MoleculeStartValuesBuildingBlock moleculeStartValues)
      {
         var molecules = BuildingBlockById<MoleculeBuildingBlock>(moleculeStartValues.MoleculeBuildingBlockId);
         var spatialStructure = BuildingBlockById<SpatialStructure>(moleculeStartValues.SpatialStructureId);
         return _startValuesCreator.CreateFrom(spatialStructure, molecules);
      }

      public override MoleculeStartValuesBuildingBlock CreateStartValuesForSimulation(SimulationConfiguration simulationConfiguration)
      {
         //TODO OSMOSES combining multiple spatial structures and molecule building blocks is not supported yet
         var simulationStartValues = _startValuesCreator.CreateFrom(simulationConfiguration.All<SpatialStructure>().First(), simulationConfiguration.All<MoleculeBuildingBlock>().First())
            .WithName(simulationConfiguration.All<MoleculeStartValuesBuildingBlock>().First().Name);
         
         var templateValues = UpdateValuesFromTemplate(simulationStartValues, simulationConfiguration.All<MoleculeStartValuesBuildingBlock>().First());
         updateDefaultIsPresentToFalseForSpecificExtendedValues(simulationStartValues, templateValues);
         return simulationStartValues;
      }

      private void updateDefaultIsPresentToFalseForSpecificExtendedValues(MoleculeStartValuesBuildingBlock startValues, ICache<string, MoleculeStartValue> templateValues)
      {
         var startValuesThatShouldPotentiallyNotBePresent = startValues.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
         var extendedStartValuesThatShouldNotBePresent = startValuesThatShouldPotentiallyNotBePresent.Where(x => !templateValues.Contains(x.Key));
         extendedStartValuesThatShouldNotBePresent.Each(x => x.Value.IsPresent = false);
      }

      public IMoBiCommand SetIsPresent(MoleculeStartValuesBuildingBlock moleculeStartValues, IEnumerable<MoleculeStartValue> startValues, bool isPresent)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingIsPresentCommandDescription(isPresent),
            ObjectType = ObjectTypes.MoleculeStartValue,
         };

         startValues.Where(x => x.IsPresent != isPresent).Each(msv =>
            macroCommand.Add(updateIsPresent(moleculeStartValues, msv, isPresent)));

         return macroCommand;
      }

      private IMoBiCommand updateIsPresent(MoleculeStartValuesBuildingBlock moleculeStartValues, MoleculeStartValue msv, bool isPresent)
      {
         return new UpdateMoleculeStartValueIsPresentCommand(moleculeStartValues, msv, isPresent).Run(Context);
      }

      public IMoBiCommand SetNegativeValuesAllowed(MoleculeStartValuesBuildingBlock moleculeStartValues, IEnumerable<MoleculeStartValue> startValues, bool negativeValuesAllowed)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingNegativeValuesAllowedCommandDescription(negativeValuesAllowed),
            ObjectType = ObjectTypes.MoleculeStartValue,
         };

         startValues.Where(x => x.NegativeValuesAllowed != negativeValuesAllowed).Each(msv =>
            macroCommand.Add(updateNegativeValuesAllowed(moleculeStartValues, msv, negativeValuesAllowed)));

         return macroCommand;
      }

      private IMoBiCommand updateNegativeValuesAllowed(MoleculeStartValuesBuildingBlock moleculeStartValues, MoleculeStartValue msv, bool negativeValuesAllowed)
      {
         return new UpdateMoleculeStartValueNegativeValuesAllowedCommand(moleculeStartValues, msv, negativeValuesAllowed).Run(Context);
      }

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(MoleculeStartValuesBuildingBlock buildingBlock, MoleculeStartValue moleculeStartValue)
      {
         return AddNewFormulaAtBuildingBlock(buildingBlock, moleculeStartValue, referenceParameter: null);
      }

      public override IMoBiCommand AddStartValueToBuildingBlock(MoleculeStartValuesBuildingBlock buildingBlock, MoleculeStartValue moleculeStartValue)
      {
         return GenerateAddCommand(buildingBlock, moleculeStartValue).Run(Context);
      }

      public override IMoBiCommand ImportStartValuesToBuildingBlock(MoleculeStartValuesBuildingBlock startValuesBuildingBlock, IEnumerable<ImportedQuantityDTO> startValues)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportMoleculeStartValues,
            ObjectType = ObjectTypes.MoleculeStartValue
         };

         GetImportStartValuesMacroCommand(startValuesBuildingBlock, startValues, macroCommand);

         return macroCommand.Run(Context);
      }

      public override IMoBiCommand RemoveStartValueFromBuildingBlockCommand(MoleculeStartValue startValue, MoleculeStartValuesBuildingBlock buildingBlock)
      {
         return new RemoveMoleculeStartValueFromBuildingBlockCommand(buildingBlock, startValue.Path);
      }

      public override IMoBiCommand RefreshStartValuesFromBuildingBlocks(MoleculeStartValuesBuildingBlock buildingBlock, IEnumerable<MoleculeStartValue> startValuesToRefresh)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshStartValuesFromBuildingBlocks,
            ObjectType = ObjectTypes.MoleculeStartValue
         };

         startValuesToRefresh.Each(startValue =>
         {
            var moleculeBuilder = _moleculeResolver.Resolve(startValue.ContainerPath, startValue.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock));
            if (moleculeBuilder == null) return;

            var originalStartValue = moleculeBuilder.GetDefaultMoleculeStartValue();
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

      public IMoBiCommand UpdateStartValueScaleDivisor(MoleculeStartValuesBuildingBlock buildingBlock, MoleculeStartValue startValue, double newScaleDivisor, double oldScaleDivisor)
      {
         return new UpdateMoleculeStartValueScaleDivisorCommand(buildingBlock, startValue, newScaleDivisor, oldScaleDivisor).Run(Context);
      }

      protected override SpatialStructure SpatialStructureReferencedBy(MoleculeStartValuesBuildingBlock buildingBlock)
      {
         return Context.Get<SpatialStructure>(buildingBlock.SpatialStructureId) ?? _spatialStructureFactory.Create();
      }

      protected override MoleculeBuildingBlock MoleculeBuildingBlockReferencedBy(MoleculeStartValuesBuildingBlock buildingBlock)
      {
         return Context.Get<MoleculeBuildingBlock>(buildingBlock.MoleculeBuildingBlockId) ?? Context.Create<MoleculeBuildingBlock>(buildingBlock.MoleculeBuildingBlockId);
      }

      public override bool IsEquivalentToOriginal(MoleculeStartValue startValue, MoleculeStartValuesBuildingBlock buildingBlock)
      {
         var moleculeBuilder = _moleculeResolver.Resolve(startValue.ContainerPath, startValue.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock));

         if (moleculeBuilder == null)
            return false;

         return HasEquivalentDimension(startValue, moleculeBuilder) &&
                HasEquivalentFormula(startValue, moleculeBuilder.DefaultStartFormula) &&
                HasEquivalentStartValue(startValue, moleculeBuilder.GetDefaultMoleculeStartValue());
      }

      public override IDimension GetDefaultDimension()
      {
         return _dimensionRetriever.MoleculeDimension;
      }

      public override bool CanResolve(MoleculeStartValuesBuildingBlock buildingBlock, MoleculeStartValue startValue)
      {
         return _moleculeResolver.Resolve(startValue.ContainerPath, startValue.MoleculeName, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock)) != null;
      }

      protected override IMoBiCommand GetUpdateStartValueInBuildingBlockCommand(MoleculeStartValuesBuildingBlock startValuesBuildingBlock, ImportedQuantityDTO dto)
      {
         var scaleDisivor = dto.IsScaleDivisorSpecified ? dto.ScaleDivisor : startValuesBuildingBlock[dto.Path].ScaleDivisor;
         var startValue = dto.IsQuantitySpecified ? dto.QuantityInBaseUnit : startValuesBuildingBlock[dto.Path].Value;

         return new UpdateMoleculeStartValueInBuildingBlockCommand(startValuesBuildingBlock, dto.Path, startValue, dto.IsPresent, scaleDisivor, dto.NegativeValuesAllowed);
      }

      protected override IMoBiCommand GenerateAddCommand(MoleculeStartValuesBuildingBlock targetBuildingBlock, MoleculeStartValue startValueToAdd)
      {
         return new AddMoleculeStartValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }

      protected override bool AreEquivalentItems(MoleculeStartValue first, MoleculeStartValue second)
      {
         return first.IsEquivalentTo(second);
      }

      protected override IMoBiCommand GenerateRemoveCommand(MoleculeStartValuesBuildingBlock targetBuildingBlock, MoleculeStartValue startValueToRemove)
      {
         return new RemoveMoleculeStartValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }
   }
}