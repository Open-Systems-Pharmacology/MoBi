using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class BuildingBlockIrreversibleChangeCommandBase<T> : MoBiCommand where T : class, IBuildingBlock
   {
      protected T _buildingBlock;
      protected string _buildingBlockId;

      protected BuildingBlockIrreversibleChangeCommandBase(T buildingBlock)
      {
         _buildingBlock = buildingBlock;
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_buildingBlock == null) return;
         var buildingBlockVersionUpdater = context.Resolve<IBuildingBlockVersionUpdater>();
         buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_buildingBlock, shouldIncrementVersion: true);
      }
   }
}