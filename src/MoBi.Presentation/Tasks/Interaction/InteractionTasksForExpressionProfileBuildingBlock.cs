using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForExpressionProfileBuildingBlock : IInteractionTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IInteractionTasksForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      IReadOnlyList<ExpressionProfileBuildingBlock> LoadFromPKML();
      IMoBiCommand UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock buildingBlock);
   }

   public class InteractionTasksForExpressionProfileBuildingBlock : InteractionTasksForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>, IInteractionTasksForExpressionProfileBuildingBlock
   {
      private readonly IEditTasksForExpressionProfileBuildingBlock _editTaskForExpressionProfileBuildingBlock;
      private readonly IPKSimStarter _pkSimStarter;

      public InteractionTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForExpressionProfileBuildingBlock editTask, IMoBiFormulaTask formulaTask, IPKSimStarter pkSimStarter) :
         base(interactionTaskContext, editTask, formulaTask)
      {
         _editTaskForExpressionProfileBuildingBlock = editTask;
         _pkSimStarter = pkSimStarter;
      }

      public IReadOnlyList<ExpressionProfileBuildingBlock> LoadFromPKML()
      {
         var filename = AskForPKMLFileToOpen();
         return (string.IsNullOrEmpty(filename) ? Enumerable.Empty<ExpressionProfileBuildingBlock>() : LoadItems(filename)).ToList();
      }

      public IMoBiCommand UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock buildingBlock)
      {
         var expressionProfileUpdate = _pkSimStarter.UpdateExpressionProfileFromDatabase(buildingBlock);
         
         if (expressionProfileUpdate == null)
            return new MoBiEmptyCommand();

         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = ObjectTypes.ExpressionProfileBuildingBlock,
            CommandType = AppConstants.Commands.EditCommand,
            Description = AppConstants.Commands.UpdateRelativeExpressions
         };

         macroCommand.AddRange(expressionProfileUpdate.Select(parameter => updateCommandFor(buildingBlock, parameter.Path, parameter.UpdatedValue)));

         return macroCommand.Run(Context);
      }

      private static ICommand updateCommandFor(ExpressionProfileBuildingBlock buildingBlock, ObjectPath path, double? value)
      {
         var parameterToUpdate = buildingBlock[path];

         if (parameterToUpdate == null)
            return new MoBiEmptyCommand();

         return new PathAndValueEntityValueOrUnitChangedCommand<ExpressionParameter, ExpressionProfileBuildingBlock>(parameterToUpdate, value, parameterToUpdate.DisplayUnit, buildingBlock);
      }

      protected override string GetNewNameForClone(ExpressionProfileBuildingBlock buildingBlockToClone)
      {
         var existingObjectsInParent = Context.CurrentProject.All<ExpressionProfileBuildingBlock>();
         var forbiddenValues = _editTask.GetForbiddenNames(buildingBlockToClone, existingObjectsInParent).ToList();
         var suggestedCategory = $"{buildingBlockToClone.Category} - {AppConstants.Clone}";
            
         return _editTaskForExpressionProfileBuildingBlock.NewNameFromSuggestions(buildingBlockToClone.MoleculeName, buildingBlockToClone.Species, suggestedCategory, buildingBlockToClone.Type, forbiddenValues);
      }
   }
}