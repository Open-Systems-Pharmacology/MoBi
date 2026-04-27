using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public interface IEventGroupsTask : IBuildingBlockTask<EventGroupBuildingBlock>
{
}

public class EventGroupsTask : BuildingBlockTask<EventGroupBuildingBlock>, IEventGroupsTask
{
   public EventGroupsTask(ISerializationTask serializationTask) : base(serializationTask)
   {
   }
}