using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class SynchronizeParameterValueCommand : BuildingBlockChangeCommandBase<ParameterValuesBuildingBlock>
   {
      private IParameter _parameter;
      private readonly ParameterValue _parameterValue;
      private IMoBiSimulation _simulation;
      private readonly string _parameterId;
      private readonly string _simulationId;
      private double? _originalValue;
      private IDimension _originalDimension;
      private Unit _originalDisplayUnit;
      private readonly ValueOrigin _originalValueOrigin = new ValueOrigin();

      public SynchronizeParameterValueCommand(IParameter parameter, ParameterValue parameterValue, ParameterValuesBuildingBlock changingBuildingBlock, IMoBiSimulation simulation) : base(changingBuildingBlock)
      {
         _parameter = parameter;
         _parameterId = parameter.Id;
         _parameterValue = parameterValue;
         _simulation = simulation;
         _simulationId = simulation.Id;
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.ParameterValue;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _originalValue = _parameterValue.Value;
         _originalDimension = _parameterValue.Dimension;
         _originalDisplayUnit = _parameterValue.DisplayUnit;
         _originalValueOrigin.UpdateFrom(_parameterValue.ValueOrigin);

         _parameterValue.Value = _parameter.Value;
         _parameterValue.Dimension = _parameter.Dimension;
         _parameterValue.DisplayUnit = _parameter.DisplayUnit;
         _parameterValue.UpdateValueOriginFrom(_parameter.ValueOrigin);

         Description = AppConstants.Commands.UpdateParameterValue(_parameterValue.Path, _parameterValue.Value, _parameterValue.DisplayUnit);
         context.Resolve<ISimulationEntitySourceUpdater>().UpdateSourcesForNewPathAndValueEntity(_buildingBlock, _parameterValue.Path, _simulation);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parameter = null;
         _simulation = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RestoreParameterValueCommand(_parameter, _parameterValue, _buildingBlock, _simulation, _originalValue, _originalDimension, _originalDisplayUnit, _originalValueOrigin)
         {
            Visible = Visible,
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parameter = context.Get<IParameter>(_parameterId);
         _simulation = context.CurrentProject.Simulations.FindById(_simulationId);
      }

      // Should only ever be invoked as an inverse to the SynchronizeParameterValueCommand command. The values committed to the
      // building block cannot be recovered from the parameter because it still holds the value that was committed.
      private class RestoreParameterValueCommand : BuildingBlockChangeCommandBase<ParameterValuesBuildingBlock>
      {
         private IParameter _parameter;
         private readonly ParameterValue _parameterValue;
         private IMoBiSimulation _simulation;
         private readonly string _parameterId;
         private readonly string _simulationId;
         private readonly double? _valueToRestore;
         private readonly IDimension _dimensionToRestore;
         private readonly Unit _displayUnitToRestore;
         private readonly ValueOrigin _valueOriginToRestore;

         public RestoreParameterValueCommand(IParameter parameter, ParameterValue parameterValue, ParameterValuesBuildingBlock changingBuildingBlock, IMoBiSimulation simulation,
            double? valueToRestore, IDimension dimensionToRestore, Unit displayUnitToRestore, ValueOrigin valueOriginToRestore) : base(changingBuildingBlock)
         {
            _parameter = parameter;
            _parameterId = parameter.Id;
            _parameterValue = parameterValue;
            _simulation = simulation;
            _simulationId = simulation.Id;
            _valueToRestore = valueToRestore;
            _dimensionToRestore = dimensionToRestore;
            _displayUnitToRestore = displayUnitToRestore;
            _valueOriginToRestore = valueOriginToRestore;
            CommandType = AppConstants.Commands.UpdateCommand;
            ObjectType = ObjectTypes.ParameterValue;
         }

         protected override void ExecuteWith(IMoBiContext context)
         {
            base.ExecuteWith(context);
            _parameterValue.Value = _valueToRestore;
            _parameterValue.Dimension = _dimensionToRestore;
            _parameterValue.DisplayUnit = _displayUnitToRestore;
            _parameterValue.UpdateValueOriginFrom(_valueOriginToRestore);

            Description = AppConstants.Commands.UpdateParameterValue(_parameterValue.Path, _parameterValue.Value, _parameterValue.DisplayUnit);
         }

         protected override void ClearReferences()
         {
            base.ClearReferences();
            _parameter = null;
            _simulation = null;
         }

         protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return new SynchronizeParameterValueCommand(_parameter, _parameterValue, _buildingBlock, _simulation)
            {
               Visible = Visible,
            }.AsInverseFor(this);
         }

         public override void RestoreExecutionData(IMoBiContext context)
         {
            base.RestoreExecutionData(context);
            _parameter = context.Get<IParameter>(_parameterId);
            _simulation = context.CurrentProject.Simulations.FindById(_simulationId);
         }
      }
   }
}
