using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateValueOriginInStartValueCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
   {
      private readonly T _startValue;
      private ValueOrigin _oldValueOrigin;
      private ValueOrigin _newValueOrigin;

      public UpdateValueOriginInStartValueCommand(T startValue, ValueOrigin newValueOrigin, PathAndValueEntityBuildingBlock<T> startValuesBuildingBlock) : base(startValuesBuildingBlock)
      {
         _startValue = startValue;
         _newValueOrigin = newValueOrigin;

         ObjectType = new ObjectTypeResolver().TypeFor(startValue);
         CommandType = AppConstants.Commands.EditCommand;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<PathAndValueEntityBuildingBlock<T>>(_buildingBlockId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateValueOriginInStartValueCommand<T>(_startValue, _oldValueOrigin, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldValueOrigin = _startValue.ValueOrigin.Clone();
         _startValue.UpdateValueOriginFrom(_newValueOrigin);
         Description = AppConstants.Commands.UpdateStartValueValueOrigin(_startValue.Path.PathAsString, _oldValueOrigin.ToString(), _newValueOrigin.ToString(), ObjectType, _buildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newValueOrigin = null;
      }
   }
}