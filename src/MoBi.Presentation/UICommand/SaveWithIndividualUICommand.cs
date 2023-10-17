using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class SaveWithIndividualUICommand : ObjectUICommand<IContainer>
   {
      private readonly IEditTaskForContainer _editTaskForContainer;

      public SaveWithIndividualUICommand(IEditTaskForContainer editTaskForContainer)
      {
         _editTaskForContainer = editTaskForContainer;
      }
      protected override void PerformExecute()
      {
         _editTaskForContainer.SaveWithIndividual(Subject);
      }
   }
}