using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Extensions;

public static class PathAndValueEntityBuildingBlockExtensions
{
   public static string[] AllPathsFrom<TBuilder>(this ILookupBuildingBlock<TBuilder> buildingBlock) where TBuilder : PathAndValueEntity
      => AllFrom(buildingBlock, paths: null, x => x.Path.PathAsString);

   public static double[] AllValuesFrom<TBuilder>(this ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) where TBuilder : PathAndValueEntity
      => AllFrom(buildingBlock, paths, x => x.Value ?? double.NaN);

   public static string[] AllDimensionsFrom<TBuilder>(this ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) where TBuilder : PathAndValueEntity
      => AllFrom(buildingBlock, paths, x => x.Dimension.Name);

   public static string[] AllUnitsFrom<TBuilder>(this ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) where TBuilder : PathAndValueEntity
      => AllFrom(buildingBlock, paths, x => x.Dimension.BaseUnit.Name);

   public static string[] AllValueOriginsFrom<TBuilder>(this ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) where TBuilder : PathAndValueEntity
      => AllFrom(buildingBlock, paths, x => x.ValueOrigin.Display);

   public static T[] AllFrom<TBuilder, T>(this ILookupBuildingBlock<TBuilder> buildingBlock, string[] paths, Func<TBuilder, T> selector) where TBuilder : PathAndValueEntity
      => AllFrom((IEnumerable<TBuilder>)buildingBlock, paths, selector);

   public static T[] AllFrom<TBuilder, T>(this IEnumerable<TBuilder> buildingBlock, string[] paths, Func<TBuilder, T> selector) where TBuilder : PathAndValueEntity
   {
      if (paths != null && paths.Length != 0)
      {
         var pathSet = new HashSet<string>(paths);

         if (pathSet.Count != paths.Length)
            throw new ArgumentException(AppConstants.Exceptions.DuplicatePathsInInput(paths.Length, pathSet.Count));

         var lookup = buildingBlock.Where(x => pathSet.Contains(x.Path)).ToDictionary(x => x.Path.PathAsString);

         if (lookup.Count != paths.Length)
            throw new ArgumentException(AppConstants.Exceptions.NotAllPathsFoundInBuildingBlock(paths.Length, lookup.Count));

         return paths.Select(p => selector(lookup[p])).ToArray();
      }

      return buildingBlock.Select(selector).ToArray();
   }
}
