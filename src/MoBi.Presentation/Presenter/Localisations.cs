using System;

namespace MoBi.Presentation.Presenter
{
   [Flags]
   public enum Localisations
   {
      ContainerOnly = 2 << 0,
      NeighborhoodsOnly = 2 << 1,
      PhysicalOnly = 2 << 2,
      LogicalOnly = 2 << 2,
      PhysicalContainerOnly = ContainerOnly + PhysicalOnly,
      Everywhere = ContainerOnly + NeighborhoodsOnly + LogicalOnly + PhysicalOnly
   }

   public static class LocalisationsExtensions
   {
      public static bool Is(this Localisations localisations, Localisations typeToCompare)
      {
         return (localisations & typeToCompare) != 0;
      }
   }
}