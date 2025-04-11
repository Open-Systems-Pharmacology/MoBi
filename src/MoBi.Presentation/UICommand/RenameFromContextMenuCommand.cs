using System.Linq;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RenameFromContextMenuCommand<T> : ObjectUICommand<T> where T : class, IObjectBase
   {
      private readonly IEditTaskFor<T> _editTask;

      public RenameFromContextMenuCommand(IEditTaskFor<T> editTask)
      {
         _editTask = editTask;
      }

      protected override void PerformExecute()
      {
         var parent = Enumerable.Empty<IObjectBase>();
         if (Subject is IEntity entity)
            parent = entity.ParentContainer;

         _editTask.Rename(Subject, parent, Subject as IBuildingBlock);
      }
   }
}