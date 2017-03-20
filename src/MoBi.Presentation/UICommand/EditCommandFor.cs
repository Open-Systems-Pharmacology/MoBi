using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class EditCommandFor<T> : ObjectUICommand<T> where T : class, IObjectBase
   {
      private readonly IEditTaskFor<T> _editTask;

      public EditCommandFor(IEditTaskFor<T> editTask)
      {
         _editTask = editTask;
      }

      protected override void PerformExecute()
      {
         _editTask.Edit(Subject);
      }
   }
}