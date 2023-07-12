using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForParameterValuesBuildingBlock : EditTasksForBuildingBlock<ParameterValuesBuildingBlock>
   {
      public EditTasksForParameterValuesBuildingBlock(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(ParameterValuesBuildingBlock buildingBlock, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         // Add all the parameter values building blocks from the parent module
         return base.GetUnallowedNames(buildingBlock, existingObjectsInParent).Concat(buildingBlock.Module.ParameterValuesCollection.AllNames());
      }
   }
}