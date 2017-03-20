using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForTransportBuilder : EditTaskFor<ITransportBuilder>
   {
      public EditTasksForTransportBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(ITransportBuilder transportBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         if (existingObjectsInParent != null)
            return existingObjectsInParent.AllNames();

         var activePassiveTransportBuildingBlock = getPassiveTransportBuildingBlockFor(transportBuilder);
         return activePassiveTransportBuildingBlock?.Select(x => x.Name) ?? Enumerable.Empty<string>();
      }

      private IPassiveTransportBuildingBlock getPassiveTransportBuildingBlockFor(ITransportBuilder objectBase)
      {
         var passiveTransportBuildingBlock = _context.CurrentProject.PassiveTransportCollection.FirstOrDefault(x => x.Contains(objectBase));
         return passiveTransportBuildingBlock ?? _interactionTaskContext.Active<IPassiveTransportBuildingBlock>();
      }
   }
}