using System;
using System.IO;
using System.Reflection;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public abstract class PKSimPathAndValuesTask<TBuildingBlock, TBuilder> : PathAndValuesBuildingBlockTask<TBuildingBlock, TBuilder>
   where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder>
   where TBuilder : PathAndValueEntity
{
   private const string PKSIM_R_DLL = "PKSim.R.dll";
   protected readonly IPKSimAssemblyLoader _pkSimLoader;

   protected PKSimPathAndValuesTask(ISerializationTask serializationTask, IPKSimAssemblyLoader pkSimLoader) : base(serializationTask)
   {
      _pkSimLoader = pkSimLoader;
      _pkSimLoader.InitializePath(pkSimAssemblyPath());
   }

   private static string pkSimAssemblyPath() =>
      Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), PKSIM_R_DLL);

   protected static IMoBiCommand UpdateValueCommandFor(TBuildingBlock buildingBlock, string quantityPath, double newBaseValue)
   {
      var builder = buildingBlock[quantityPath.ToObjectPath()];
      return builder == null ? throw new ArgumentException(AppConstants.Exceptions.ParameterNotFoundForPath(quantityPath, buildingBlock.Name)) : new PathAndValueEntityValueOrUnitChangedCommand<TBuilder, TBuildingBlock>(builder, newBaseValue, builder.DisplayUnit, buildingBlock);
   }
}