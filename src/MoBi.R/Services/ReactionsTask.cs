using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using ISerializationTask = MoBi.Core.Serialization.Services.ICoreSerializationTask;

namespace MoBi.R.Services;

public interface IReactionsTask : IBuildingBlockTask<ReactionBuildingBlock>
{
}

public class ReactionsTask : BuildingBlockTask<ReactionBuildingBlock>, IReactionsTask
{
   public ReactionsTask(ISerializationTask serializationTask) : base(serializationTask)
   {
   }

   public override ReactionBuildingBlock LoadFromPKML(string filePath) =>
      LoadSingleFromPKML<MoBiReactionBuildingBlock>(filePath);
}