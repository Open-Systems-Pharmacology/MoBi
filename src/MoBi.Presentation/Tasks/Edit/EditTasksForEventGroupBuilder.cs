using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForEventGroupBuilder<TEventGroupBuilder> : EditTasksForBuilder<TEventGroupBuilder, IEventGroupBuildingBlock> where TEventGroupBuilder: class, IEventGroupBuilder
   {
      public EditTasksForEventGroupBuilder(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
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
      public EditTasksForApplicationBuilder(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
      {
      }
   }


   public class EditTasksForEventGroupBuilder: EditTasksForEventGroupBuilder<IEventGroupBuilder> 
   {
      public EditTasksForEventGroupBuilder(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
      {
      }
   }
}