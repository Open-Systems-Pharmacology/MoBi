using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Extensions
{
   public static class BuildingBlockExtensions
   {
      public static ICache<string, T> ToCache<T>(this IEnumerable<T> pathAndValueEntities) where T : PathAndValueEntity
      {
         var cache = new Cache<string, T>(x => x.Path.ToString(), x => null);
         cache.AddRange(pathAndValueEntities);
         return cache;
      }

      public static ICache<string, T> ToCache<T>(this IEnumerable<T> elements, Func<T, string> getKey) where T : class
      {
         var cache = new Cache<string, T>(getKey, x => null);
         cache.AddRange(elements);
         return cache;
      }

      public static IReadOnlyList<IFormula> UniqueFormulasByName<T>(this IEnumerable<T> buildingBlock) where T : IUsingFormula
      {
         var formulaCache = new Cache<string, IFormula>(getKey: x => x.Name);
         buildingBlock.Where(x => x.Formula != null).Each(x => formulaCache[x.Formula.Name] = x.Formula);

         return formulaCache.ToList();
      }
   }

   public static class MoleculeBuildingBlockExtensions
   {
      public static bool ContainsBuilder(this MoleculeBuildingBlock buildingBlock, IObjectBase entity)
      {
         if (!entity.CouldBeInMoleculeBuildingBlock())
            return false;

         if (entity.IsAnImplementationOf<MoleculeBuilder>())
            return buildingBlock.Contains((MoleculeBuilder)entity);

         if (entity.IsAnImplementationOf<TransportBuilder>())
         {
            var transporterMoleculeContainers = buildingBlock.SelectMany(mb => mb.TransporterMoleculeContainerCollection);
            var allTransports = transporterMoleculeContainers.SelectMany(x => x.ActiveTransportRealizations);
            return allTransports.Any(child => child.Equals(entity));
         }

         var allChildren = buildingBlock.SelectMany(mb => mb.GetAllChildren<IEntity>());
         return allChildren.Any(child => child.Equals(entity));
      }
   }
}