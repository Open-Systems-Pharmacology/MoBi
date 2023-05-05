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
   public interface IMoleculeStartValuesTask : IStartValuesTask<InitialConditionsBuildingBlock, InitialCondition>
   {
      IMoBiCommand SetIsPresent(InitialConditionsBuildingBlock moleculeStartValues, IEnumerable<InitialCondition> startValues, bool isPresent);

      IMoBiCommand SetNegativeValuesAllowed(InitialConditionsBuildingBlock moleculeStartValues, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed);

      /// <summary>
      ///    Updates the scale divisor for a start value
      /// </summary>
      /// <param name="buildingBlock">The building block that this start value is part of</param>
      /// <param name="startValue">The start value being updated</param>
      /// <param name="newScaleDivisor">The new value of the scale divisor</param>
      /// <param name="oldScaleDivisor">The old value of the scale divisor</param>
      /// <returns>The command used to modify the start value</returns>
      IMoBiCommand UpdateStartValueScaleDivisor(InitialConditionsBuildingBlock buildingBlock, InitialCondition startValue, double newScaleDivisor, double oldScaleDivisor);
   }

   public class MoleculeStartValuesTask : AbstractStartValuesTask<InitialConditionsBuildingBlock, InitialCondition>, IMoleculeStartValuesTask
   {
      private readonly IInitialConditionsCreator _startValuesCreator;
      private readonly IReactionDimensionRetriever _dimensionRetriever;
      private readonly IMoleculeResolver _moleculeResolver;

      public MoleculeStartValuesTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<InitialConditionsBuildingBlock> editTask,
         IInitialConditionsCreator startValuesCreator,
         IImportedQuantityToMoleculeStartValueMapper dtoMapper,
         IMoleculeStartValueBuildingBlockExtendManager startValueBuildingBlockExtendManager,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IReactionDimensionRetriever dimensionRetriever,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory, IMoleculeStartValuePathTask moleculeStartValuePathTask, IMoleculeResolver moleculeResolver)
         : base(interactionTaskContext, editTask, startValueBuildingBlockExtendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoMapper, moleculeStartValuePathTask)
      {
         _startValuesCreator = startValuesCreator;
         _dimensionRetriever = dimensionRetriever;
         _moleculeResolver = moleculeResolver;
      }

      public override void ExtendStartValues(InitialConditionsBuildingBlock moleculeStartValuesBuildingBlock)
      {
         var newStartValues = createStartValuesBasedOnUsedTemplates(moleculeStartValuesBuildingBlock);
         updateDefaultIsPresentToFalseForSpecificExtendedValues(newStartValues, moleculeStartValuesBuildingBlock.ToCache());
         AddCommand(Extend(newStartValues, moleculeStartValuesBuildingBlock));
      }

      private InitialConditionsBuildingBlock createStartValuesBasedOnUsedTemplates(InitialConditionsBuildingBlock moleculeStartValues)
      {
         var molecules = BuildingBlockById<MoleculeBuildingBlock>(moleculeStartValues.MoleculeBuildingBlockId);
         var spatialStructure = BuildingBlockById<SpatialStructure>(moleculeStartValues.SpatialStructureId);
         return _startValuesCreator.CreateFrom(spatialStructure, molecules);
      }

      public override InitialConditionsBuildingBlock CreateStartValuesForSimulation(SimulationConfiguration simulationConfiguration)
      {
         //TODO OSMOSES combining multiple spatial structures and molecule building blocks is not supported yet
         var simulationStartValues = _startValuesCreator.CreateFrom(simulationConfiguration.All<SpatialStructure>().First(), simulationConfiguration.All<MoleculeBuildingBlock>().First())
            .WithName(simulationConfiguration.All<InitialConditionsBuildingBlock>().First().Name);
         
         var templateValues = UpdateValuesFromTemplate(simulationStartValues, simulationConfiguration.All<InitialConditionsBuildingBlock>().First());
         updateDefaultIsPresentToFalseForSpecificExtendedValues(simulationStartValues, templateValues);
         return simulationStartValues;
      }

      private void updateDefaultIsPresentToFalseForSpecificExtendedValues(InitialConditionsBuildingBlock startValues, ICache<string, InitialCondition> templateValues)
      {
         var startValuesThatShouldPotentiallyNotBePresent = startValues.ToCache().KeyValues.Where(x => AppConstants.Organs.DefaultIsPresentShouldBeFalse.Any(organ => x.Key.Contains(organ)));
         var extendedStartValuesThatShouldNotBePresent = startValuesThatShouldPotentiallyNotBePresent.Where(x => !templateValues.Contains(x.Key));
         extendedStartValuesThatShouldNotBePresent.Each(x => x.Value.IsPresent = false);
      }

      public IMoBiCommand SetIsPresent(InitialConditionsBuildingBlock moleculeStartValues, IEnumerable<InitialCondition> startValues, bool isPresent)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingIsPresentCommandDescription(isPresent),
            ObjectType = ObjectTypes.InitialCondition,
         };

         startValues.Where(x => x.IsPresent != isPresent).Each(msv =>
            macroCommand.Add(updateIsPresent(moleculeStartValues, msv, isPresent)));

         return macroCommand;
      }

      private IMoBiCommand updateIsPresent(InitialConditionsBuildingBlock moleculeStartValues, InitialCondition msv, bool isPresent)
      {
         return new UpdateMoleculeStartValueIsPresentCommand(moleculeStartValues, msv, isPresent).Run(Context);
      }

      public IMoBiCommand SetNegativeValuesAllowed(InitialConditionsBuildingBlock moleculeStartValues, IEnumerable<InitialCondition> startValues, bool negativeValuesAllowed)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.SettingNegativeValuesAllowedCommandDescription(negativeValuesAllowed),
            ObjectType = ObjectTypes.InitialCondition,
         };

         startValues.Where(x => x.NegativeValuesAllowed != negativeValuesAllowed).Each(msv =>
            macroCommand.Add(updateNegativeValuesAllowed(moleculeStartValues, msv, negativeValuesAllowed)));

         return macroCommand;
      }

      private IMoBiCommand updateNegativeValuesAllowed(InitialConditionsBuildingBlock moleculeStartValues, InitialCondition msv, bool negativeValuesAllowed)
      {
         return new UpdateMoleculeStartValueNegativeValuesAllowedCommand(moleculeStartValues, msv, negativeValuesAllowed).Run(Context);
      }

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(InitialConditionsBuildingBlock buildingBlock, InitialCondition moleculeStartValue)
      {
         return AddNewFormulaAtBuildingBlock(buildingBlock, moleculeStartValue, referenceParameter: null);
      }

      public override IMoBiCommand AddStartValueToBuildingBlock(InitialConditionsBuildingBlock buildingBlock, InitialCondition moleculeStartValue)
      {
         return GenerateAddCommand(buildingBlock, moleculeStartValue).Run(Context);
      }

      public override IMoBiCommand ImportStartValuesToBuildingBlock(InitialConditionsBuildingBlock startValuesBuildingBlock, IEnumerable<ImportedQuantityDTO> startValues)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportMoleculeStartValues,
            ObjectType = ObjectTypes.InitialCondition
         };

         GetImportStartValuesMacroCommand(startValuesBuildingBlock, startValues, macroCommand);

         return macroCommand.Run(Context);
      }

      public override IMoBiCommand RemoveStartValueFromBuildingBlockCommand(InitialCondition startValue, InitialConditionsBuildingBlock buildingBlock)
      {
         return new RemoveMoleculeStartValueFromBuildingBlockCommand(buildingBlock, startValue.Path);
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

      public IMoBiCommand UpdateStartValueScaleDivisor(InitialConditionsBuildingBlock buildingBlock, InitialCondition startValue, double newScaleDivisor, double oldScaleDivisor)
      {
         return new UpdateMoleculeStartValueScaleDivisorCommand(buildingBlock, startValue, newScaleDivisor, oldScaleDivisor).Run(Context);
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

         return new UpdateMoleculeStartValueInBuildingBlockCommand(startValuesBuildingBlock, dto.Path, startValue, dto.IsPresent, scaleDisivor, dto.NegativeValuesAllowed);
      }

      protected override IMoBiCommand GenerateAddCommand(InitialConditionsBuildingBlock targetBuildingBlock, InitialCondition startValueToAdd)
      {
         return new AddMoleculeStartValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }

      protected override IMoBiCommand GenerateRemoveCommand(InitialConditionsBuildingBlock targetBuildingBlock, InitialCondition startValueToRemove)
      {
         return new RemoveMoleculeStartValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(InitialConditionsBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.InitialConditionsCollection;
      }
   }
}