using OSPSuite.Utility.Container;

namespace MoBi.UI.Extensions;

public static class UtilityContainerExtensions
{
   public static void Register<TService1, TService2, TService3, TService4, TService5, TImplementation>(this IContainer container, LifeStyle lifeStyle)
   {
      // Existing extensions in OSPSuite.Infrastructure only go up to 4 services
      container.Register([
         typeof(TService1),
         typeof(TService2),
         typeof(TService3),
         typeof(TService4),
         typeof(TService5)], typeof(TImplementation), lifeStyle);
   }
}