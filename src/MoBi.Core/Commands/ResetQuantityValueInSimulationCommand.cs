using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class ResetQuantityValueInSimulationCommand : SetQuantityPropertyInSimulationCommandBase<IQuantity>
   {
      private double _oldValue;

      public ResetQuantityValueInSimulationCommand(IQuantity quantity, IMoBiSimulation simulation)
         : base(quantity, simulation)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         //inverse of a reset command set the previous value back into the quantity
         return new SetQuantityValueInSimulationCommand(_quantity, _oldValue, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldValue = _quantity.Value;
         _quantity.IsFixedValue = false;
         var unitName = _quantity.DisplayUnit.Name;
         var newDisplayValue = _quantity.ValueInDisplayUnit;
         var oldDisplayValue = _quantity.ConvertToDisplayUnit(_oldValue);
         Description = AppConstants.Commands.SetQuantityValueInSimulation(ObjectType, newDisplayValue, unitName, oldDisplayValue, unitName, _quantity.EntityPath(), _simulation.Name);
      }
   }

}