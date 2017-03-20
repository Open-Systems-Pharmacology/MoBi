using OSPSuite.Presentation.Nodes;
using MoBi.Presentation.DTO;

namespace MoBi.Presentation.Nodes
{
   public class ReferenceNode : HierarchicalStructureNode
   {
      private string _id;

      public ReferenceNode(IObjectBaseDTO objectBaseDTO) : base(objectBaseDTO)
      {
      }

      public override string Id
      {
         get
         {
            if (string.IsNullOrEmpty(_id))
               updateId();

            return _id;
         }
      }

      public override ITreeNode ParentNode
      {
         set
         {
            base.ParentNode = value;
            updateId();
         }
      }

      private void updateId()
      {
         _id = FullPath();
      }
   }
}