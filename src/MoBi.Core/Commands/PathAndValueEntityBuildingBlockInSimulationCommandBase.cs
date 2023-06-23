using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class PathAndValueEntityBuildingBlockInSimulationCommandBase<T> : MoBiReversibleCommand where T : PathAndValueEntity
   {
      protected PathAndValueEntityBuildingBlock<T> _pathAndValueEntitiesBuildingBlock;
      private readonly string _pathAndValueEntitiesBuildingBlockId;

      protected PathAndValueEntityBuildingBlockInSimulationCommandBase(PathAndValueEntityBuildingBlock<T> pathAndValueEntitiesBuildingBlock)
      {
         _pathAndValueEntitiesBuildingBlock = pathAndValueEntitiesBuildingBlock;
         _pathAndValueEntitiesBuildingBlockId = pathAndValueEntitiesBuildingBlock.Id;
      }

      protected override void ClearReferences()
      {
         _pathAndValueEntitiesBuildingBlock = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _pathAndValueEntitiesBuildingBlock = context.Get<PathAndValueEntityBuildingBlock<T>>(_pathAndValueEntitiesBuildingBlockId);
      }
   }
}