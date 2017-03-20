using OSPSuite.Core.Commands.Core;

namespace MoBi.Core.Commands
{
   public static  class SimulationChangeCommandBaseExtensions
   {
      public static T AsInverseFor<T>(this T inverseCommand, SimulationChangeCommandBase originalCommand) where T : SimulationChangeCommandBase
      {
         CommandExtensions.AsInverseFor(inverseCommand, originalCommand);
         inverseCommand.WasChanged = originalCommand.WasChanged;
         inverseCommand.Increment = !originalCommand.Increment;
         return inverseCommand;
      }
 
   }
}