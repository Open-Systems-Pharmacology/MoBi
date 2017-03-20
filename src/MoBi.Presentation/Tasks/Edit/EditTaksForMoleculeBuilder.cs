using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaksForMoleculeBuilder : EditTasksForBuilder<IMoleculeBuilder, IMoleculeBuildingBlock>
   {
      public EditTaksForMoleculeBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}