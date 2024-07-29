using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;
using System.Collections.Generic;

namespace MoBi.Presentation.UICommand
{
   public class SaveUICommandForMultiple<T> : ObjectUICommand<IReadOnlyList<T>> where T : class, IObjectBase
   {
      private readonly IEditTaskFor<T> _editTasks;

      public SaveUICommandForMultiple(IEditTaskFor<T> editTasks)
      {
         _editTasks = editTasks;
      }

      protected override void PerformExecute() =>
         _editTasks.SaveMultiple(Subject);
   }
}