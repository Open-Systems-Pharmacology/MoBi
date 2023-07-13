using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForInitialConditionsBuildingBlock : EditTasksForBuildingBlock<InitialConditionsBuildingBlock>
   {
      public EditTasksForInitialConditionsBuildingBlock(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(InitialConditionsBuildingBlock buildingBlock, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         // Add all the initial conditions building blocks from the parent module.
         return base.GetUnallowedNames(buildingBlock, existingObjectsInParent).Concat(buildingBlock.Module.InitialConditionsCollection.AllNames());
      }
   }
}