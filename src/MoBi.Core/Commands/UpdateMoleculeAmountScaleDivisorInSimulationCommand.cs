using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeAmountScaleDivisorInSimulationCommand : SetQuantityPropertyInSimulationCommandBase<MoleculeAmount>
   {
      private readonly double _newScaleDivisor;
      private readonly double _oldScaleDivisor;

      public UpdateMoleculeAmountScaleDivisorInSimulationCommand(MoleculeAmount quantity, double newScaleDivisor, IMoBiSimulation simulation) : base(quantity, simulation)
      {
         _newScaleDivisor = newScaleDivisor;
         _oldScaleDivisor = _quantity.ScaleDivisor;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateMoleculeAmountScaleDivisorInSimulationCommand(_quantity, _oldScaleDivisor, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         _quantity.ScaleDivisor = _newScaleDivisor;
         Description = AppConstants.Commands.UpdateScaleDivisorValue(_quantity.Name, _oldScaleDivisor, _newScaleDivisor);
      }
   }
}