using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Extensions
{
   public static class ObjectPathExtensions
   {
      /// <summary>
      /// Creates a new object path that does not conain the last entry defined in <paramref name="objectPath"/>
      /// </summary>
      public static IObjectPath ContainerPath(this IReadOnlyList<string> objectPath)
      {
         var containerPath = new List<string>();
         for (int i = 0; i < objectPath.Count - 1; i++)
         {
            containerPath.Add(objectPath[i]);
         }
         return new ObjectPath(containerPath);
      }
   }
}