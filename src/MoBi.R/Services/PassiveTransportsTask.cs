using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Presentation.Tasks.ISerializationTask;

namespace MoBi.R.Services;

public interface IPassiveTransportsTask : IBuildingBlockTask<PassiveTransportBuildingBlock>
{
}

public class PassiveTransportsTask : BuildingBlockTask<PassiveTransportBuildingBlock>, IPassiveTransportsTask
{
   public PassiveTransportsTask(ISerializationTask serializationTask) : base(serializationTask)
   {
   }
}