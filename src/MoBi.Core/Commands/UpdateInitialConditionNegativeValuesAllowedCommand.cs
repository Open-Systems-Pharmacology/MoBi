using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateInitialConditionNegativeValuesAllowedCommand : BuildingBlockChangeCommandBase<ILookupBuildingBlock<InitialCondition>>
   {
      private readonly ObjectPath _initialConditionPath;
      private InitialCondition _initialCondition;
      private readonly bool _oldNegativeValuesAllowed;
      private readonly bool _newNegativeValuesAllowed;

      public UpdateInitialConditionNegativeValuesAllowedCommand(ILookupBuildingBlock<InitialCondition> initialConditionsBuildingBlock, InitialCondition initialCondition, bool negativeValuesAllowed)
         : base(initialConditionsBuildingBlock)
      {
         _initialConditionPath = initialCondition.Path;
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
         _initialCondition = _buildingBlock.ByPath(_initialConditionPath);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateInitialConditionNegativeValuesAllowedCommand(_buildingBlock, _initialCondition, _oldNegativeValuesAllowed).AsInverseFor(this);
      }
   }
}