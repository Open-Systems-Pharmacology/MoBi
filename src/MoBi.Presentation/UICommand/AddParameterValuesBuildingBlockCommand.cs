using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddParameterValuesBuildingBlockCommand : ObjectUICommand<Module>
   {
      private readonly IMoBiContext _context;
      private readonly IParameterValuesTask _parameterValuesTask;

      public AddParameterValuesBuildingBlockCommand(IMoBiContext context,
         IParameterValuesTask parameterValuesTask,
         IEditTasksForBuildingBlock<IndividualBuildingBlock> editTask)
      {
         _context = context;
         _parameterValuesTask = parameterValuesTask;
      }

      protected override void PerformExecute()
      {
         var parameterValuesBuildingBlocks = _parameterValuesTask.LoadFromPKML();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectTypes.ParameterValuesBuildingBlock,
            Description = AppConstants.Commands.AddMany(ObjectTypes.ParameterValuesBuildingBlock)
         };

         foreach (var buildingBlock in parameterValuesBuildingBlocks)
         {
            macroCommand.AddCommand(_parameterValuesTask.AddTo(buildingBlock, Subject, null));
         }

         _context.AddToHistory(macroCommand);
      }
   }
}