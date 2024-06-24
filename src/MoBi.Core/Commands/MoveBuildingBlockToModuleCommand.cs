using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class MoveBuildingBlockToModuleCommand : MoBiMacroCommand
   {
      public MoveBuildingBlockToModuleCommand(IBuildingBlock buildingBlockToMove, Module targetModule)
      {
         ObjectType = ObjectTypes.BuildingBlock;
         CommandType = AppConstants.Commands.MoveCommand;
         Description = AppConstants.Commands.MoveBuildingBlockToModule(buildingBlockToMove.DisplayName, targetModule.Name);

         var allCommands = new List<IMoBiCommand>
         {
            //add the required commands
            new RemoveBuildingBlockFromModuleCommand<IBuildingBlock>(buildingBlockToMove, buildingBlockToMove.Module),
            new AddBuildingBlockToModuleCommand<IBuildingBlock>(buildingBlockToMove, targetModule)
         };

         AddRange(allCommands);
      }
   }
}