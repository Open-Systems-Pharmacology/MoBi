using System.Collections.Generic;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

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

      public override void Rename(Module objectBase, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock)
      {
         //This method is sending null as the last parameter because the building block should not be used in the Rename method
         //The caller of this method uses _activeSubjectRetriever.Active<IBuildingBlock>() to get this value
         //Which sometimes does not match the building block of the object being renamed
         //Causing the rename to check all the used names for it

         base.Rename(objectBase, existingObjectsInParent, null);
      }

   }
}