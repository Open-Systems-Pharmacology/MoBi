using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Nodes
{
   public class BuildingBlockInfoNode : AbstractNode<IBuildingBlockInfo>
   {
      public BuildingBlockInfoNode(IBuildingBlockInfo buildingBlockInfo)
         : base(buildingBlockInfo)
      {
         UpdateText();
      }

      public override string Id
      {
         get { return buildingBlock.Id; }
      }

      private IBuildingBlock buildingBlock
      {
         get { return Tag.UntypedBuildingBlock; }
      }

      protected override void UpdateText()
      {
         Text = Tag.UntypedBuildingBlock.Name;
      }
   }
}