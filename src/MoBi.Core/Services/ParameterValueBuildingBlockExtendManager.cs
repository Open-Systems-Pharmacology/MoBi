using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
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
         return new UpdateParameterValueInBuildingBlockCommand(buildingBlock, objectPath, valueInBaseUnit);

      var dimension = _dimensionFactory.Dimension(dimensionName);

      var initialCondition = _parameterValuesCreator.CreateParameterValue(
         objectPath,
         valueInBaseUnit,
         dimension,
         displayUnit: null,
         valueOrigin: null
      );

      return GenerateAddCommand(buildingBlock, initialCondition);
   }
}