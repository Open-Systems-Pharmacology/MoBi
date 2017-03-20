using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveCommandForContainerAtEventGroup : ObjectUICommand<IContainer>
   {
      private readonly IInteractionTasksForChildren<IEventGroupBuilder, IContainer> _interactionTasksContainerInEventGroup;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RemoveCommandForContainerAtEventGroup(IInteractionTasksForChildren<IEventGroupBuilder, IContainer> interactionTasksContainerInEventGroup, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasksContainerInEventGroup = interactionTasksContainerInEventGroup;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasksContainerInEventGroup.Remove(Subject,Subject.ParentContainer as IEventGroupBuilder,buildingBlock));
      }
   }
}