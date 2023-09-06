using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Assets;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class SynchronizeParameterValueCommand : BuildingBlockChangeCommandBase<ParameterValuesBuildingBlock>
   {
      private IParameter _parameter;
      private readonly ParameterValue _parameterValue;
      private readonly string _parameterId;

      public SynchronizeParameterValueCommand(IParameter parameter, ParameterValue parameterValue, ParameterValuesBuildingBlock changingBuildingBlock) : base(changingBuildingBlock)
      {
         _parameter = parameter;
         _parameterId = parameter.Id;
         _parameterValue = parameterValue;
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
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parameter = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new SynchronizeParameterValueCommand(_parameter, _parameterValue, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parameter = context.Get<IParameter>(_parameterId);
      }
   }
}