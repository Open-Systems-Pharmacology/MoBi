using System;

namespace MoBi.Core
{
   [Flags]
   public enum MessageOrigin
   {
      Formula = 2 << 0,
      Simulation = 2 << 1,
      All = Formula | Simulation
   }

   public static class MessageOriginExtensions
   {
      public static bool Is(this MessageOrigin messageOrigin, MessageOrigin messageOriginToCompare)
      {
         return (messageOrigin & messageOriginToCompare) != 0;
      }
   }
}