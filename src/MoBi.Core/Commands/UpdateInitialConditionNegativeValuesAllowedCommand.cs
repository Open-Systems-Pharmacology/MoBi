using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class UpdateInitialConditionNegativeValuesAllowedCommand : BuildingBlockChangeCommandBase<InitialConditionsBuildingBlock>
   {
      private readonly string _initialConditionId;
      private InitialCondition _initialCondition;
      private readonly bool _oldNegativeValuesAllowed;
      private readonly bool _newNegativeValuesAllowed;

      public UpdateInitialConditionNegativeValuesAllowedCommand(InitialConditionsBuildingBlock initialConditionsBuildingBlock, InitialCondition initialCondition, bool negativeValuesAllowed)
         : base(initialConditionsBuildingBlock)
      {
         _initialConditionId = initialCondition.Id;
         _initialCondition = initialCondition;
         _oldNegativeValuesAllowed = initialCondition.NegativeValuesAllowed;
         _newNegativeValuesAllowed = negativeValuesAllowed;

         Description = AppConstants.Commands.UpdateInitialConditionNegativeValuesAllowed(_initialCondition.Path.ToString(), _oldNegativeValuesAllowed, _newNegativeValuesAllowed);
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.InitialCondition;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _initialCondition = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _initialCondition.NegativeValuesAllowed = _newNegativeValuesAllowed;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _initialCondition = context.Get<InitialCondition>(_initialConditionId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateInitialConditionNegativeValuesAllowedCommand(_buildingBlock, _initialCondition, _oldNegativeValuesAllowed).AsInverseFor(this);
      }
   }
}