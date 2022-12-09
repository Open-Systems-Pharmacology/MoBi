using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   /// <summary>
   ///    Ensures that the value defined in the <see cref="IMoleculeStartValue" /> of simulation are synchronized
   ///    with the values defined in the <see cref="IQuantity" /> 
   /// </summary>
   public class SynchronizeMoleculeStartValueCommand : MoBiReversibleCommand
   {
      private IQuantity _quantity;
      private IMoleculeAmount _moleculeAmount;
      private readonly IMoleculeStartValue _moleculeStartValue;
      private readonly string _quantityId;

      public SynchronizeMoleculeStartValueCommand(IQuantity quantity, IMoleculeStartValue moleculeStartValue)
      {
         _quantity = quantity;
         _quantityId = quantity.Id;
         _moleculeStartValue = moleculeStartValue;
         _moleculeAmount = quantity as IMoleculeAmount ?? quantity.ParentContainer as IMoleculeAmount;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         updateStartValue();

         if (_moleculeStartValue.Dimension == _quantity.Dimension)
            _moleculeStartValue.DisplayUnit = _quantity.DisplayUnit;

         if (_moleculeAmount!=null)
            _moleculeStartValue.ScaleDivisor = _moleculeAmount.ScaleDivisor;
      }

      private void updateStartValue()
      {
         _moleculeStartValue.UpdateValueOriginFrom(_quantity.ValueOrigin);

         //we are dealing with a quantity in simulation that was initialized with a constant value, we can update 
         if (_quantity.Formula.IsConstant())
         {
            _moleculeStartValue.Value = _quantity.Value;
            return;
         }

         //value was overriden in the simulation
         if (_quantity.IsFixedValue)
         {
            _moleculeStartValue.Value = _quantity.Value;
            return;
         }

         //value has not been changed by the user and the underlying quantity has no constant formula=> we should remove the start value
         _moleculeStartValue.Value = null;
      }

      protected override void ClearReferences()
      {
         _quantity = null;
         _moleculeAmount = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SynchronizeMoleculeStartValueCommand(_quantity, _moleculeStartValue)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _quantity = context.Get<IQuantity>(_quantityId);
      }
   }
}