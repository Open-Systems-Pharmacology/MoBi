using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class SaveWithIndividualAndExpressionUICommand : ObjectUICommand<IContainer>
   {
      private readonly IEditTaskForContainer _editTaskForContainer;

      public SaveWithIndividualAndExpressionUICommand(IEditTaskForContainer editTaskForContainer)
      {
         _editTaskForContainer = editTaskForContainer;
      }

      protected override void PerformExecute()
      {
         _editTaskForContainer.SaveWithIndividualAndExpression(Subject);
      }
   }
}