using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForNeighborhoodBuilder : EditTaskFor<INeighborhoodBuilder>
   {
      public EditTasksForNeighborhoodBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(INeighborhoodBuilder objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var spatialStructure = getSpatialStructure();
         return spatialStructure.Neighborhoods.Select(x => x.Name).Union(AppConstants.UnallowedNames);
      }

      private IMoBiSpatialStructure getSpatialStructure()
      {
         return _interactionTaskContext.Active<IMoBiSpatialStructure>();
      }

      public override bool EditEntityModal(INeighborhoodBuilder entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         var name = _interactionTaskContext.DialogCreator.AskForInput(AppConstants.Dialog.AskForNewNeighborhoodBuilderName(entity.FirstNeighbor.Name, entity.SecondNeighbor.Name),
            AppConstants.Captions.NewWindow(ObjectName), String.Empty,
            GetForbiddenNamesWithoutSelf(entity, existingObjectsInParent));
         if (name.IsNullOrEmpty())
            return false;

         entity.Name = name;
         return true;
      }
   }
}