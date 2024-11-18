using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   static class ModuleContentChangeCommandExtensions
   {
      public static ModuleContentChangedCommand<T> AsInverseFor<T>(this ModuleContentChangedCommand<T> inverseCommand, ModuleContentChangedCommand<T> originalCommand) where T : class, IBuildingBlock
      {
         CommandExtensions.AsInverseFor(inverseCommand, originalCommand);
         inverseCommand.WithNewPKSimModuleStateFrom(originalCommand);
         return inverseCommand;
      }
   }
}