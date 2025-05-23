using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class SynchronizeParameterValueCommand : BuildingBlockChangeCommandBase<ParameterValuesBuildingBlock>
   {
      private IParameter _parameter;
      private readonly ParameterValue _parameterValue;
      private IMoBiSimulation _simulation;
      private readonly string _parameterId;
      private readonly string _simulationId;

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
         return new SynchronizeParameterValueCommand(_parameter, _parameterValue, _buildingBlock, _simulation)
         {
            Visible = Visible,
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parameter = context.Get<IParameter>(_parameterId);
         context.CurrentProject.Simulations.FindById(_simulationId);
      }
   }
}