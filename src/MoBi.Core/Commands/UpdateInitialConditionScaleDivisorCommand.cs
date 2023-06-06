using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateInitialConditionScaleDivisorCommand : BuildingBlockChangeCommandBase<IBuildingBlock<InitialCondition>>
   {
      private readonly double _newScaleDivisor;
      private readonly double _oldScaleDivisor;
      private InitialCondition _initialCondition;
      private readonly string _initialConditionId;

      public UpdateInitialConditionScaleDivisorCommand(IBuildingBlock<InitialCondition> buildingBlock, InitialCondition initialCondition, double newScaleDivisor, double oldScaleDivisor)
         : base(buildingBlock)
      {
         _newScaleDivisor = newScaleDivisor;
         _oldScaleDivisor = oldScaleDivisor;
         _initialCondition = initialCondition;
         _initialConditionId = initialCondition.Id;

         Description = AppConstants.Commands.UpdateInitialConditionScaleDivisor(_initialCondition.Path.ToString(), oldScaleDivisor, newScaleDivisor);
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
         _initialCondition.ScaleDivisor = _newScaleDivisor;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _initialCondition = context.Get<InitialCondition>(_initialConditionId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateInitialConditionScaleDivisorCommand(_buildingBlock, _initialCondition, _oldScaleDivisor, _newScaleDivisor).AsInverseFor(this);
      }
   }
}