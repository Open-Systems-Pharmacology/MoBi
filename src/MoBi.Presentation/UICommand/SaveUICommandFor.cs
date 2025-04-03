using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class SaveUICommandFor<T> : ObjectUICommand<T> where T: class, IObjectBase
   {
      private readonly IEditTaskFor<T> _editTasks;

      public SaveUICommandFor(IEditTaskFor<T> editTasks)
      {
         _editTasks = editTasks;
      }

      protected override void PerformExecute()
      {
         _editTasks.Save(Subject);
      }
   }
}