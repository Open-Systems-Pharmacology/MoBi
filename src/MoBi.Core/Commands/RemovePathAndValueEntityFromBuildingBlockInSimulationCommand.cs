using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Assets;
using MoBi.Core.Helper;

namespace MoBi.Core.Commands
{
   public class RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
   {
      private readonly ObjectPath _objectPath;
      private T _pathAndValueEntity;
      private readonly ObjectTypeResolver _objectTypeResolver;

      public RemovePathAndValueEntityFromBuildingBlockInSimulationCommand(ObjectPath objectPath, PathAndValueEntityBuildingBlock<T> pathAndValueEntitiesBuildingBlock) : base(pathAndValueEntitiesBuildingBlock)
      {
         _objectPath = objectPath;
         CommandType = AppConstants.Commands.DeleteCommand;
         _objectTypeResolver = new ObjectTypeResolver();
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         if (_objectPath == null)
            return;

         _pathAndValueEntity = _buildingBlock[_objectPath];
         if (_pathAndValueEntity == null)
            return;

         ObjectType = _objectTypeResolver.TypeFor(_pathAndValueEntity);
         Description = AppConstants.Commands.RemovedPathAndValueEntity(_pathAndValueEntity, _buildingBlock.Name, ObjectType);

         _buildingBlock.Remove(_pathAndValueEntity);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddPathAndValueEntityToBuildingBlockCommand<T>(_buildingBlock, _pathAndValueEntity)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }
}