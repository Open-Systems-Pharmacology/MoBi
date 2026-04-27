using System.Linq;
using MoBi.Assets;
using MoBi.Core.Exceptions;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public interface IBuildingBlockTask<TBuildingBlock>
{
   TBuildingBlock LoadFromPKML(string filePath);
}

public abstract class BuildingBlockTask<TBuildingBlock> : IBuildingBlockTask<TBuildingBlock>
{
   private readonly ISerializationTask _serializationTask;

   protected BuildingBlockTask(ISerializationTask serializationTask)
   {
      _serializationTask = serializationTask;
   }

   public virtual TBuildingBlock LoadFromPKML(string filePath) => LoadSingleFromPKML<TBuildingBlock>(filePath);

   protected T LoadSingleFromPKML<T>(string filePath)
   {
      var buildingBlocks = _serializationTask.LoadAll<T>(filePath);
      if (buildingBlocks.Count == 0)
         throw new MoBiException(AppConstants.Exceptions.NoBuildingBlocksOfTypeFound(typeof(T).Name));
      if (buildingBlocks.Count > 1)
         throw new MoBiException(AppConstants.Exceptions.MoreThanOneBuildingBlocks(typeof(T).Name));
      return buildingBlocks.First();
   }
}