using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Captions;
using static MoBi.Assets.AppConstants.Dialog;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaskForNeighborhoodBuilder : EditTaskFor<NeighborhoodBuilder>
   {
      public EditTaskForNeighborhoodBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(NeighborhoodBuilder objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var spatialStructure = getSpatialStructure();
         return spatialStructure.Neighborhoods.Select(x => x.Name).Union(AppConstants.UnallowedNames);
      }

      private MoBiSpatialStructure getSpatialStructure() => _interactionTaskContext.Active<MoBiSpatialStructure>();

      public override bool EditEntityModal(NeighborhoodBuilder neighborhood, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         // Neighborhood is connected if we are editing creating from the diagram
         if (neighborhoodIsConnected(neighborhood))
            return editConnectedNeighborhood(neighborhood, existingObjectsInParent);

         //or we are creating the neighborhood from scratch
         return base.EditEntityModal(neighborhood, existingObjectsInParent, commandCollector, buildingBlock);
      }

      private bool neighborhoodIsConnected(NeighborhoodBuilder neighborhood) =>
         neighborhood.FirstNeighborPath != null && neighborhood.SecondNeighborPath != null;

      private bool editConnectedNeighborhood(NeighborhoodBuilder neighborhood, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var title = AskForNewNeighborhoodBuilderName(neighborhood.FirstNeighborPath, neighborhood.SecondNeighborPath);
         var name = _interactionTaskContext.NewName(title, NewWindow(ObjectName), forbiddenValues: GetForbiddenNamesWithoutSelf(neighborhood, existingObjectsInParent));
         if (name.IsNullOrEmpty())
            return false;

         neighborhood.Name = name;
         return true;
      }
   }
}