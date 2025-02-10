using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class AddMultipleBuildingBlocksToModuleCommand : MoBiMacroCommand
   {
      private Module _existingModule;
      private IReadOnlyList<IBuildingBlock> _listOfNewBuildingBlocks;

      public AddMultipleBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks)
      {
         ObjectType = ObjectTypes.BuildingBlock;
         CommandType = AppConstants.Commands.AddCommand;
         _existingModule = existingModule;
         _listOfNewBuildingBlocks = listOfNewBuildingBlocks;
         Description = AppConstants.Commands.AddBuildingBlocksToModule(_existingModule.Name);
      }

      public override void Execute(IMoBiContext context)
      {
         var allCommands = new List<IMoBiCommand>();

         //add the required commands
         _listOfNewBuildingBlocks.Each(x => allCommands.Add(new AddBuildingBlockToModuleCommand<IBuildingBlock>(x, _existingModule)));

         //hide from the history browser ( only if at least two command)
         if (allCommands.Count >= 2)
            allCommands.Each(x => x.Visible = false);

         AddRange(allCommands);

         //now execute all commands
         base.Execute(context);

         //clear references
         _existingModule = null;
         _listOfNewBuildingBlocks = null;
      }
   }
}