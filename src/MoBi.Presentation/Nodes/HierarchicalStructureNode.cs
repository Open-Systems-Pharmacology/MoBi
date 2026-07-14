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

      /// <summary>
      ///    Returns <c>true</c> once the children have been enumerated (lazily loaded) into this node, even if that
      ///    enumeration produced no children. Unlike <see cref="AbstractNode.HasChildren" /> this distinguishes an
      ///    enumerated-but-empty node from one whose children were never loaded, and it does not trigger the loading.
      /// </summary>
      public bool ChildrenLoaded => _childrenLoaded;

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