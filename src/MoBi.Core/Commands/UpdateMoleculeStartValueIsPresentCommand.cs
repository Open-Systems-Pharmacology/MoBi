using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeStartValueIsPresentCommand : BuildingBlockChangeCommandBase<MoleculeStartValuesBuildingBlock>
   {
      private readonly string _startValueId;
      private MoleculeStartValue _startValue;
      private readonly bool _oldIsPresent;
      private readonly bool _newIsPresent;

      public UpdateMoleculeStartValueIsPresentCommand(MoleculeStartValuesBuildingBlock moleculeStartValuesBuildingBlock, MoleculeStartValue moleculeStartValue, bool isPresent) : base(moleculeStartValuesBuildingBlock)
      {
         _startValueId = moleculeStartValue.Id;
         _startValue = moleculeStartValue;
         _oldIsPresent = moleculeStartValue.IsPresent;
         _newIsPresent = isPresent;

         Description = AppConstants.Commands.UpdateMoleculeStartValueIsPresent(_startValue.Path.ToString(), _oldIsPresent, _newIsPresent);
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
         _startValue.IsPresent = _newIsPresent;

      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _startValue = context.Get<MoleculeStartValue>(_startValueId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateMoleculeStartValueIsPresentCommand(_buildingBlock, _startValue, _oldIsPresent).AsInverseFor(this);
      }
   }
}