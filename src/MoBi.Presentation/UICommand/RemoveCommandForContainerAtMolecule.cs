using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveCommandForContainerAtMolecule : ObjectUICommand<InteractionContainer>
   {
      public RemoveCommandForContainerAtMolecule(IInteractionTasksForChildren<MoleculeBuilder, InteractionContainer> interactionTasksForInteractionContainer, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasksForInteractionContainer = interactionTasksForInteractionContainer;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      private readonly IInteractionTasksForChildren<MoleculeBuilder, InteractionContainer> _interactionTasksForInteractionContainer;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasksForInteractionContainer.Remove(Subject, Subject.ParentContainer as MoleculeBuilder, buildingBlock));
      }
   }
}