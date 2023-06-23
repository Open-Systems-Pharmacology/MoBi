using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddPathAndValueEntityToBuildingBlockInSimulationCommand<T> : PathAndValueEntityBuildingBlockInSimulationCommandBase<T> where T : PathAndValueEntity
   {
      private readonly T _pathAndValueEntity;
      private ObjectPath _objectPath;

      public AddPathAndValueEntityToBuildingBlockInSimulationCommand(T pathAndValueEntity, PathAndValueEntityBuildingBlock<T> pathAndValueEntitiesBuildingBlock)
         : base(pathAndValueEntitiesBuildingBlock)
      {
         _pathAndValueEntity = pathAndValueEntity;
         _objectPath = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_pathAndValueEntity == null)
            return;

         _objectPath = _pathAndValueEntity.Path;
         if (_pathAndValueEntitiesBuildingBlock[_objectPath] != null)
            return;

         _pathAndValueEntitiesBuildingBlock.Add(_pathAndValueEntity);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<T>(_objectPath, _pathAndValueEntitiesBuildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }
}