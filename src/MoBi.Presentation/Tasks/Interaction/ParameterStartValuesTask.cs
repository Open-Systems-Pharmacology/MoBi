using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterStartValuesTask : IStartValuesTask<IParameterStartValuesBuildingBlock, IParameterStartValue>
   {
      IParameter GetpossibleParameterFromProject(IObjectPath parameterPath);

      /// <summary>
      ///    Adds a new formula to the building block formula cache and assigns it to the start value
      /// </summary>
      /// <typeparam name="T">The type of formula being created</typeparam>
      /// <param name="buildingBlock">The building block that has the formula added and contains the start value</param>
      /// <param name="parameterStartValue">the start value being updated with a new formula</param>
      /// <returns>The command used to modify the building block and start value</returns>
      ICommand<IMoBiContext> AddNewFormulaAtParameterStartValueBuildingBlock<T>(IParameterStartValuesBuildingBlock buildingBlock, IParameterStartValue parameterStartValue);
   }

   public class ParameterStartValuesTask : AbstractStartValuesTask<IParameterStartValuesBuildingBlock, IParameterStartValue>, IParameterStartValuesTask
   {
      private readonly IParameterStartValuesCreator _startValuesCreator;
      private readonly IParameterResolver _parameterResolver;

      public ParameterStartValuesTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<IParameterStartValuesBuildingBlock> editTask,
         IParameterStartValuesCreator startValuesCreator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IImportedQuantityToParameterStartValueMapper dtoToQuantityToParameterStartValueMapper,
         IParameterResolver parameterResolver,
         IParameterStartValueBuildingBlockMergeManager parameterStartValueBuildingBlockMergeManager,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory, IParameterStartValuePathTask parameterStartValuePathTask)
         : base(interactionTaskContext, editTask, parameterStartValueBuildingBlockMergeManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoToQuantityToParameterStartValueMapper, parameterStartValuePathTask)
      {
         _startValuesCreator = startValuesCreator;
         _parameterResolver = parameterResolver;
      }

      private IParameterStartValuesBuildingBlock createTempStartValues(IParameterStartValuesBuildingBlock parameterStartValues)
      {
         var molecules = BuildingBlockById<IMoleculeBuildingBlock>(parameterStartValues.MoleculeBuildingBlockId);
         var spatialStructure = BuildingBlockById<ISpatialStructure>(parameterStartValues.SpatialStructureId);
         return _startValuesCreator.CreateFrom(spatialStructure, molecules);
      }

      public override void ExtendStartValues(IParameterStartValuesBuildingBlock parameterStartValues)
      {
         var newStartValues = createTempStartValues(parameterStartValues);
         AddCommand(Extend(newStartValues, parameterStartValues));
      }

      public override IParameterStartValuesBuildingBlock CreateStartValuesForSimulation(IMoBiBuildConfiguration buildConfiguration)
      {
         var startValues = _startValuesCreator.CreateFrom(buildConfiguration.SpatialStructureInfo.TemplateBuildingBlock,
            buildConfiguration.MoleculesInfo.TemplateBuildingBlock)
            .WithName(buildConfiguration.ParameterStartValuesInfo.TemplateBuildingBlock.Name);

         UpdateValuesFromTemplate(startValues, buildConfiguration.ParameterStartValuesInfo);

         return startValues;
      }

      public override IMoBiCommand AddStartValueToBuildingBlock(IParameterStartValuesBuildingBlock buildingBlock, IParameterStartValue startValue)
      {
         return GenerateAddCommand(buildingBlock, startValue).Run(Context);
      }

      protected override IMoBiCommand GetUpdateStartValueInBuildingBlockCommand(IParameterStartValuesBuildingBlock startValuesBuildingBlock, ImportedQuantityDTO dto)
      {
         return new UpdateParameterStartValueInBuildingBlockCommand(startValuesBuildingBlock, dto.Path, dto.QuantityInBaseUnit);
      }

      public IParameter GetpossibleParameterFromProject(IObjectPath parameterPath)
      {
         foreach (var topContainer in Context.CurrentProject.SpatialStructureCollection.SelectMany(spatialStructure => spatialStructure.TopContainers))
         {
            bool found;
            var parameter = parameterPath.TryResolve<IParameter>(topContainer, out found);
            if (parameter != null)
               return parameter;
         }
         return null;
      }

      public ICommand<IMoBiContext> AddNewFormulaAtParameterStartValueBuildingBlock<T>(IParameterStartValuesBuildingBlock buildingBlock, IParameterStartValue parameterStartValue)
      {
         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = _interactionTaskContext.GetTypeFor(parameterStartValue),
            Description = AppConstants.Commands.SetStartValueAndFormula
         };

         var parameter = GetpossibleParameterFromProject(parameterStartValue.Path);

         macroCommand.Add(AddFormulaToFormulaCacheAndSetOnStartValue<ExplicitFormula>(buildingBlock, parameterStartValue, parameter));

         return macroCommand;
      }

      public override IMoBiCommand ImportStartValuesToBuildingBlock(IParameterStartValuesBuildingBlock startValuesBuildingBlock, IEnumerable<ImportedQuantityDTO> startValues)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportParameterStartValues,
            ObjectType = ObjectTypes.ParameterStartValue
         };

         GetImportStartValuesMacroCommand(startValuesBuildingBlock, startValues, macroCommand);

         return macroCommand.Run(Context);
      }

      public override IMoBiCommand RemoveStartValueFromBuildingBlockCommand(IParameterStartValue startValue, IParameterStartValuesBuildingBlock buildingBlock)
      {
         return new RemoveParameterStartValueFromBuildingBlockCommand(buildingBlock, startValue.Path);
      }

      public override bool IsEquivalentToOriginal(IParameterStartValue startValue, IParameterStartValuesBuildingBlock buildingBlock)
      {
         var spatialStructure = SpatialStructureReferencedBy(buildingBlock);
         var moleculeBuildingBlock = MoleculeBuildingBlockReferencedBy(buildingBlock);

         var parameter = _parameterResolver.Resolve(startValue.ContainerPath, startValue.Name, spatialStructure, moleculeBuildingBlock);

         if (parameter == null)
            return false;

         return HasEquivalentDimension(startValue, parameter) &&
                HasEquivalentFormula(startValue, parameter.Formula) &&
                HasEquivalentStartValue(startValue, parameter.Value);
      }

      public override IDimension GetDefaultDimension()
      {
         return Constants.Dimension.NO_DIMENSION;
      }

      public override bool CanResolve(IParameterStartValuesBuildingBlock buildingBlock, IParameterStartValue startValue)
      {
         return _parameterResolver.Resolve(startValue.ContainerPath, startValue.Name, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock)) != null;
      }

      public override IMoBiCommand RefreshStartValuesFromBuildingBlocks(IParameterStartValuesBuildingBlock buildingBlock, IEnumerable<IParameterStartValue> startValuesToRefresh)
      {
         var spatialStructure = SpatialStructureReferencedBy(buildingBlock);
         var moleculeBuildingBlock = MoleculeBuildingBlockReferencedBy(buildingBlock);

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshStartValuesFromBuildingBlocks,
            ObjectType = ObjectTypes.ParameterStartValue
         };

         startValuesToRefresh.Each(startValue =>
         {
            var parameter = _parameterResolver.Resolve(startValue.ContainerPath, startValue.Name, spatialStructure, moleculeBuildingBlock);
            if (parameter == null)
               return;

            if (!HasEquivalentDimension(startValue, parameter))
               macroCommand.Add(UpdateStartValueDimension(buildingBlock, startValue, parameter.Dimension));

            if (!HasEquivalentStartValue(startValue, parameter.Value))
               macroCommand.Add(SetStartDisplayValueWithUnit(startValue, parameter.ConvertToDisplayUnit(parameter.Value), parameter.DisplayUnit, buildingBlock));

            // Evaluating the startValue before the formula is important if the startValue is a constant and the original building block uses a constantformula
            if (!HasEquivalentFormula(startValue, parameter.Formula))
               macroCommand.Add(ChangeStartValueFormulaCommand(buildingBlock, startValue, parameter.Formula.IsConstant() ? null : _cloneManagerForBuildingBlock.Clone(parameter.Formula, buildingBlock.FormulaCache)));
         });

         return macroCommand;
      }

      protected override bool AreEquivalentItems(IParameterStartValue first, IParameterStartValue second)
      {
         return first.IsEquivalentTo(second);
      }

      protected override IMoBiCommand GenerateRemoveCommand(IParameterStartValuesBuildingBlock targetBuildingBlock, IParameterStartValue startValueToRemove)
      {
         return new RemoveParameterStartValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IMoBiCommand GenerateAddCommand(IParameterStartValuesBuildingBlock targetBuildingBlock, IParameterStartValue startValueToAdd)
      {
         return new AddParameterStartValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }
   }
}