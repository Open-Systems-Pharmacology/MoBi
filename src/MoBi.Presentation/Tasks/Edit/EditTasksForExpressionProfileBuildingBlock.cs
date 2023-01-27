using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForExpressionProfileBuildingBlock : IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock>
   {
      string NewNameFromSuggestions(string moleculeName, string species, string category, ExpressionType type, IReadOnlyList<string> prohibitedNames);
   }

   public class EditTasksForExpressionProfileBuildingBlock : EditTasksForBuildingBlock<ExpressionProfileBuildingBlock>, IEditTasksForExpressionProfileBuildingBlock
   {
      public EditTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
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
   }
}