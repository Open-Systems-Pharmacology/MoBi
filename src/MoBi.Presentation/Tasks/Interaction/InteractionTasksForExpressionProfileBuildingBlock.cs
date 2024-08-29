using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForExpressionProfileBuildingBlock : IInteractionTasksForProjectBuildingBlock<ExpressionProfileBuildingBlock>,
      IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<ExpressionProfileBuildingBlock, ExpressionParameter>,
      IInteractionTasksForProjectBuildingBlock
   {
      IMoBiCommand UpdateExpressionProfileFromDatabase(ExpressionProfileBuildingBlock buildingBlock);
   }

   public class InteractionTasksForExpressionProfileBuildingBlock : InteractionTasksForProjectPathAndValueEntityBuildingBlocks<ExpressionProfileBuildingBlock, ExpressionParameter>, IInteractionTasksForExpressionProfileBuildingBlock
   {
      private readonly IEditTasksForExpressionProfileBuildingBlock _editTaskForExpressionProfileBuildingBlock;
      private readonly IPKSimStarter _pkSimStarter;
      private readonly IContainerTask _containerTask;

      public InteractionTasksForExpressionProfileBuildingBlock(IInteractionTaskContext interactionTaskContext,
         IEditTasksForExpressionProfileBuildingBlock editTask,
         IMoBiFormulaTask formulaTask,
         IPKSimStarter pkSimStarter,
         IContainerTask containerTask,
         IParameterFactory parameterFactory,
         IExportDataTableToExcelTask exportDataTableToExcelTask,
         IExpressionProfileBuildingBlockToDataTableMapper mapper) :
         base(interactionTaskContext, editTask, formulaTask, parameterFactory, exportDataTableToExcelTask, mapper)
      {
         _editTaskForExpressionProfileBuildingBlock = editTask;
         _pkSimStarter = pkSimStarter;
         _containerTask = containerTask;
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
         var suggestedCategory = $"{buildingBlockToClone.Category} - {AppConstants.Clone}";

         return newNameFromSuggestion(buildingBlockToClone, suggestedCategory);
      }

      private string newNameFromSuggestion(ExpressionProfileBuildingBlock buildingBlockToClone, string suggestedCategory)
      {
         var existingObjectsInParent = Context.CurrentProject.ExpressionProfileCollection;
         var forbiddenValues = _editTask.GetForbiddenNames(buildingBlockToClone, existingObjectsInParent).ToList();

         return _editTaskForExpressionProfileBuildingBlock.NewNameFromSuggestions(buildingBlockToClone.MoleculeName, buildingBlockToClone.Species, suggestedCategory, buildingBlockToClone.Type, forbiddenValues);
      }

      public override IMoBiCommand GetRemoveCommand(ExpressionProfileBuildingBlock expressionProfileToRemove, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new RemoveExpressionProfileBuildingBlockFromProjectCommand(expressionProfileToRemove);
      }

      protected override bool CorrectName(ExpressionProfileBuildingBlock expressionProfile, MoBiProject project)
      {
         var forbiddenNames = project.ExpressionProfileCollection.AllNames();
         if (!forbiddenNames.Contains(expressionProfile.Name))
            return true;

         (_, _, string suggestedCategory) = Constants.ContainerName.NamesFromExpressionProfileName(_containerTask.CreateUniqueName(forbiddenNames, expressionProfile.Name, canUseBaseName: true));

         var newName = newNameFromSuggestion(expressionProfile, suggestedCategory);
         if (string.IsNullOrEmpty(newName))
            return false;

         expressionProfile.Name = newName;
         return true;
      }

      public override IMoBiCommand GetAddCommand(ExpressionProfileBuildingBlock expressionProfileToAdd, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new AddExpressionProfileBuildingBlockToProjectCommand(expressionProfileToAdd);
      }
   }
}