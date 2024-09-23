using MoBi.Core.Domain;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class BuildingBlockChangeCommandBase : MoBiReversibleCommand
   {
      public abstract bool WillConvertPKSimModuleToExtension { get; }
      public abstract Module Module { get; }
   }
   
   public abstract class BuildingBlockChangeCommandBase<T> : BuildingBlockChangeCommandBase where T :  class, IBuildingBlock
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
         var originalPkSimModuleState = _buildingBlock.IsPkSimModule();
         if (_buildingBlock == null) return;
         var buildingBlockVersionUpdater = context.Resolve<IBuildingBlockVersionUpdater>();
         buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_buildingBlock, ShouldIncrementVersion, ConversionOption);
         HasChangedModuleType = originalPkSimModuleState != _buildingBlock.IsPkSimModule();
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

      public override Module Module => _buildingBlock?.Module;

      public override bool WillConvertPKSimModuleToExtension
      {
         get
         {
            // not a module building block
            if (_buildingBlock.Module == null)
               return false;

            // already an extension module
            if (!_buildingBlock.Module.IsPKSimModule)
               return false;

            return ConversionOption == PKSimModuleConversion.SetAsExtensionModule;
         }
      }
   }
}