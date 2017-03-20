using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public interface IRemoveRootEventBuilderFromEventGroupBuilderUICommand : IObjectUICommand<IEventBuilder>
   {
   }

   public class RemoveRootEventBuilderFromEventGroupBuilderUICommand : IRemoveRootEventBuilderFromEventGroupBuilderUICommand
   {
      private readonly IInteractionTasksForChildren<IEventGroupBuilder,IEventBuilder> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private IEventGroupBuilder _parent;

      public RemoveRootEventBuilderFromEventGroupBuilderUICommand(IInteractionTasksForChildren<IEventGroupBuilder, IEventBuilder> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
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

      public IObjectUICommand<IEventBuilder> For(IEventBuilder eventBuilder)
      {
         _parent = (IEventGroupBuilder) eventBuilder.ParentContainer;
         Subject = eventBuilder;
         return this;
      }

      public IEventBuilder Subject { get; set; }
   }

   internal class RemoveEventBuilderFromEventGroupBuilderUICommand : ObjectUICommand<IEventGroupBuilder>
   {
      private readonly IInteractionTasksForChildren<IEventGroupBuilder, IEventGroupBuilder> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RemoveEventBuilderFromEventGroupBuilderUICommand(IInteractionTasksForChildren<IEventGroupBuilder, IEventGroupBuilder> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasks.Remove(Subject, (IEventGroupBuilder)Subject.ParentContainer, buildingBlock));
      }
   }
}