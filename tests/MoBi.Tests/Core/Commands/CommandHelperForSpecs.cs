using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;

namespace MoBi.Core.Commands
{
   public static class CommandHelperForSpecs
   {
      public static IReversibleCommand<IMoBiContext> ExecuteAndInvokeInverse(this IReversibleCommand<IMoBiContext> command, IMoBiContext context)
      {
         command.Execute(context);
         return command.InvokeInverse(context);
      }

      public static IReversibleCommand<IMoBiContext> InvokeInverse(this IReversibleCommand<IMoBiContext> command, IMoBiContext context)
      {
         command.RestoreExecutionData(context);
         var inverse = command.InverseCommand(context);
         inverse.Execute(context);
         return inverse;
      }
   }
}