using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SetQuantityUnitInSimulationCommand : SetQuantityPropertyInSimulationCommandBase<IQuantity>
   {
      private readonly Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;

      public SetQuantityUnitInSimulationCommand(IQuantity quantity, Unit newDisplayUnit, IMoBiSimulation simulation)
         : base(quantity, simulation)
      {
         _newDisplayUnit = newDisplayUnit;
         _oldDisplayUnit = _quantity.DisplayUnit;
      }

      protected override void DoExecute(IMoBiContext context)
      {
         double oldDisplayValue = _quantity.Dimension.BaseUnitValueToUnitValue(_oldDisplayUnit, _quantity.Value);
         //settings display unit should be done before updating value to ensure that all event triggered by value changes are using the accurate display unit
         _quantity.DisplayUnit = _newDisplayUnit;
         _quantity.Value = _quantity.Dimension.UnitValueToBaseUnitValue(_newDisplayUnit, oldDisplayValue);
         Description = AppConstants.Commands.SetQuantityValueInSimulation(ObjectType, _quantity.ValueInDisplayUnit, _newDisplayUnit.Name, oldDisplayValue, _oldDisplayUnit.Name, _quantity.EntityPath(), _simulation.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetQuantityUnitInSimulationCommand(_quantity, _oldDisplayUnit, _simulation).AsInverseFor(this);
      }
   }
}