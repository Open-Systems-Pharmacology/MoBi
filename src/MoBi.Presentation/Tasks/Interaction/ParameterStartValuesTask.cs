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
   public interface IParameterStartValuesTask : IStartValuesTask<IParameterStartValuesBuildingBlock, ParameterStartValue>
   {
      IParameter GetPossibleParameterFromProject(ObjectPath parameterPath);
   }

   public class ParameterStartValuesTask : AbstractStartValuesTask<IParameterStartValuesBuildingBlock, ParameterStartValue>, IParameterStartValuesTask
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

      public override IMoBiCommand AddStartValueToBuildingBlock(IParameterStartValuesBuildingBlock buildingBlock, ParameterStartValue startValue)
      {
         return GenerateAddCommand(buildingBlock, startValue).Run(Context);
      }

      protected override IMoBiCommand GetUpdateStartValueInBuildingBlockCommand(IParameterStartValuesBuildingBlock startValuesBuildingBlock, ImportedQuantityDTO dto)
      {
         return new UpdateParameterStartValueInBuildingBlockCommand(startValuesBuildingBlock, dto.Path, dto.QuantityInBaseUnit);
      }

      public IParameter GetPossibleParameterFromProject(ObjectPath parameterPath)
      {
         foreach (var topContainer in Context.CurrentProject.SpatialStructureCollection.SelectMany(spatialStructure => spatialStructure.TopContainers))
         {
            var parameter = parameterPath.TryResolve<IParameter>(topContainer, out _);
            if (parameter != null)
               return parameter;
         }
         return null;
      }

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(IParameterStartValuesBuildingBlock buildingBlock, ParameterStartValue parameterStartValue)
      {
         var parameter = GetPossibleParameterFromProject(parameterStartValue.Path);

         return AddNewFormulaAtBuildingBlock(buildingBlock, parameterStartValue, parameter);
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

      public override IMoBiCommand RemoveStartValueFromBuildingBlockCommand(ParameterStartValue startValue, IParameterStartValuesBuildingBlock buildingBlock)
      {
         return new RemoveParameterStartValueFromBuildingBlockCommand(buildingBlock, startValue.Path);
      }

      public override bool IsEquivalentToOriginal(ParameterStartValue startValue, IParameterStartValuesBuildingBlock buildingBlock)
      {
         var spatialStructure = SpatialStructureReferencedBy(buildingBlock);
         var moleculeBuildingBlock = MoleculeBuildingBlockReferencedBy(buildingBlock);

         var parameter = _parameterResolver.Resolve(startValue.ContainerPath, startValue.Name, spatialStructure, moleculeBuildingBlock);

         if (parameter == null)
            return false;

         return HasEquivalentDimension(startValue, parameter) &&
                HasEquivalentFormula(startValue, parameter.Formula) &&
                HasEquivalentStartValue(startValue, parameter);
      }


      public override IDimension GetDefaultDimension()
      {
         return Constants.Dimension.NO_DIMENSION;
      }

      public override bool CanResolve(IParameterStartValuesBuildingBlock buildingBlock, ParameterStartValue startValue)
      {
         return _parameterResolver.Resolve(startValue.ContainerPath, startValue.Name, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock)) != null;
      }

      public override IMoBiCommand RefreshStartValuesFromBuildingBlocks(IParameterStartValuesBuildingBlock buildingBlock, IEnumerable<ParameterStartValue> startValuesToRefresh)
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

            if (!HasEquivalentStartValue(startValue, parameter))
               macroCommand.Add(SetDisplayValueWithUnit(startValue, parameter.ConvertToDisplayUnit(parameter.Value), parameter.DisplayUnit, buildingBlock));

            // Evaluating the startValue before the formula is important if the startValue is a constant and the original building block uses a constant formula
            if (!HasEquivalentFormula(startValue, parameter.Formula))
               macroCommand.Add(ChangeValueFormulaCommand(buildingBlock, startValue, parameter.Formula.IsConstant() ? null : _cloneManagerForBuildingBlock.Clone(parameter.Formula, buildingBlock.FormulaCache)));
         });

         return macroCommand;
      }

      protected override bool AreEquivalentItems(ParameterStartValue first, ParameterStartValue second)
      {
         return first.IsEquivalentTo(second);
      }

      protected override IMoBiCommand GenerateRemoveCommand(IParameterStartValuesBuildingBlock targetBuildingBlock, ParameterStartValue startValueToRemove)
      {
         return new RemoveParameterStartValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IMoBiCommand GenerateAddCommand(IParameterStartValuesBuildingBlock targetBuildingBlock, ParameterStartValue startValueToAdd)
      {
         return new AddParameterStartValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }
   }
}