using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForObserverBuilder<TBuilder> : EditTaskFor<TBuilder> where TBuilder : class, IObserverBuilder
   {
      public EditTasksForObserverBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(TBuilder observerBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         if (existingObjectsInParent != null)
            return base.GetUnallowedNames(observerBuilder, existingObjectsInParent);

         var activeObservers = getObserverBuildingBlockFor(observerBuilder);
         return activeObservers == null ? Enumerable.Empty<string>() : activeObservers.Select(x => x.Name);
      }

      private IObserverBuildingBlock getObserverBuildingBlockFor(TBuilder objectBase)
      {
         var observerBuildingBlock = _context.CurrentProject.ObserverBlockCollection.FirstOrDefault(x => x.Contains(objectBase));
         return observerBuildingBlock ?? _interactionTaskContext.Active<IObserverBuildingBlock>();
      }
   }

   public class EditTasksForAmountObserverBuilder : EditTasksForObserverBuilder<IAmountObserverBuilder>
   {
      public EditTasksForAmountObserverBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }

   public class EditTasksForContainerObserverBuilder : EditTasksForObserverBuilder<IContainerObserverBuilder>
   {
      public EditTasksForContainerObserverBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}