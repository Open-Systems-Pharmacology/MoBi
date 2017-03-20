using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_EditTaskForEventBuilder : ContextSpecification<EditTaskForEventBuilder>
   {
      protected override void Context()
      {
         sut = new EditTaskForEventBuilder(A.Fake<IInteractionTaskContext>());
      }
   }
}
