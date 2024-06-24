using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using NPOI.SS.Formula.Functions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class MoveBuildingBlockToModuleCommand : MoBiMacroCommand
   {
      private Module _destinationModule;
      private IBuildingBlock _buildingBlockToMove;

      public MoveBuildingBlockToModuleCommand(IBuildingBlock buildingBlockToMove, Module targetModule)
      {
         ObjectType = ObjectTypes.BuildingBlock;
         CommandType = AppConstants.Commands.MoveCommand;
         Description = AppConstants.Commands.MoveBuildingBlockToModule(buildingBlockToMove.DisplayName, targetModule.Name);

         var allCommands = new List<IMoBiCommand>
      {
         var allCommands = new List<IMoBiCommand>();

         //add the required commands
            new RemoveBuildingBlockFromModuleCommand<IBuildingBlock>(buildingBlockToMove, buildingBlockToMove.Module),
            new AddBuildingBlockToModuleCommand<IBuildingBlock>(buildingBlockToMove, targetModule)
         };

         AddRange(allCommands);

         //now execute all commands
         base.Execute(context);

         //clear references
         _destinationModule = null;
         _buildingBlockToMove = null;
      }
   }
}
