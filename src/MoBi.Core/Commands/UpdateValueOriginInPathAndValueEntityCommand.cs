using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class UpdateValueOriginInPathAndValueEntityCommand<T> : BuildingBlockChangeCommandBase<IBuildingBlock<T>> where T : PathAndValueEntity
   {
      private readonly T _pathAndValueEntity;
      private ValueOrigin _oldValueOrigin;
      private ValueOrigin _newValueOrigin;

      public UpdateValueOriginInPathAndValueEntityCommand(T pathAndValueEntity, ValueOrigin newValueOrigin, IBuildingBlock<T> buildingBlock) : base(buildingBlock)
      {
         _pathAndValueEntity = pathAndValueEntity;
         _newValueOrigin = newValueOrigin;

         ObjectType = new ObjectTypeResolver().TypeFor(pathAndValueEntity);
         CommandType = AppConstants.Commands.EditCommand;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<PathAndValueEntityBuildingBlock<T>>(_buildingBlockId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateValueOriginInPathAndValueEntityCommand<T>(_pathAndValueEntity, _oldValueOrigin, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldValueOrigin = _pathAndValueEntity.ValueOrigin.Clone();
         _pathAndValueEntity.UpdateValueOriginFrom(_newValueOrigin);
         Description = AppConstants.Commands.UpdatePathAndValueEntityValueOrigin(_pathAndValueEntity.Path.PathAsString, _oldValueOrigin.ToString(), _newValueOrigin.ToString(), ObjectType, _buildingBlock.Name);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _newValueOrigin = null;
      }
   }
}