using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<T> : PathAndValueEntityBuildingBlockInSimulationCommandBase<T> where T : PathAndValueEntity
   {
      private readonly ObjectPath _objectPath;
      private T _pathAndValueEntity;

      public RemovePathAndValueEntityFromBuildingBlockInSimulationCommand(ObjectPath objectPath, PathAndValueEntityBuildingBlock<T> pathAndValueEntitiesBuildingBlock) : base(pathAndValueEntitiesBuildingBlock)
      {
         _objectPath = objectPath;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_objectPath == null)
            return;

         _pathAndValueEntity = _pathAndValueEntitiesBuildingBlock[_objectPath];
         if (_pathAndValueEntity == null)
            return;

         _pathAndValueEntitiesBuildingBlock.Remove(_pathAndValueEntity);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddPathAndValueEntityToBuildingBlockCommand<T>(_pathAndValueEntitiesBuildingBlock, _pathAndValueEntity)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }
}