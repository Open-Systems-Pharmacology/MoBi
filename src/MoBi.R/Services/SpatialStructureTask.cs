using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Core.Serialization.Services.ICoreSerializationTask;

namespace MoBi.R.Services;

public interface ISpatialStructureTask : IBuildingBlockTask<SpatialStructure>
{
}

public class SpatialStructureTask : BuildingBlockTask<SpatialStructure>, ISpatialStructureTask
{
   public SpatialStructureTask(ISerializationTask serializationTask) : base(serializationTask)
   {
   }

   public override SpatialStructure LoadFromPKML(string filePath) =>
      LoadSingleFromPKML<MoBiSpatialStructure>(filePath);
}