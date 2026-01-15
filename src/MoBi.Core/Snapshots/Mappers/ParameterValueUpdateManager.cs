using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Snapshots.Mappers;

public interface IParameterValueUpdateManager : IMapper<ParameterValueWithInitialState, UpdatedParameterValue>
{
   void UpdateParameterValueIn<TBuildingBlock, TParameterValue>(TBuildingBlock buildingBlock, UpdatedParameterValue updatedParameterValue) where TBuildingBlock : PathAndValueEntityBuildingBlock<TParameterValue> where TParameterValue : ParameterValueWithInitialState;
}

public class ParameterValueUpdateManager : IParameterValueUpdateManager
{
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

   public void UpdateParameterValueIn<TBuildingBlock, TParameterValue>(TBuildingBlock buildingBlock, UpdatedParameterValue updatedParameterValue) where TBuildingBlock : PathAndValueEntityBuildingBlock<TParameterValue> where TParameterValue : ParameterValueWithInitialState
   {
      var parameterValue = buildingBlock.FindByPath(updatedParameterValue.Path);

      if (parameterValue == null)
         return;

      parameterValue.InitialValue = parameterValue.Value;
      parameterValue.InitialFormulaId = parameterValue.Formula?.Id;
      parameterValue.InitialUnit = parameterValue.DisplayUnit;

      parameterValue.Value = updatedParameterValue.NewValue;
      parameterValue.DisplayUnit = parameterValue.Dimension.Unit(updatedParameterValue.NewUnit);
      parameterValue.Formula = buildingBlock.FormulaCache.Contains(updatedParameterValue.NewFormulaId) ? buildingBlock.FormulaCache[updatedParameterValue.NewFormulaId] : null;
   }
}