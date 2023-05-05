using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeStartValueInBuildingBlockCommand : BuildingBlockChangeCommandBase<InitialConditionsBuildingBlock>
   {
      private readonly ObjectPath _path;
      private readonly double? _value;
      private readonly double? _originalValue;
      private readonly bool _present;
      private readonly bool _originalPresent;
      private readonly double _scaleDisivor;
      private readonly bool _negativeValuesAllowed;
      private readonly double _originalScaleDivisor;
      private readonly bool _originalNegativeValuesAllowed;

      public UpdateMoleculeStartValueInBuildingBlockCommand(
         InitialConditionsBuildingBlock startValuesBuildingBlock, 
         ObjectPath path, 
         double? value, bool present, double scaleDisivor, bool negativeValuesAllowed) : base(startValuesBuildingBlock)
      {
         CommandType = AppConstants.Commands.UpdateCommand;
         ObjectType = ObjectTypes.InitialCondition;
         _path = path;
         _value = value;
         _present = present;
         _scaleDisivor = scaleDisivor;
         _negativeValuesAllowed = negativeValuesAllowed;

         var moleculeStartValue = _buildingBlock[_path];
         if (moleculeStartValue == null) return;
         _originalValue = moleculeStartValue.Value;
         _originalPresent = moleculeStartValue.IsPresent;
         _originalScaleDivisor = moleculeStartValue.ScaleDivisor;
         _originalNegativeValuesAllowed = moleculeStartValue.NegativeValuesAllowed;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var msv = _buildingBlock[_path];
         if (msv == null) return;

         msv.Value = _value;
         msv.IsPresent = _present;
         msv.ScaleDivisor = _scaleDisivor;
         msv.NegativeValuesAllowed = _negativeValuesAllowed;
         Description = AppConstants.Commands.UpdateMoleculeStartValue(_path, _value, _present, msv.DisplayUnit, _scaleDisivor,_negativeValuesAllowed);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateMoleculeStartValueInBuildingBlockCommand(_buildingBlock, _path, _originalValue, _originalPresent, _originalScaleDivisor,_originalNegativeValuesAllowed).AsInverseFor(this);
      }
   }
}
