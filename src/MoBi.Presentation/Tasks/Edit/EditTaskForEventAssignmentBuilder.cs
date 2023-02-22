using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaskForEventAssignmentBuilder : EditTaskFor<IEventAssignmentBuilder>
   {
      public EditTaskForEventAssignmentBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}