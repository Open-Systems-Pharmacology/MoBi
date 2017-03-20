using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Extensions
{
   public static class ContainerExtensions
   {
      public static bool CanSetBuildModeForParameters(this IContainer container)
      {
         return container.IsAnImplementationOf<IReactionBuilder>() ||
                container.IsAnImplementationOf<IMoleculeBuilder>();
      }

      public static IEnumerable<ParameterBuildMode> AvailableBuildModeForParameters(this IContainer container)
      {
         yield return ParameterBuildMode.Local;
         yield return ParameterBuildMode.Global;

         if (container.IsAnImplementationOf<IMoleculeBuilder>())
            yield return ParameterBuildMode.Property;
      }

      public static ParameterBuildMode DefaultParameterBuildMode(this IContainer container)
      {
         var needsGlobalParameter = container.IsAnImplementationOf<TransporterMoleculeContainer>() ||
                                    container.IsAnImplementationOf<IReactionBuilder>();

         return needsGlobalParameter ? ParameterBuildMode.Global : ParameterBuildMode.Local;
      }

      public static IContainer WithTag(this IContainer container, string tag)
      {
         container.AddTag(tag);
         return container;
      }

      public static IEnumerable<T> GetChildrenSortedByName<T>(this IContainer container, Func<T, bool> pred) where T : class, IEntity
      {
         return container.GetChildren(pred).OrderBy(x => x.Name);
      }

      public static IEnumerable<T> GetChildrenSortedByName<T>(this IContainer container) where T : class, IEntity
      {
         return container.GetChildren<T>().OrderBy(x => x.Name);
      }
   }
}