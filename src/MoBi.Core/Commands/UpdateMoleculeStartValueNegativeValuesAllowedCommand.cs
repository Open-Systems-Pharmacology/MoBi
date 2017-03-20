using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeStartValueNegativeValuesAllowedCommand : BuildingBlockChangeCommandBase<IMoleculeStartValuesBuildingBlock>
   {
      private readonly string _startValueId;
      private IMoleculeStartValue _startValue;
      private readonly bool _oldNegativeValuesAllowed;
      private readonly bool _newNegativeValuesAllowed;

      public UpdateMoleculeStartValueNegativeValuesAllowedCommand(IMoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock, IMoleculeStartValue moleculeStartValue, bool negativeValuesAllowed)
         : base(moleculeStartValuesBuildingBlock)
      {
         _startValueId = moleculeStartValue.Id;
         _startValue = moleculeStartValue;
         _oldNegativeValuesAllowed = moleculeStartValue.NegativeValuesAllowed;
         _newNegativeValuesAllowed = negativeValuesAllowed;

         Description = AppConstants.Commands.UpdateMoleculeStartValueNegativeValuesAllowed(_startValue.Path.ToString(), _oldNegativeValuesAllowed, _newNegativeValuesAllowed);
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.MoleculeStartValue;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _startValue = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _startValue.NegativeValuesAllowed = _newNegativeValuesAllowed;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _startValue = context.Get<IMoleculeStartValue>(_startValueId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateMoleculeStartValueNegativeValuesAllowedCommand(_buildingBlock, _startValue, _oldNegativeValuesAllowed).AsInverseFor(this);
      }
   }
}