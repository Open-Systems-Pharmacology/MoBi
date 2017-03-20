using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal abstract class AbstractStartValueSubjectRetrieverUICommand<T, TStartValue> : ActiveObjectUICommand<T> where T : class, IStartValuesBuildingBlock<TStartValue> where TStartValue : class, IStartValue
   {
      protected readonly IStartValuesTask<T, TStartValue> _startValueTasks;
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;

      protected AbstractStartValueSubjectRetrieverUICommand(IStartValuesTask<T, TStartValue> startValueTasks, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _startValueTasks = startValueTasks;
         _activeSubjectRetriever = activeSubjectRetriever;
      }
   }
}