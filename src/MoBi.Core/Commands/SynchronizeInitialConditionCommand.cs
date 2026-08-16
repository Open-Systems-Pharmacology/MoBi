using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using MoBi.Assets;
using OSPSuite.Assets;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SynchronizeInitialConditionCommand : BuildingBlockChangeCommandBase<InitialConditionsBuildingBlock>
   {
      private IQuantity _quantity;
      private MoleculeAmount _moleculeAmount;
      private readonly InitialCondition _initialCondition;
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;
      private readonly string _quantityId;
      private double? _originalValue;
      private Unit _originalDisplayUnit;
      private double _originalScaleDivisor;
      private readonly ValueOrigin _originalValueOrigin = new ValueOrigin();

      /// <summary>
      ///    Ensures that the value defined in the <see cref="InitialCondition" /> of simulation are synchronized
      ///    with the values defined in the <see cref="IQuantity" />
      /// </summary>
      public SynchronizeInitialConditionCommand(IQuantity quantity, InitialCondition initialCondition, InitialConditionsBuildingBlock buildingBlock, IMoBiSimulation simulation) : base(buildingBlock)
      {
         _quantity = quantity;
         _quantityId = quantity.Id;
         _initialCondition = initialCondition;
         _simulation = simulation;
         _simulationId = simulation.Id;
         _moleculeAmount = quantity as MoleculeAmount ?? quantity.ParentContainer as MoleculeAmount;
         ObjectType = ObjectTypes.InitialCondition;
         CommandType = AppConstants.Commands.UpdateCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _originalValue = _initialCondition.Value;
         _originalDisplayUnit = _initialCondition.DisplayUnit;
         _originalScaleDivisor = _initialCondition.ScaleDivisor;
         _originalValueOrigin.UpdateFrom(_initialCondition.ValueOrigin);

         updateInitialCondition();

         if (_initialCondition.Dimension == _quantity.Dimension)
            _initialCondition.DisplayUnit = _quantity.DisplayUnit;

         if (_moleculeAmount!=null)
            _initialCondition.ScaleDivisor = _moleculeAmount.ScaleDivisor;

         Description = AppConstants.Commands.UpdateInitialCondition(_initialCondition.Path, _initialCondition.Value, _initialCondition.IsPresent, _initialCondition.DisplayUnit, _initialCondition.ScaleDivisor, _initialCondition.NegativeValuesAllowed);

         context.Resolve<ISimulationEntitySourceUpdater>().UpdateSourcesForNewPathAndValueEntity(_buildingBlock, _initialCondition.Path, _simulation);
      }

      private void updateInitialCondition()
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
         base.ClearReferences();
         _quantity = null;
         _simulation = null;
         _moleculeAmount = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RestoreInitialConditionCommand(_quantity, _initialCondition, _buildingBlock, _simulation, _originalValue, _originalDisplayUnit, _originalScaleDivisor, _originalValueOrigin)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _quantity = context.Get<IQuantity>(_quantityId);
         _simulation = context.CurrentProject.Simulations.FindById(_simulationId);
      }

      // Should only ever be invoked as an inverse to the SynchronizeInitialConditionCommand command. The values committed to the
      // building block cannot be recovered from the quantity because it still holds the value that was committed.
      private class RestoreInitialConditionCommand : BuildingBlockChangeCommandBase<InitialConditionsBuildingBlock>
      {
         private IQuantity _quantity;
         private readonly InitialCondition _initialCondition;
         private IMoBiSimulation _simulation;
         private readonly string _simulationId;
         private readonly string _quantityId;
         private readonly double? _valueToRestore;
         private readonly Unit _displayUnitToRestore;
         private readonly double _scaleDivisorToRestore;
         private readonly ValueOrigin _valueOriginToRestore;

         public RestoreInitialConditionCommand(IQuantity quantity, InitialCondition initialCondition, InitialConditionsBuildingBlock buildingBlock, IMoBiSimulation simulation,
            double? valueToRestore, Unit displayUnitToRestore, double scaleDivisorToRestore, ValueOrigin valueOriginToRestore) : base(buildingBlock)
         {
            _quantity = quantity;
            _quantityId = quantity.Id;
            _initialCondition = initialCondition;
            _simulation = simulation;
            _simulationId = simulation.Id;
            _valueToRestore = valueToRestore;
            _displayUnitToRestore = displayUnitToRestore;
            _scaleDivisorToRestore = scaleDivisorToRestore;
            _valueOriginToRestore = valueOriginToRestore;
            ObjectType = ObjectTypes.InitialCondition;
            CommandType = AppConstants.Commands.UpdateCommand;
         }

         protected override void ExecuteWith(IMoBiContext context)
         {
            base.ExecuteWith(context);
            _initialCondition.Value = _valueToRestore;
            _initialCondition.DisplayUnit = _displayUnitToRestore;
            _initialCondition.ScaleDivisor = _scaleDivisorToRestore;
            _initialCondition.UpdateValueOriginFrom(_valueOriginToRestore);

            Description = AppConstants.Commands.UpdateInitialCondition(_initialCondition.Path, _initialCondition.Value, _initialCondition.IsPresent, _initialCondition.DisplayUnit, _initialCondition.ScaleDivisor, _initialCondition.NegativeValuesAllowed);
         }

         protected override void ClearReferences()
         {
            base.ClearReferences();
            _quantity = null;
            _simulation = null;
         }

         protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return new SynchronizeInitialConditionCommand(_quantity, _initialCondition, _buildingBlock, _simulation)
            {
               Visible = Visible
            }.AsInverseFor(this);
         }

         public override void RestoreExecutionData(IMoBiContext context)
         {
            base.RestoreExecutionData(context);
            _quantity = context.Get<IQuantity>(_quantityId);
            _simulation = context.CurrentProject.Simulations.FindById(_simulationId);
         }
      }
   }
}
