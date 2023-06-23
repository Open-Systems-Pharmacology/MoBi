using System.Collections.Generic;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskForModule : IEditTaskFor<Module>
   {
   }

   public class EditTaskForModule : EditTasksForSimpleNameObjectBase<Module>, IEditTaskForModule
   {
      public EditTaskForModule(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(Module objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return _context.CurrentProject.Modules.AllNames();
      }
   }
}