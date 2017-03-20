using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForEventGroupBuilder<TEventGroupBuilder> : EditTasksForBuilder<TEventGroupBuilder, IEventGroupBuildingBlock> where TEventGroupBuilder: class, IEventGroupBuilder
   {
      public EditTasksForEventGroupBuilder(IInteractionTaskContext interactionTaskContext)
         : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(TEventGroupBuilder objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         if (existingObjectsInParent != null)
            return existingObjectsInParent.AllNames();

         //TopEventGroup
         var eventGroupBuildingBlock = _interactionTaskContext.Active<IEventGroupBuildingBlock>();
         if (eventGroupBuildingBlock == null)
            return Enumerable.Empty<string>();
         return eventGroupBuildingBlock.Select(x => x.Name);
      }

   }

   public class EditTasksForApplicationBuilder : EditTasksForEventGroupBuilder<IApplicationBuilder> 
   {
      public EditTasksForApplicationBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }


   public class EditTasksForEventGroupBuilder: EditTasksForEventGroupBuilder<IEventGroupBuilder> 
   {
      public EditTasksForEventGroupBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}