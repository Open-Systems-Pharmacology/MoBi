using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public abstract class AbstractStartValueSubjectRetrieverUICommand<T, TPathAndValueEntity> : ActiveObjectUICommand<T> where T : PathAndValueEntityBuildingBlock<TPathAndValueEntity> where TPathAndValueEntity : PathAndValueEntity
   {
      protected readonly IInteractionTasksForExtendablePathAndValueEntity<T, TPathAndValueEntity> _startValueTasks;
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;

      protected AbstractStartValueSubjectRetrieverUICommand(IInteractionTasksForExtendablePathAndValueEntity<T, TPathAndValueEntity> startValueTasks, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _startValueTasks = startValueTasks;
         _activeSubjectRetriever = activeSubjectRetriever;
      }
   }
}