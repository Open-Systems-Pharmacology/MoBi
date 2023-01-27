using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTasksForDistributedParameter : EditTaskFor<IDistributedParameter>
   {
      public EditTasksForDistributedParameter(IInteractionTaskContext interactionTaskContext, IObjectTypeResolver objectTypeResolver, ICheckNameVisitor checkNamesVisitor) : base(interactionTaskContext, objectTypeResolver, checkNamesVisitor)
      {
      }
   }
}