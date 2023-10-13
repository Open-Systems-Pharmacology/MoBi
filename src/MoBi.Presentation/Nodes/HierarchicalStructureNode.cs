using System;
using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Utility.Extensions;

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
               //remove all children before adding new ones
               DeleteChildren();
               var children = GetChildren(Tag);
               children.Each(AddChild);
               _childrenLoaded = true;
            }

            return base.Children;
         }
      }
   }
}