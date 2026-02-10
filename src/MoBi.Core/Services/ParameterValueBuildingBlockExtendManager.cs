using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using System.Collections.Generic;

namespace MoBi.Core.Services;

public interface IParameterValueBuildingBlockExtendManager : IExtendPathAndValuesManager<ParameterValue>
{
}

public class ParameterValueBuildingBlockExtendManager : ExtendPathAndValuesManager<ParameterValue>, IParameterValueBuildingBlockExtendManager
{
   private readonly IParameterValuesCreator _parameterValuesCreator;

   public ParameterValueBuildingBlockExtendManager(IParameterValuesCreator parameterValuesCreator, IMoBiFormulaTask moBiFormulaTask, IObjectTypeResolver objectTypeResolver, IMoBiContext moBiContext) : base(moBiFormulaTask, objectTypeResolver, moBiContext)
   {
      _parameterValuesCreator = parameterValuesCreator;
   }

   protected override IReadOnlyList<ParameterValue> CreatePathAndValueEntitiesBasedOnUsedTemplates(SpatialStructure spatialStructure, IReadOnlyList<MoleculeBuilder> molecules, ILookupBuildingBlock<ParameterValue> buildingBlock) => 
      _parameterValuesCreator.CreateFrom(spatialStructure, molecules);

   protected override IMoBiCommand GenerateRemoveCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToRemove) => new RemoveParameterValueFromBuildingBlockCommand(targetBuildingBlock, startValueToRemove.Path);

   public override IMoBiCommand GenerateAddCommand(ILookupBuildingBlock<ParameterValue> targetBuildingBlock, ParameterValue startValueToAdd) => new AddParameterValueToBuildingBlockCommand(targetBuildingBlock, startValueToAdd);
}