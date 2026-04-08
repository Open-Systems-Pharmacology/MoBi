using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Services;

public interface IParameterValueBuildingBlockExtendManager : IExtendPathAndValuesManager<ParameterValue>
{
   IMoBiCommand MergeWithUpdate(ParameterValuesBuildingBlock buildingBlock, ObjectPath objectPath, double valueInBaseUnit, string dimensionName);
}

public class ParameterValueBuildingBlockExtendManager : ExtendPathAndValuesManager<ParameterValue>, IParameterValueBuildingBlockExtendManager
{
   private readonly IParameterValuesCreator _parameterValuesCreator;
   private readonly IDimensionFactory _dimensionFactory;

   public ParameterValueBuildingBlockExtendManager(IParameterValuesCreator parameterValuesCreator, IMoBiFormulaTask moBiFormulaTask, IObjectTypeResolver objectTypeResolver, IMoBiContext moBiContext, IDimensionFactory dimensionFactory) : base(moBiFormulaTask, objectTypeResolver, moBiContext)
   {
      _parameterValuesCreator = parameterValuesCreator;
      _dimensionFactory = dimensionFactory;
   }

   protected override IReadOnlyList<ParameterValue> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ILookupBuildingBlock<ParameterValue> buildingBlock) =>
      _parameterValuesCreator.CreateFrom(spatialStructure, molecules);

   protected override IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToRemove) => new RemoveParameterValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);

   public override IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToAdd) => new AddParameterValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);

   public IMoBiCommand MergeWithUpdate(ParameterValuesBuildingBlock buildingBlock, ObjectPath objectPath, double valueInBaseUnit, string dimensionName)
   {
      var pathAndValueEntity = buildingBlock[objectPath];
      if (pathAndValueEntity != null)
         return updateValueAndDimension(buildingBlock, pathAndValueEntity, objectPath, valueInBaseUnit, dimensionName);

      var dimension = _dimensionFactory.Dimension(dimensionName);

      var parameterValue = _parameterValuesCreator.CreateParameterValue(
         objectPath,
         valueInBaseUnit,
         dimension,
         displayUnit: null,
         valueOrigin: null
      );

      return GenerateAddCommand(buildingBlock, parameterValue);
   }

   private IMoBiCommand updateValueAndDimension(ParameterValuesBuildingBlock buildingBlock, ParameterValue pathAndValueEntity, ObjectPath objectPath, double valueInBaseUnit, string dimensionName)
   {
      var updateValueCommand = new UpdateParameterValueInBuildingBlockCommand(buildingBlock, objectPath, valueInBaseUnit);

      var newDimension = _dimensionFactory.Dimension(dimensionName);
      if (Equals(pathAndValueEntity.Dimension, newDimension))
         return updateValueCommand;

      var macroCommand = new MoBiMacroCommand
      {
         CommandType = AppConstants.Commands.UpdateCommand,
         ObjectType = ObjectTypes.ParameterValue
      };

      macroCommand.Add(updateValueCommand);
      macroCommand.Add(new UpdateDimensionInPathAndValueEntityCommand<ParameterValue>(pathAndValueEntity, newDimension, newDimension.DefaultUnit, buildingBlock));
      return macroCommand;
   }
}