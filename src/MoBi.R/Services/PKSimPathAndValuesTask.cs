using System;
using System.IO;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using MoBi.R.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.R.Services;

public abstract class PKSimPathAndValuesTask<TBuildingBlock, TBuilder> : PKSimAssemblyLoader, IPathAndValuesTask<TBuildingBlock, TBuilder>
   where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder>
   where TBuilder : PathAndValueEntity
{
   public string[] AllPathsFrom(TBuildingBlock buildingBlock) => buildingBlock.AllPathsFrom();

   public double[] AllValuesFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllValuesFrom(paths);

   public string[] AllDimensionsFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllDimensionsFrom(paths);

   public string[] AllUnitsFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllUnitsFrom(paths);

   public string[] AllValueOriginsFrom(TBuildingBlock buildingBlock, params string[] paths) => buildingBlock.AllValueOriginsFrom(paths);

   private const string PKSIM_R_DLL = "PKSim.R.dll";

   protected override string RetrievePKSimAssemblyPath()
   {
      var assemblyFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), PKSIM_R_DLL);
      if (FileHelper.FileExists(assemblyFile))
         return assemblyFile;

      throw new MoBiException(AppConstants.PKSim.CouldNotFindCompatiblePKSimAssemblies(assemblyFile));
   }

   protected static IMoBiCommand UpdateValueCommandFor(TBuildingBlock buildingBlock, string quantityPath, double newBaseValue)
   {
      var builder = buildingBlock[quantityPath.ToObjectPath()];
      return builder == null ?
         throw new ArgumentException(AppConstants.Exceptions.ParameterNotFoundForPath(quantityPath, buildingBlock.Name)) :
         new PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(builder, newBaseValue, builder.DisplayUnit, buildingBlock);
   }
}