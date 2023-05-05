using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateParameterStartValueInBuildingBlockCommand : BuildingBlockChangeCommandBase<ParameterValuesBuildingBlock>
   {
      private readonly ObjectPath _path;
      private readonly double? _value;
      private double? _originalValue;

      public UpdateParameterStartValueInBuildingBlockCommand(
         ParameterValuesBuildingBlock parameterStartValuesBuildingBlock, 
         ObjectPath path,
         double? value) : base(parameterStartValuesBuildingBlock)
      {
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.ParameterValue;
         _path = path;
         _value = value;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var psv = _buildingBlock[_path];
         if (psv == null) return;

         _originalValue = psv.Value;
         psv.Value = _value;
         Description = AppConstants.Commands.UpdateParameterStartValue(_path, _value, psv.DisplayUnit);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateParameterStartValueInBuildingBlockCommand(_buildingBlock, _path, _originalValue).AsInverseFor(this);
      }
   }
}