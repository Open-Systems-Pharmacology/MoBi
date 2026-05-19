using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Core.Serialization.Services.ICoreSerializationTask;

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