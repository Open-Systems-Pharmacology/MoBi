using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;
using System.Threading.Tasks;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IRemoveRootEventBuilderFromEventGroupBuilderUICommand : IObjectUICommand<EventBuilder>
   {
   }

   public class RemoveRootEventBuilderFromEventGroupBuilderUICommand : IRemoveRootEventBuilderFromEventGroupBuilderUICommand
   {
      private readonly IInteractionTasksForChildren<EventGroupBuilder,EventBuilder> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private EventGroupBuilder _parent;

      public RemoveRootEventBuilderFromEventGroupBuilderUICommand(IInteractionTasksForChildren<EventGroupBuilder, EventBuilder> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public void Execute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasks.Remove(Subject, _parent, buildingBlock));
      }

      public IObjectUICommand<EventBuilder> For(EventBuilder eventBuilder)
      {
         _parent = (EventGroupBuilder) eventBuilder.ParentContainer;
         Subject = eventBuilder;
         return this;
      }

      public EventBuilder Subject { get; set; }
   }

   internal class RemoveEventBuilderFromEventGroupBuilderUICommand : ObjectUICommand<EventGroupBuilder>
   {
      private readonly IInteractionTasksForChildren<EventGroupBuilder, EventGroupBuilder> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RemoveEventBuilderFromEventGroupBuilderUICommand(IInteractionTasksForChildren<EventGroupBuilder, EventGroupBuilder> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasks.Remove(Subject, (EventGroupBuilder)Subject.ParentContainer, buildingBlock));
      }
   }
}