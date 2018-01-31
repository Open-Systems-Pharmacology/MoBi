using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class SetParameterDefaultStateInSimulationCommand : SetQuantityPropertyInSimulationCommandBase<IParameter>
   {
      private readonly bool _isDefault;
      private bool _oldIsDefault;

      public SetParameterDefaultStateInSimulationCommand(IParameter quantity, bool isDefault, IMoBiSimulation simulation) : base(quantity, simulation)
      {
         _isDefault = isDefault;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SetParameterDefaultStateInSimulationCommand(_quantity, _oldIsDefault, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldIsDefault = _quantity.IsDefault;
         _quantity.IsDefault = _isDefault;
         Description = AppConstants.Commands.SetParameterDefaultStateInSimulation(_quantity.EntityPath(), _oldIsDefault, _isDefault, _simulation.Name);
      }
   }
}