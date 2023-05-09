using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public class UpdateDimensionInPathAndValueEntityCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
   {
      private readonly T _pathAndValueEntity;
      private readonly IDimension _oldDimension;
      private readonly IDimension _newDimension;
      private readonly Unit _oldDisplayUnit;
      private readonly Unit _newDisplayUnit;

      public UpdateDimensionInPathAndValueEntityCommand(T pathAndValueEntity, IDimension newDimension, Unit newDisplayUnit, PathAndValueEntityBuildingBlock<T> buildingBlock)
         : base(buildingBlock)
      {
         _pathAndValueEntity = pathAndValueEntity;
         _oldDimension = pathAndValueEntity.Dimension;
         _oldDisplayUnit = pathAndValueEntity.DisplayUnit;
         _newDimension = newDimension;
         _newDisplayUnit = newDisplayUnit;

         ObjectType = new ObjectTypeResolver().TypeFor(pathAndValueEntity);
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.UpdateDimensions(pathAndValueEntity.Path.PathAsString, ObjectType, _oldDimension, newDimension, buildingBlock.Name);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<PathAndValueEntityBuildingBlock<T>>(_buildingBlockId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateDimensionInPathAndValueEntityCommand<T>(_pathAndValueEntity, _oldDimension, _oldDisplayUnit, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _pathAndValueEntity.Dimension = _newDimension;
         _pathAndValueEntity.DisplayUnit = _newDisplayUnit;
      }
   }
}