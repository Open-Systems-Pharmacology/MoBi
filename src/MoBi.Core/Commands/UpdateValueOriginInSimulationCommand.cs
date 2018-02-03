using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class UpdateValueOriginInSimulationCommand : SetQuantityPropertyInSimulationCommandBase<IParameter>
   {
      private ValueOrigin _valueOrigin;
      private ValueOrigin _oldValueOrigin;

      public UpdateValueOriginInSimulationCommand(IParameter quantity, ValueOrigin valueOrigin, IMoBiSimulation simulation) : base(quantity, simulation)
      {
         _valueOrigin = valueOrigin;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateValueOriginInSimulationCommand(_quantity, _oldValueOrigin, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _oldValueOrigin = _quantity.ValueOrigin.Clone();
         _quantity.ValueOrigin.UpdateFrom(_valueOrigin);
         Description = AppConstants.Commands.UpdateParameterValueOriginInSimulation(_quantity.EntityPath(), _oldValueOrigin.ToString(), _valueOrigin.ToString(), _simulation.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _valueOrigin = null;
      }
   }
}