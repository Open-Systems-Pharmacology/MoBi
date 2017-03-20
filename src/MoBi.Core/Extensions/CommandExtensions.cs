using OSPSuite.Core.Commands.Core;

namespace MoBi.Core.Extensions
{
   public static class CommandExtensions
   {
      public static T AsHidden<T>(this T command) where T : ICommand
      {
         command.Visible = false;
         return command;
      }
   }
}