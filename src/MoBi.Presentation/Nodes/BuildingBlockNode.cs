using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.Nodes
{
   public class BuildingBlockNode : ObjectWithIdAndNameNode<IBuildingBlock> 
   {
      public BuildingBlockNode(IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         BaseIcon = buildingBlock.Icon;
      }

      public string BaseIcon { get; }
   }
}