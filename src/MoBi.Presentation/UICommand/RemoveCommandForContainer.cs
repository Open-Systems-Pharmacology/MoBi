using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveCommandForContainer : ObjectUICommand<IContainer>
   {
      private readonly IInteractionTasksForChildren<IContainer, IContainer> _interactionTasksContainer;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RemoveCommandForContainer(IInteractionTasksForChildren<IContainer, IContainer> interactionTasksContainer, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasksContainer = interactionTasksContainer;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasksContainer.Remove(Subject, Subject.ParentContainer, buildingBlock));
      }
   }
}