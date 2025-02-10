using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddIndividualAsParameterValuesCommand : ObjectUICommand<Module>
   {
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForIndividualBuildingBlock _interactionTaskForIndividualBuildingBlock;
      private readonly IInteractionTasksForBuildingBlock<Module, ParameterValuesBuildingBlock> _interactionTasksForPSVBuildingBlock;
      private readonly IIndividualToParameterValuesMapper _mapper;
      private readonly IEditTasksForBuildingBlock<IndividualBuildingBlock> _editTask;

      public AddIndividualAsParameterValuesCommand(IMoBiContext context,
         IInteractionTasksForIndividualBuildingBlock interactionTaskForIndividualBuildingBlock,
         IInteractionTasksForBuildingBlock<Module, ParameterValuesBuildingBlock> interactionTasksForPSVBuildingBlock,
         IIndividualToParameterValuesMapper mapper, IEditTasksForBuildingBlock<IndividualBuildingBlock> editTask)
      {
         _context = context;
         _interactionTaskForIndividualBuildingBlock = interactionTaskForIndividualBuildingBlock;
         _interactionTasksForPSVBuildingBlock = interactionTasksForPSVBuildingBlock;
         _mapper = mapper;
         _editTask = editTask;
      }

      protected override void PerformExecute()
      {
         var individuals = _interactionTaskForIndividualBuildingBlock.LoadFromPKML();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = _editTask.ObjectName,
            Description = AppConstants.Commands.AddMany(_editTask.ObjectName)
         };

         foreach (var individual in individuals)
         {
            var psvBuildingBlock = _mapper.MapFrom(individual);
            macroCommand.AddCommand(_interactionTasksForPSVBuildingBlock.AddTo(psvBuildingBlock, Subject, null));
         }

         _context.AddToHistory(macroCommand);
      }
   }
}