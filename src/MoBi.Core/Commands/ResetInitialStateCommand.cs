using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands;

public class ResetInitialStateCommand<TBuilder, TBuildingBlock> : PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock> where TBuilder : ParameterValueWithInitialState where TBuildingBlock : class, IBuildingBlock<TBuilder>
{
   private readonly double? _oldInitialValue;
   private readonly Unit _oldInitialUnit;
   private readonly string _oldInitialFormulaId;

   private readonly double? _newInitialValue;
   private readonly Unit _newInitialUnit;
   private readonly string _newInitialFormulaId;
   private readonly string _newFormulaId;
   private readonly string _oldFormulaId;

   public ResetInitialStateCommand(TBuilder builder, TBuildingBlock buildingBlock) : this(builder, null, null, null, builder.InitialValue, builder.InitialFormulaId, builder.InitialUnit, builder.InitialValue, builder.InitialUnit, builder.InitialFormulaId, builder.Formula?.Id, buildingBlock)
   {

   }

   // Only for use in restoring previous state after a reset
   private ResetInitialStateCommand(TBuilder builder, double? newInitialValue, string newInitialFormulaId, Unit newInitialUnit, double? oldInitialValue, string oldInitialFormulaId, Unit oldInitialUnit, double? newValue, Unit newUnit, string newFormulaId, string oldFormulaId, TBuildingBlock buildingBlock) : base(builder, newValue, newUnit, buildingBlock)
   {
      _oldInitialValue = oldInitialValue;
      _oldInitialUnit = oldInitialUnit;
      _oldInitialFormulaId = oldInitialFormulaId;

      _newInitialFormulaId = newInitialFormulaId;
      _newInitialUnit = newInitialUnit;
      _newInitialValue = newInitialValue;

      _newFormulaId = newFormulaId;
      _oldFormulaId = oldFormulaId;
   }

   protected override void ExecuteWith(IMoBiContext context)
   {
      base.ExecuteWith(context);

      _builder.Formula = _buildingBlock.FormulaCache.Contains(_newFormulaId) ? _buildingBlock.FormulaCache[_newFormulaId] : null;

      _builder.InitialValue = _newInitialValue;
      _builder.InitialFormulaId = _newInitialFormulaId;
      _builder.InitialUnit = _newInitialUnit;
   }

   protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
   {
      return new ResetInitialStateCommand<TBuilder, TBuildingBlock>(_builder, _oldInitialValue, _oldInitialFormulaId, _oldInitialUnit, _newInitialValue, _newInitialFormulaId, _newInitialUnit, _oldBaseValue, _oldDisplayUnit, _oldFormulaId, _newFormulaId, _buildingBlock);
   }
}