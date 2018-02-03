using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetConstantFormulaValueCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private ConstantFormula _constantFormula;
      private readonly double _newValue;
      private readonly double _oldValue;
      private readonly string _ownerId;
      private IEntity _owner;
      private readonly Unit _displayUnit;
      private readonly Unit _oldDisplayUnit;
      protected string FormulaId { get; private set; }

      /// <summary>
      /// Sets a constant value into a constant value formula
      /// </summary>
      /// <param name="constantFormula">The constant formula being edited</param>
      /// <param name="newValue">The new value being applied to the formula</param>
      /// <param name="displayUnit">The new value new display unit</param>
      /// <param name="oldUnit">The old value display unit</param>
      /// <param name="buildingBlock">The building block that this formula is a member of</param>
      /// <param name="formulaOwner">The entity that owns the formula</param>
      public SetConstantFormulaValueCommand(ConstantFormula constantFormula, double newValue, Unit displayUnit, Unit oldUnit, IBuildingBlock buildingBlock, IEntity formulaOwner):base(buildingBlock)
      {
         _constantFormula = constantFormula;
         _newValue = newValue;
         _oldValue = _constantFormula.Value;
         FormulaId = _constantFormula.Id;
         ObjectType = new ObjectTypeResolver().TypeFor(formulaOwner);
         CommandType = AppConstants.Commands.EditCommand;
         _ownerId = formulaOwner.Id;
         _owner = formulaOwner;
         _displayUnit = displayUnit;
         _oldDisplayUnit = oldUnit;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetConstantFormulaValueCommand(_constantFormula, _oldValue, _oldDisplayUnit, _displayUnit, _buildingBlock, _owner).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _constantFormula = null;
         _owner = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);

         Description = AppConstants.Commands.SetConstantValueFormula(
            ObjectType, 
            _constantFormula, 
            formatForDisplay(_newValue, _displayUnit), 
            formatForDisplay(_constantFormula.Value, _oldDisplayUnit),
            _owner == null? string.Empty : _owner.EntityPath());

         _constantFormula.Value = _newValue;
      }

      private string formatForDisplay(double newValue, Unit displayUnit)
      {
         return $"{_constantFormula.ConvertToUnit(newValue, displayUnit)} {displayUnit}";
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _constantFormula = context.Get<ConstantFormula>(FormulaId);
         _owner = context.Get<IEntity>(_ownerId);
      }
   }
}