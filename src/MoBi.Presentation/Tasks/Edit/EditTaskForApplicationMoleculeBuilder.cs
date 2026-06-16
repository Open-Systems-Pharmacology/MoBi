using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Edit
{
   public class EditTaskForApplicationMoleculeBuilder : EditTaskFor<ApplicationMoleculeBuilder>
   {
      public EditTaskForApplicationMoleculeBuilder(IInteractionTaskContext interactionTaskContext) : base(interactionTaskContext)
      {
      }
   }
}