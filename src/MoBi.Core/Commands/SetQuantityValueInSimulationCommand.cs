using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public class SetQuantityValueInSimulationCommand : SetQuantityPropertyInSimulationCommandBase<IQuantity>
   {
      private readonly double _valueToSet;
      private double _oldValue;
      private bool _fixedValueSetHere;

      public SetQuantityValueInSimulationCommand(IQuantity quantity, double valueToSet, IMoBiSimulation simulation)
         : base(quantity, simulation)
      {
         _valueToSet = valueToSet;
         CommandType = AppConstants.Commands.EditCommand;
         var unitName = quantity.DisplayUnit.Name;
         var newDisplayValue = quantity.ConvertToDisplayUnit(valueToSet);
         var oldDisplayValue = quantity.ValueInDisplayUnit;
         Description = AppConstants.Commands.SetQuantityValueInSimulation(ObjectType, newDisplayValue, unitName, oldDisplayValue, unitName, _quantity.EntityPath(), _simulation.Name);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldValue = _quantity.Value;
         //this is an undo scenario
         if (_fixedValueSetHere)
            resetQuantity();
         else
         {
            _quantity.Value = _valueToSet;
            _fixedValueSetHere = true;

            if (formulaIsConstantEqualsToTheNewValue)
               resetQuantity();
         }
      }

      private void resetQuantity()
      {
         _quantity.IsFixedValue = false;
         _fixedValueSetHere = false;
      }

      private bool formulaIsConstantEqualsToTheNewValue
      {
         get
         {
            var constantFormula = _quantity.Formula as ConstantFormula;
            return constantFormula != null && ValueComparer.AreValuesEqual(constantFormula.Value, _valueToSet);
         }
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetQuantityValueInSimulationCommand(_quantity, _oldValue, _simulation)
         {
            _fixedValueSetHere = _fixedValueSetHere
         }.AsInverseFor(this);
      }
   }
}