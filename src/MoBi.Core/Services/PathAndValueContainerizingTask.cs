using MoBi.Core.Domain.Extensions;
using MoBi.Core.Extensions;
using NHibernate.Loader.Custom;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Core.Services
{
   public interface IPathAndValueContainerizingTask
   {
      /// <summary>
      ///    Builds a synthetic container tree for the given building block, grouping entities by
      ///    their <see cref="PathAndValueEntity.ContainerPath" />. The root of the synthetic tree is
      ///    stored in <paramref name="cache" /> keyed by the building block so that subsequent lookups
      ///    (<see cref="ChildrenFor" />, <see cref="IsInCachedTree" />) can find it.
      /// </summary>
      IReadOnlyList<IObjectBase> ChildrenFor<TBuildingBlock, TEntity>(TBuildingBlock buildingBlock, ICache<IBuildingBlock, IContainer> cache)
         where TBuildingBlock : PathAndValueEntityBuildingBlock<TEntity>
         where TEntity : PathAndValueEntity;

      /// <summary>
      ///    Returns the ordered children of a container that is part of any synthetic tree previously
      ///    built into <paramref name="cache" />. Returns an empty list when the container is not in
      ///    the cache.
      /// </summary>
      IReadOnlyList<IObjectBase> ChildrenFor(IContainer container, ICache<IBuildingBlock, IContainer> cache);

      /// <summary>
      ///    Indicates whether <paramref name="container" /> belongs to any synthetic tree previously
      ///    built into <paramref name="cache" />.
      /// </summary>
      bool IsInCachedTree(IContainer container, ICache<IBuildingBlock, IContainer> cache);
   }

   public class PathAndValueContainerizingTask : IPathAndValueContainerizingTask
   {
      public IReadOnlyList<IObjectBase> ChildrenFor<TBuildingBlock, TEntity>(TBuildingBlock buildingBlock, ICache<IBuildingBlock, IContainer> cache)
         where TBuildingBlock : PathAndValueEntityBuildingBlock<TEntity>
         where TEntity : PathAndValueEntity
      {
         if (buildingBlock == null)
            return new List<IObjectBase>();

         if (cache.Contains(buildingBlock))
            return orderedChildrenOf(cache[buildingBlock]); ;

         var orderedEntities = buildingBlock.OrderBy(x => x.Path.PathAsString).ToList();
         var rootContainer = buildGroups(entitiesExceptSubParameters(orderedEntities));
         cache[buildingBlock] = rootContainer;

         orderedEntities.Each(x => addToContainer(x, rootContainer));

         return orderedChildrenOf(rootContainer);
      }

      public IReadOnlyList<IObjectBase> ChildrenFor(IContainer container, ICache<IBuildingBlock, IContainer> cache)
      {
         return IsInCachedTree(container, cache) ? orderedChildrenOf(container) : new List<IObjectBase>();
      }

      public bool IsInCachedTree(IContainer container, ICache<IBuildingBlock, IContainer> cache)
      {
         return container != null && cache.Any(x => x.GetAllContainersAndSelf<IContainer>().Contains(container));
      }

      private static IReadOnlyList<IObjectBase> orderedChildrenOf(IContainer container)
      {
         var result = new List<IObjectBase>();
         result.AddRange(container.GetChildrenSortedByName<IContainer>());
         result.AddRange(container.GetChildrenSortedByName<PathAndValueEntity>());
         return result;
      }

      private static IReadOnlyList<TEntity> entitiesExceptSubParameters<TEntity>(IReadOnlyList<TEntity> entities) where TEntity : PathAndValueEntity
      {
         return entities.Where(x => !isSubParameter(x, entities)).ToList();
      }

      private static bool isSubParameter<TEntity>(TEntity entity, IReadOnlyList<TEntity> entities) where TEntity : PathAndValueEntity
      {
         return entities.Any(entity.IsDirectSubParameterOf);
      }

      private static void addToContainer(PathAndValueEntity entity, IContainer container)
      {
         var parentContainer = entity.ContainerPath.TryResolve<IContainer>(container);
         parentContainer?.Add(entity);
      }

      private static IContainer buildGroups<TEntity>(IReadOnlyList<TEntity> entities) where TEntity : PathAndValueEntity
      {
         var rootContainer = new Container();

         // construct a new object path to avoid changing the original object path
         entities.Select(x => new ObjectPath(x.ContainerPath)).ToList()
            .Where(x => x.Any())
            .GroupBy(x => x.First())
            .Each(x => addContainersFor(x, rootContainer));

         return rootContainer;
      }

      private static void addContainersFor(IGrouping<string, ObjectPath> group, IContainer rootContainer)
      {
         if (string.IsNullOrEmpty(group.Key))
            return;

         var groupContainer = new Container().WithName(group.Key);
         group.Each(x => x.RemoveFirst());
         group.Where(x => x.Any()).GroupBy(x => x.First()).Each(x => addContainersFor(x, groupContainer));
         rootContainer.Add(groupContainer);
      }
   }
}