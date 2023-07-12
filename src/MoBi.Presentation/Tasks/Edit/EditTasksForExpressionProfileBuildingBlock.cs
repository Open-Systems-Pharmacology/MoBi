using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Commands;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForExpressionProfileBuildingBlock : IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock>
   {
      string NewNameFromSuggestions(string moleculeName, string species, string category, ExpressionType type, IReadOnlyList<string> prohibitedNames, bool isRename = false);
   }

   public class EditTasksForExpressionProfileBuildingBlock : EditTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IEditTasksForExpressionProfileBuildingBlock
   {
      public EditTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override string NewNameFor(ExpressionProfileBuildingBlock expressionProfile, IReadOnlyList<string> prohibitedNames)
      {
         return NewNameFromSuggestions(expressionProfile.MoleculeName, expressionProfile.Species, expressionProfile.Category, expressionProfile.Type, prohibitedNames, isRename: true);
      }

      public string NewNameFromSuggestions(string moleculeName, string species, string category, ExpressionType type, IReadOnlyList<string> prohibitedNames, bool isRename)
      {
         using (var renameExpressionProfilePresenter = _applicationController.Start<INewNameForExpressionProfileBuildingBlockPresenter>())
         {
            return renameExpressionProfilePresenter.NewNameFrom(moleculeName, species, category, type, prohibitedNames, isRename);
         }
      }

      protected override IEnumerable<string> GetUnallowedNames(ExpressionProfileBuildingBlock objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return base.GetUnallowedNames(objectBase, existingObjectsInParent).Concat(_interactionTaskContext.BuildingBlockRepository.ExpressionProfileCollection.AllNames());
      }

      protected override IMoBiCommand GetRenameCommandFor(ExpressionProfileBuildingBlock expressionProfileBuildingBlock, IBuildingBlock buildingBlock, string newName, string objectType)
      {
         return new RenameExpressionProfileBuildingBlockCommand(expressionProfileBuildingBlock, newName, buildingBlock) { ObjectType = objectType };
      }
   }
}