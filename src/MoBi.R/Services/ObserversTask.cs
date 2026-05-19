using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Core.Serialization.Services.ICoreSerializationTask;

namespace MoBi.R.Services;

public interface IObserversTask : IBuildingBlockTask<ObserverBuildingBlock>
{
}

public class ObserversTask : BuildingBlockTask<ObserverBuildingBlock>, IObserversTask
{
   public ObserversTask(ISerializationTask serializationTask) : base(serializationTask)
   {
   }
}