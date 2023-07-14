using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   public class RemoveCommandFor<TParent,TChild> : IUICommand where TParent : class where TChild : class
   {
      protected readonly IInteractionTasksForChildren<TParent,TChild> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      
      public RemoveCommandFor(IInteractionTasksForChildren<TParent,TChild> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public void Execute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasks.Remove(Child,Parent, buildingBlock));
      }

      public TChild Child { get; set; }

      public TParent Parent { get; set; }
   }
}