using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateInitialConditionIsPresentCommand : BuildingBlockChangeCommandBase<ILookupBuildingBlock<InitialCondition>>
   {
      private readonly ObjectPath _initialConditionPath;
      private InitialCondition _initialCondition;
      private readonly bool _oldIsPresent;
      private readonly bool _newIsPresent;

      public UpdateInitialConditionIsPresentCommand(ILookupBuildingBlock<InitialCondition> initialConditionsBuildingBlock, InitialCondition initialCondition, bool isPresent) : base(initialConditionsBuildingBlock)
      {
         _initialConditionPath = initialCondition.Path;
         _initialCondition = initialCondition;
         _oldIsPresent = initialCondition.IsPresent;
         _newIsPresent = isPresent;

         Description = AppConstants.Commands.UpdateInitialConditionIsPresent(_initialCondition.Path.ToString(), _oldIsPresent, _newIsPresent);
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
         _initialCondition.IsPresent = _newIsPresent;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _initialCondition = _buildingBlock.ByPath(_initialConditionPath);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateInitialConditionIsPresentCommand(_buildingBlock, _initialCondition, _oldIsPresent).AsInverseFor(this);
      }
   }
}