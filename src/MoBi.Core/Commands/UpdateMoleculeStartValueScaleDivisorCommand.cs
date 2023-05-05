using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateMoleculeStartValueScaleDivisorCommand : BuildingBlockChangeCommandBase<InitialConditionsBuildingBlock>
   {
      private readonly double _newScaleDivisor;
      private readonly double _oldScaleDivisor;
      private InitialCondition _startValue;
      private readonly string _startValueId;

      public UpdateMoleculeStartValueScaleDivisorCommand(InitialConditionsBuildingBlock buildingBlock, InitialCondition startValue, double newScaleDivisor, double oldScaleDivisor) 
         : base(buildingBlock)
      {
         _newScaleDivisor = newScaleDivisor;
         _oldScaleDivisor = oldScaleDivisor;
         _startValue = startValue;
         _startValueId = startValue.Id;

         Description = AppConstants.Commands.UpdateMoleculeStartValueScaleDivisor(_startValue.Path.ToString(), oldScaleDivisor, newScaleDivisor);
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.InitialCondition;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _startValue = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _startValue.ScaleDivisor = _newScaleDivisor;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _startValue = context.Get<InitialCondition>(_startValueId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateMoleculeStartValueScaleDivisorCommand(_buildingBlock, _startValue, _oldScaleDivisor, _newScaleDivisor).AsInverseFor(this);
      }
   }
}