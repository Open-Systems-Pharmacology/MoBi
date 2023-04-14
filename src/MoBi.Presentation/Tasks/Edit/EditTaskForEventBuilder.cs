using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaskForEventBuilder : EditTaskFor<EventBuilder>
   {
      public EditTaskForEventBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}