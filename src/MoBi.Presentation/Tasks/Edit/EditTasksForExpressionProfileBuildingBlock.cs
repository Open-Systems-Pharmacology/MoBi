using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForExpressionProfileBuildingBlock : IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock>
   {
      string NewNameFromSuggestions(string moleculeName, string species, string category, ExpressionType type, IReadOnlyList<string> prohibitedNames);
   }

   public class EditTasksForExpressionProfileBuildingBlock : EditTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IEditTasksForExpressionProfileBuildingBlock
   {
      public EditTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override string NewNameFor(ExpressionProfileBuildingBlock expressionProfile, IReadOnlyList<string> prohibitedNames)
      {
         return NewNameFromSuggestions(expressionProfile.MoleculeName, expressionProfile.Species, expressionProfile.Category, expressionProfile.Type, prohibitedNames);
      }

      public string NewNameFromSuggestions(string moleculeName, string species, string category, ExpressionType type, IReadOnlyList<string> prohibitedNames)
      {
         using (var renameExpressionProfilePresenter = _applicationController.Start<INewNameForExpressionProfileBuildingBlockPresenter>())
         {
            return renameExpressionProfilePresenter.NewNameFrom(moleculeName, species, category, type, prohibitedNames);
         }
      }

      protected override RenameObjectBaseCommand GetRenameCommandFor(ExpressionProfileBuildingBlock objectBase, IBuildingBlock buildingBlock, string newName, string objectName)
      {
         return new RenameExpressionProfileBuildingBlockCommand(objectBase, newName, buildingBlock) { ObjectType = objectName };
      }
   }
}