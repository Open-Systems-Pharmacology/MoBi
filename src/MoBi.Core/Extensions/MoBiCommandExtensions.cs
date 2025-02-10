using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;

namespace MoBi.Core.Extensions
{
   public static class MoBiCommandExtensions
   {
      public static T RunCommand<T, TExecutionContext>(this T command, TExecutionContext context) where T : ICommand<TExecutionContext> where TExecutionContext : IMoBiContext
      {
         context.PromptForCancellation(command);
         return command.Run(context);
      }
   }
}
