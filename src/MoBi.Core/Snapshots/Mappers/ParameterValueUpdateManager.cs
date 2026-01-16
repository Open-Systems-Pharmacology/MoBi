using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Utility;

namespace MoBi.Core.Snapshots.Mappers;

public interface IParameterValueUpdateManager : IMapper<ParameterValueWithInitialState, UpdatedParameterValue>
{
   void UpdateParameterValueIn<TBuildingBlock, TParameterValue>(TBuildingBlock buildingBlock, UpdatedParameterValue updatedParameterValue, FormulaCache formulaCache) where TBuildingBlock : PathAndValueEntityBuildingBlock<TParameterValue> where TParameterValue : ParameterValueWithInitialState;
}

public class ParameterValueUpdateManager : IParameterValueUpdateManager
{
   private readonly IOSPSuiteLogger _logger;

   public ParameterValueUpdateManager(IOSPSuiteLogger logger)
   {
      _logger = logger;
   }

   public UpdatedParameterValue MapFrom(ParameterValueWithInitialState source)
   {
      return new UpdatedParameterValue
      {
         NewUnit = source.DisplayUnitName(),
         NewValue = source.Value,
         NewFormulaId = source.Formula?.Id,
         Path = source.Path
      };
   }

   public void UpdateParameterValueIn<TBuildingBlock, TParameterValue>(TBuildingBlock buildingBlock, UpdatedParameterValue updatedParameterValue, FormulaCache formulaCache) where TBuildingBlock : PathAndValueEntityBuildingBlock<TParameterValue> where TParameterValue : ParameterValueWithInitialState
   {
      var parameterValue = buildingBlock.FindByPath(updatedParameterValue.Path);

      if (parameterValue == null)
      {
         _logger.AddWarning($"Could not find parameter value with path '{updatedParameterValue.Path}' in building block '{buildingBlock.Name}'");
         return;
      }

      parameterValue.InitialValue = parameterValue.Value;
      parameterValue.InitialFormulaId = parameterValue.Formula?.Id;
      parameterValue.InitialUnit = parameterValue.DisplayUnit;

      parameterValue.Value = updatedParameterValue.NewValue;
      parameterValue.DisplayUnit = parameterValue.Dimension.Unit(updatedParameterValue.NewUnit);
      parameterValue.Formula = formulaFrom<TBuildingBlock, TParameterValue>(buildingBlock, updatedParameterValue, formulaCache);
   }

   /// <summary>
   ///    Retrieves the <paramref name="updatedParameterValue" /> formula from the <paramref name="buildingBlock" /> formula
   ///    cache if present. If not present, retrieves it from the <paramref name="formulaCache" />
   ///    and adds it to the building block formula cache. If not found in either, returns null.
   /// </summary>
   private IFormula formulaFrom<TBuildingBlock, TParameterValue>(TBuildingBlock buildingBlock, UpdatedParameterValue updatedParameterValue, FormulaCache formulaCache) where TBuildingBlock : PathAndValueEntityBuildingBlock<TParameterValue> where TParameterValue : ParameterValueWithInitialState
   {
      var newFormulaId = updatedParameterValue.NewFormulaId;

      if (newFormulaId == null)
         return null;

      if (formulaCache != null && formulaCache.Contains(newFormulaId) && !buildingBlock.FormulaCache.Contains(newFormulaId))
         buildingBlock.FormulaCache.Add(formulaCache[newFormulaId]);

      if (!buildingBlock.FormulaCache.Contains(newFormulaId))
      {
         _logger.AddWarning($"Could not find formula with Id '{updatedParameterValue.NewFormulaId}' in the building block '{buildingBlock.Name}'");
         return null;
      }
      return buildingBlock.FormulaCache[newFormulaId];
   }
}