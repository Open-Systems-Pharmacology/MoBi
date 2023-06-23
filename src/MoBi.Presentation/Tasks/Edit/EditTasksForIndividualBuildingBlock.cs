using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForIndividualBuildingBlock : IEditTasksForBuildingBlock<IndividualBuildingBlock>
   {
   }
   
   public class EditTasksForIndividualBuildingBlock : EditTasksForBuildingBlock<IndividualBuildingBlock>, IEditTasksForIndividualBuildingBlock
   {
      public EditTasksForIndividualBuildingBlock(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }

      protected override IEnumerable<string> GetUnallowedNames(IndividualBuildingBlock objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return base.GetUnallowedNames(objectBase, existingObjectsInParent).Concat(_interactionTaskContext.BuildingBlockRepository.IndividualsCollection.AllNames());
      }
   }
}