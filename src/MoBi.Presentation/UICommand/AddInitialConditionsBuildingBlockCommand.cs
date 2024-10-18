using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddInitialConditionsBuildingBlockCommand : ObjectUICommand<Module>
   {
      private readonly IMoBiContext _context;
      private readonly IInitialConditionsTask<InitialConditionsBuildingBlock> _initialConditionsTask;

      public AddInitialConditionsBuildingBlockCommand(IMoBiContext context,
         IInitialConditionsTask<InitialConditionsBuildingBlock> initialConditionsTask)
      {
         _context = context;
         _initialConditionsTask = initialConditionsTask;
      }

      protected override void PerformExecute()
      {
         var initialConditionsBuildingBlocks = _initialConditionsTask.LoadFromPKML();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectTypes.InitialConditionsBuildingBlock,
            Description = AppConstants.Commands.AddMany(ObjectTypes.InitialConditionsBuildingBlock)
         };

         foreach (var buildingBlock in initialConditionsBuildingBlocks)
         {
            macroCommand.AddCommand(_initialConditionsTask.AddTo(buildingBlock, Subject, null));
         }

         _context.AddToHistory(macroCommand);
      }
   }
}