using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   static class BuildingBlockChangeBaseCommandExtensions
   {
      public static BuildingBlockChangeCommandBase<T> AsInverseFor<T>(this BuildingBlockChangeCommandBase<T> inverseCommand, BuildingBlockChangeCommandBase<T> originalCommand) where T : class, IBuildingBlock 
      {
         CommandExtensions.AsInverseFor(inverseCommand, originalCommand);
         inverseCommand.ShouldIncrementVersion = !originalCommand.ShouldIncrementVersion;
         return inverseCommand;
      }
   }
}