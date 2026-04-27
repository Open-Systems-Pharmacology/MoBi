using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

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