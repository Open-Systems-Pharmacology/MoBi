using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class CopyBuildingBlockToModuleCommand<T> : AddBuildingBlockToModuleCommand<T> where T : class, IBuildingBlock
   {
      public CopyBuildingBlockToModuleCommand(T buildingBlock, Module existingModule) : base(buildingBlock, existingModule)
      {
         CommandType = AppConstants.Commands.CopyCommand;
         Description = AppConstants.Commands.CopyToDescription(ObjectType, buildingBlock.Name, _existingModule.Name);
      }
   }
}