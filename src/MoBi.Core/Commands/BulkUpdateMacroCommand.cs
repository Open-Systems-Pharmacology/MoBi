using MoBi.Core.Domain.Model;
using MoBi.Core.Events;

namespace MoBi.Core.Commands
{
   public class BulkUpdateMacroCommand : MoBiMacroCommand
   {
      public override void Execute(IMoBiContext context)
      {
         try
         {
            context.PublishEvent(new BulkUpdateStartedEvent());
            base.Execute(context);
         }
         finally
         {
            context.PublishEvent(new BulkUpdateFinishedEvent());
         }
      }
   }
}
