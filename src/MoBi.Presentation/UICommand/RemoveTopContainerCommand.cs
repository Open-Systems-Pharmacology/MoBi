using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveTopContainerCommand : ObjectUICommand<IContainer>
   {
      private readonly IInteractionTasksForTopContainer _editTasksForTopContainer;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RemoveTopContainerCommand(IInteractionTasksForTopContainer editTasksForTopContainer, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _editTasksForTopContainer = editTasksForTopContainer;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<MoBiSpatialStructure>();
         _context.AddToHistory(_editTasksForTopContainer.Remove(Subject, buildingBlock,buildingBlock));
      }
   }
}