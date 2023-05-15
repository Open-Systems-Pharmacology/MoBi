using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForTransportBuilder : EditTaskFor<TransportBuilder>
   {
      public EditTasksForTransportBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(TransportBuilder transportBuilder, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         if (existingObjectsInParent != null)
            return existingObjectsInParent.AllNames();

         var activePassiveTransportBuildingBlock = getPassiveTransportBuildingBlockFor(transportBuilder);
         return activePassiveTransportBuildingBlock?.Select(x => x.Name) ?? Enumerable.Empty<string>();
      }

      private PassiveTransportBuildingBlock getPassiveTransportBuildingBlockFor(TransportBuilder objectBase)
      {
         var passiveTransportBuildingBlock = _interactionTaskContext.BuildingBlockRepository.PassiveTransportCollection.FirstOrDefault(x => x.Contains(objectBase));
         return passiveTransportBuildingBlock ?? _interactionTaskContext.Active<PassiveTransportBuildingBlock>();
      }
   }
}