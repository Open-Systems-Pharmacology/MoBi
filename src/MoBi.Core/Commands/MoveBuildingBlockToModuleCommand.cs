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
         _destinationModule = targetModule;
         _buildingBlockToMove = buildingBlockToMove;
         Description = AppConstants.Commands.MoveBuildingBlockToModule(_buildingBlockToMove.DisplayName, _destinationModule.Name);
      }

      public override void Execute(IMoBiContext context)
      {
         var allCommands = new List<IMoBiCommand>();

         //add the required commands
         allCommands.Add(new RemoveBuildingBlockFromModuleCommand<IBuildingBlock>(_buildingBlockToMove, _buildingBlockToMove.Module));
         allCommands.Add(new AddBuildingBlockToModuleCommand<IBuildingBlock>(_buildingBlockToMove, _destinationModule));

         AddRange(allCommands);

         //now execute all commands
         base.Execute(context);

         //clear references
         _destinationModule = null;
         _buildingBlockToMove = null;
      }
   }
}
