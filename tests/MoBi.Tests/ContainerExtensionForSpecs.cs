using OSPSuite.Core.Domain;

namespace MoBi
{
   public static class ContainerExtensionForSpecs
   {
      public static IContainer WithChildContainerNamed(this IContainer container, string childContainerName)
      {
         var child = new Container { Name = childContainerName };
         container.Add(child);
         return child;
      }
   }
}