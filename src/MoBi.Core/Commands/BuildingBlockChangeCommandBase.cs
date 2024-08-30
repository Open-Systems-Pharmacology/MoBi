using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class BuildingBlockChangeCommandBase<T> : MoBiReversibleCommand where T :  class, IBuildingBlock
   {
      public bool ShouldIncrementVersion { get; set; }
      public bool HasChangedModuleType { get; private set; }
      public PKSimModuleConversion ConversionOption { get; set; }  = PKSimModuleConversion.SetAsExtensionModule;

      protected T _buildingBlock;
      protected string _buildingBlockId;

      protected BuildingBlockChangeCommandBase(T buildingBlock)
      {
         ShouldIncrementVersion = true;
         _buildingBlock = buildingBlock;
         if (buildingBlock != null)
            _buildingBlockId = _buildingBlock.Id;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_buildingBlock == null) return;
         var buildingBlockVersionUpdater = context.Resolve<IBuildingBlockVersionUpdater>();
         HasChangedModuleType = buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_buildingBlock, ShouldIncrementVersion, ConversionOption);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = default(T);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         if (_buildingBlockId != null)
            _buildingBlock = context.Get<T>(_buildingBlockId);
      }
   }
}