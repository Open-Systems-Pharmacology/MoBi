using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.Nodes
{
   public class HierarchicalStructureNode : ObjectWithIdAndNameNode<ObjectBaseDTO>
   {
      private bool _childrenLoaded;
      public Func<ObjectBaseDTO, IEnumerable<ITreeNode>> GetChildren { get; set; }

      public HierarchicalStructureNode(ObjectBaseDTO objectBaseDTO) : base(objectBaseDTO)
      {
         _childrenLoaded = false;
      }

      public override IEnumerable<ITreeNode> Children
      {
         get
         {
            if (!_childrenLoaded)
            {
               var children = GetChildren(Tag);
               children.Each(AddChild);
               _childrenLoaded = true;
            }
            return base.Children;
         }
      }
   }
}