using System.Linq;
using MoBi.Assets;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;

namespace MoBi.Presentation.Tasks
{
   public interface IExpressionProfileTask
   {
      void LoadExpressionProfileIntoProject();
   }

   public class ExpressionProfileTask : IExpressionProfileTask
   {
      private readonly IInteractionTask _interactionTask;

      public ExpressionProfileTask(IInteractionTask interactionTask)
      {
         _interactionTask = interactionTask;
      }

      public void LoadExpressionProfileIntoProject()
      {
         var filename = _interactionTask.AskForFileToOpen(AppConstants.Dialog.Load("Expression Profile"), Filter.PKML_FILE_FILTER,
            DirectoryKey.MODEL_PART);
         if (filename.IsNullOrEmpty())
            return;

         //first or default since we have one expression profile per pkml
         var expressionProfile = _interactionTask.LoadItems<ExpressionProfileBuildingBlock>(filename).FirstOrDefault();
         if (expressionProfile == null)
            return;

         //add the loaded items to project
      }
   }
}