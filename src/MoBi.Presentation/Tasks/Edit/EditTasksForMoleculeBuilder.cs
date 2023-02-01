using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForMoleculeBuilder : EditTasksForBuilder<IMoleculeBuilder, IMoleculeBuildingBlock>
   {
      public EditTasksForMoleculeBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}