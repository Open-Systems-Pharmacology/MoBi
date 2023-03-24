﻿using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddExpressionAsParameterStartValuesCommand : ObjectUICommand<IMoBiProject>
   {
      private readonly IMoBiContext _context;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTaskForExpressionProfileBuildingBlock;
      private readonly IInteractionTasksForBuildingBlock<ParameterStartValuesBuildingBlock> _interactionTasksForPSVBuildingBlock;
      private readonly IExpressionProfileToParameterStartValuesMapper _mapper;
      private readonly IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> _editTask;

      public AddExpressionAsParameterStartValuesCommand(IMoBiContext context,
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfileBuildingBlock,
         IInteractionTasksForBuildingBlock<ParameterStartValuesBuildingBlock> interactionTasksForPSVBuildingBlock,
         IExpressionProfileToParameterStartValuesMapper mapper, IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> editTask)
      {
         _context = context;
         _interactionTaskForExpressionProfileBuildingBlock = interactionTaskForExpressionProfileBuildingBlock;
         _interactionTasksForPSVBuildingBlock = interactionTasksForPSVBuildingBlock;
         _mapper = mapper;
         _editTask = editTask;
      }

      protected override void PerformExecute()
      {
         var expressionProfiles = _interactionTaskForExpressionProfileBuildingBlock.LoadFromPKML();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = _editTask.ObjectName,
            Description = AppConstants.Commands.AddMany(_editTask.ObjectName)
         };

         foreach (var expressionProfile in expressionProfiles)
         {
            var psvBuildingBlock = _mapper.MapFrom(expressionProfile);
            macroCommand.AddCommand(_interactionTasksForPSVBuildingBlock.AddToProject(psvBuildingBlock));
         }

         _context.AddToHistory(macroCommand);
      }
   }
}