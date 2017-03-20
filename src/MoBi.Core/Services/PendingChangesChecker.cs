using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Services
{
   public interface IPendingChangesChecker
   {
      void CheckForBuildingBlockChanges(IBuildingBlockInfo buildingBlockInfo, IBuildingBlock commitTargetBuildingBlock);
   }

   public class PendingChangesChecker : IPendingChangesChecker
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public PendingChangesChecker(IDialogCreator dialogCreator, IObjectTypeResolver objectTypeResolver)
      {
         _dialogCreator = dialogCreator;
         _objectTypeResolver = objectTypeResolver;
      }

      public void CheckForBuildingBlockChanges(IBuildingBlockInfo buildingBlockInfo, IBuildingBlock commitTargetBuildingBlock)
      {
         if (buildingBlockInfo.BuildingBlockChanged)
         {
            var typeName = _objectTypeResolver.TypeFor(commitTargetBuildingBlock);
            _dialogCreator.MessageBoxInfo(AppConstants.Dialog.PendingBuildingBlockChangesInfo(typeName, commitTargetBuildingBlock.Name));
         }
      }
   }
}