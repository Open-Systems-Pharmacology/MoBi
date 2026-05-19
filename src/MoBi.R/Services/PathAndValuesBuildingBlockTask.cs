using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public interface IPathAndValuesTask<TBuildingBlock, TBuilder> : IBuildingBlockTask<TBuildingBlock>
   where TBuildingBlock : ILookupBuildingBlock<TBuilder>
   where TBuilder : PathAndValueEntity
{
   string[] AllPathsFrom(ILookupBuildingBlock<TBuilder> buildingBlock);
   double[] AllValuesFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths);
   string[] AllDimensionsFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths);
   string[] AllUnitsFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths);
   string[] AllValueOriginsFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths);
   void ExportToPKML(TBuildingBlock buildingBlock, string filePath);
}

public abstract class PathAndValuesBuildingBlockTask<TBuildingBlock, TBuilder> : BuildingBlockTask<TBuildingBlock>, IPathAndValuesTask<TBuildingBlock, TBuilder>
   where TBuildingBlock : ILookupBuildingBlock<TBuilder>
   where TBuilder : PathAndValueEntity
{
   protected readonly IXmlSerializationService _xmlSerializationService;

   protected PathAndValuesBuildingBlockTask(ISerializationTask serializationTask, IXmlSerializationService xmlSerializationService) : base(serializationTask)
   {
      _xmlSerializationService = xmlSerializationService;
   }

   public void ExportToPKML(TBuildingBlock buildingBlock, string filePath) =>
      _xmlSerializationService.SerializeModelPart(buildingBlock).PermissiveSave(filePath);

   public string[] AllPathsFrom(ILookupBuildingBlock<TBuilder> buildingBlock) => AllFrom(buildingBlock, paths: null, x => x.Path.PathAsString);

   public double[] AllValuesFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) => AllFrom(buildingBlock, paths, x => x.Value ?? double.NaN);

   public string[] AllDimensionsFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) => AllFrom(buildingBlock, paths, x => x.Dimension.Name);

   public string[] AllUnitsFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) => AllFrom(buildingBlock, paths, x => x.Dimension.BaseUnit.Name);

   public string[] AllValueOriginsFrom(ILookupBuildingBlock<TBuilder> buildingBlock, params string[] paths) => AllFrom(buildingBlock, paths, x => x.ValueOrigin.Display);

   protected static T[] AllFrom<T>(ILookupBuildingBlock<TBuilder> buildingBlock, string[] paths, Func<TBuilder, T> selector) =>
      AllFrom((IEnumerable<TBuilder>)buildingBlock, paths, selector);

   protected static T[] AllFrom<T>(IEnumerable<TBuilder> buildingBlock, string[] paths, Func<TBuilder, T> selector)
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