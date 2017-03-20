using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForReactionBuilder : EditTasksForBuilder<IReactionBuilder, IReactionBuildingBlock>
   {
      public EditTasksForReactionBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}