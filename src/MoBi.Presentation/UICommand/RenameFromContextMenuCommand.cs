using System.Linq;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RenameFromContextMenuCommand<T> : ObjectUICommand<T> where T : class, IObjectBase
   {
      protected IEditTaskFor<T> _editTask;
      protected IActiveSubjectRetriever _activeSubjectRetriever;

      public RenameFromContextMenuCommand(IEditTaskFor<T> editTask, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _editTask = editTask;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var parent = Enumerable.Empty<IObjectBase>();
         var entity = Subject as IEntity;
         if (entity != null)
            parent = entity.ParentContainer;

         _editTask.Rename(Subject, parent, Subject as IBuildingBlock);
      }
   }
}