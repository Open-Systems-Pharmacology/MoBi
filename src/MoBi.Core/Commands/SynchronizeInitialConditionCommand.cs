using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{

   public class SynchronizeInitialConditionCommand : MoBiReversibleCommand
   {
      private IQuantity _quantity;
      private MoleculeAmount _moleculeAmount;
      private readonly InitialCondition _initialCondition;
      private readonly string _quantityId;

      /// <summary>
      ///    Ensures that the value defined in the <see cref="InitialCondition" /> of simulation are synchronized
      ///    with the values defined in the <see cref="IQuantity" /> 
      /// </summary>
      public SynchronizeInitialConditionCommand(IQuantity quantity, InitialCondition initialCondition)
      {
         _quantity = quantity;
         _quantityId = quantity.Id;
         _initialCondition = initialCondition;
         _moleculeAmount = quantity as MoleculeAmount ?? quantity.ParentContainer as MoleculeAmount;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         updateStartValue();

         if (_initialCondition.Dimension == _quantity.Dimension)
            _initialCondition.DisplayUnit = _quantity.DisplayUnit;

         if (_moleculeAmount!=null)
            _initialCondition.ScaleDivisor = _moleculeAmount.ScaleDivisor;
      }

      private void updateStartValue()
      {
         _initialCondition.UpdateValueOriginFrom(_quantity.ValueOrigin);

         //we are dealing with a quantity in simulation that was initialized with a constant value, we can update 
         if (_quantity.Formula.IsConstant())
         {
            _initialCondition.Value = _quantity.Value;
            return;
         }

         //value was overriden in the simulation
         if (_quantity.IsFixedValue)
         {
            _initialCondition.Value = _quantity.Value;
            return;
         }

         //value has not been changed by the user and the underlying quantity has no constant formula=> we should remove the start value
         _initialCondition.Value = null;
      }

      protected override void ClearReferences()
      {
         _quantity = null;
         _moleculeAmount = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SynchronizeInitialConditionCommand(_quantity, _initialCondition)
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