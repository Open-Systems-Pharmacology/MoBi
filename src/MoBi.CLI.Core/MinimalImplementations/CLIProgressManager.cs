using OSPSuite.Utility.Events;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class CLIProgressManager : IProgressManager
   {
      private readonly IEventPublisher _eventPublisher;

      public CLIProgressManager(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public IProgressUpdater Create()
      {
         return new ProgressUpdater(_eventPublisher);
      }
   }
}