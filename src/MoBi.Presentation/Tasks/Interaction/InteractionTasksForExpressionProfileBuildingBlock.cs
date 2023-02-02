using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForExpressionProfileBuildingBlock : IInteractionTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IInteractionTasksForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      IReadOnlyList<ExpressionProfileBuildingBlock> LoadFromPKML();
   }

   public class InteractionTasksForExpressionProfileBuildingBlock : InteractionTasksForPathAndValueEntity<ExpressionProfileBuildingBlock, ExpressionParameter>, IInteractionTasksForExpressionProfileBuildingBlock
   {
      private readonly IEditTasksForExpressionProfileBuildingBlock _editTaskForExpressionProfileBuildingBlock;

      public InteractionTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForExpressionProfileBuildingBlock editTask, IMoBiFormulaTask formulaTask) :
         base(interactionTaskContext, editTask, formulaTask)
      {
         _editTaskForExpressionProfileBuildingBlock = editTask;
      }

      public IReadOnlyList<ExpressionProfileBuildingBlock> LoadFromPKML()
      {
         var filename = AskForPKMLFileToOpen();
         return (string.IsNullOrEmpty(filename) ? Enumerable.Empty<ExpressionProfileBuildingBlock>() : LoadItems(filename)).ToList();
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