using System.Linq;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RenameObjectCommand<T> : ObjectUICommand<T> where T : class, IObjectBase
   {
      protected IEditTaskFor<T> _editTask;
      protected IActiveSubjectRetriever _activeSubjectRetriever;

      public RenameObjectCommand(IEditTaskFor<T> editTask, IActiveSubjectRetriever activeSubjectRetriever)
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

         _editTask.Rename(Subject, parent, _activeSubjectRetriever.Active<IBuildingBlock>());
      }
   }
}