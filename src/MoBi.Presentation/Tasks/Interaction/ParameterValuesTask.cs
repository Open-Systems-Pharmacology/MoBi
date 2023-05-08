using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
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
   public interface IParameterValuesTask : IStartValuesTask<ParameterValuesBuildingBlock, ParameterValue>
   {
      IParameter GetPossibleParameterFromProject(ObjectPath parameterPath);
   }

   public class ParameterValuesTask : AbstractStartValuesTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuesTask
   {
      private readonly IParameterValuesCreator _startValuesCreator;
      private readonly IParameterResolver _parameterResolver;

      public ParameterValuesTask(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<ParameterValuesBuildingBlock> editTask,
         IParameterValuesCreator startValuesCreator,
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IImportedQuantityToParameterValueMapper dtoToQuantityToParameterValueMapper,
         IParameterResolver parameterResolver,
         IParameterValueBuildingBlockExtendManager parameterValuesExtendManager,
         IMoBiFormulaTask moBiFormulaTask,
         IMoBiSpatialStructureFactory spatialStructureFactory, IParameterValuePathTask parameterValuePathTask)
         : base(interactionTaskContext, editTask, parameterValuesExtendManager, cloneManagerForBuildingBlock, moBiFormulaTask, spatialStructureFactory, dtoToQuantityToParameterValueMapper, parameterValuePathTask)
      {
         _startValuesCreator = startValuesCreator;
         _parameterResolver = parameterResolver;
      }

      private ParameterValuesBuildingBlock createTempStartValues(ParameterValuesBuildingBlock parameterValues)
      {
         return new ParameterValuesBuildingBlock();
         // var molecules = BuildingBlockById<MoleculeBuildingBlock>(parameterStartValues.MoleculeBuildingBlockId);
         // var spatialStructure = BuildingBlockById<SpatialStructure>(parameterStartValues.SpatialStructureId);
         // return _startValuesCreator.CreateFrom(spatialStructure, molecules);
      }

      public override void ExtendStartValues(ParameterValuesBuildingBlock parameterValuesBuildingBlock)
      {
         var newStartValues = createTempStartValues(parameterValuesBuildingBlock);
         AddCommand(Extend(newStartValues, parameterValuesBuildingBlock));
      }

      public override ParameterValuesBuildingBlock CreateStartValuesForSimulation(SimulationConfiguration simulationConfiguration)
      {
         //TODO OSMOSES 
         return new ParameterValuesBuildingBlock();
         // return _cloneManagerForBuildingBlock.Clone(simulationConfiguration.ParameterStartValues);
      }

      public override IMoBiCommand AddStartValueToBuildingBlock(ParameterValuesBuildingBlock buildingBlock, ParameterValue startValue)
      {
         return GenerateAddCommand(buildingBlock, startValue).Run(Context);
      }

      protected override IMoBiCommand GetUpdateStartValueInBuildingBlockCommand(ParameterValuesBuildingBlock startValuesBuildingBlock, ImportedQuantityDTO dto)
      {
         return new UpdateParameterValueInBuildingBlockCommand(startValuesBuildingBlock, dto.Path, dto.QuantityInBaseUnit);
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

      public ICommand<IMoBiContext> AddNewFormulaAtBuildingBlock(ParameterValuesBuildingBlock buildingBlock, ParameterValue parameterValue)
      {
         var parameter = GetPossibleParameterFromProject(parameterValue.Path);

         return AddNewFormulaAtBuildingBlock(buildingBlock, parameterValue, parameter);
      }

      public override IMoBiCommand ImportStartValuesToBuildingBlock(ParameterValuesBuildingBlock startValuesBuildingBlock, IEnumerable<ImportedQuantityDTO> startValues)
      {
         var macroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.ImportCommand,
            Description = AppConstants.Commands.ImportParameterValues,
            ObjectType = ObjectTypes.ParameterValue
         };

         GetImportStartValuesMacroCommand(startValuesBuildingBlock, startValues, macroCommand);

         return macroCommand.Run(Context);
      }

      public override IMoBiCommand RemoveStartValueFromBuildingBlockCommand(ParameterValue startValue, ParameterValuesBuildingBlock buildingBlock)
      {
         return new RemoveParameterValueFromBuildingBlockCommand(buildingBlock, startValue.Path);
      }

      protected override MoleculeBuildingBlock MoleculeBuildingBlockReferencedBy(ParameterValuesBuildingBlock buildingBlock)
      {
         return new MoleculeBuildingBlock();
      }

      protected override SpatialStructure SpatialStructureReferencedBy(ParameterValuesBuildingBlock buildingBlock)
      {
         return new MoBiSpatialStructure();
      }

      public override bool IsEquivalentToOriginal(ParameterValue startValue, ParameterValuesBuildingBlock buildingBlock)
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

      public override bool CanResolve(ParameterValuesBuildingBlock buildingBlock, ParameterValue startValue)
      {
         return _parameterResolver.Resolve(startValue.ContainerPath, startValue.Name, SpatialStructureReferencedBy(buildingBlock), MoleculeBuildingBlockReferencedBy(buildingBlock)) != null;
      }

      public override IMoBiCommand RefreshStartValuesFromBuildingBlocks(ParameterValuesBuildingBlock buildingBlock, IEnumerable<ParameterValue> startValuesToRefresh)
      {
         var spatialStructure = SpatialStructureReferencedBy(buildingBlock);
         var moleculeBuildingBlock = MoleculeBuildingBlockReferencedBy(buildingBlock);

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.RefreshStartValuesFromBuildingBlocks,
            ObjectType = ObjectTypes.ParameterValue
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


      protected override IMoBiCommand GenerateRemoveCommand(ParameterValuesBuildingBlock targetBuildingBlock, ParameterValue startValueToRemove)
      {
         return new RemoveParameterValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);
      }

      protected override IMoBiCommand GenerateAddCommand(ParameterValuesBuildingBlock targetBuildingBlock, ParameterValue startValueToAdd)
      {
         return new AddParameterValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
      }

      protected override IReadOnlyCollection<IObjectBase> GetNamedObjectsInParent(ParameterValuesBuildingBlock buildingBlockToClone)
      {
         return buildingBlockToClone.Module.ParameterValuesCollection;
      }
   }
}