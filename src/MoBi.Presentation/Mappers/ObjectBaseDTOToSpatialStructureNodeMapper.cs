using System;
using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseDTOToSpatialStructureNodeMapper : IMapper<ObjectBaseDTO, HierarchicalStructureNode>
   {
      void Initialize(Func<ObjectBaseDTO, IEnumerable<ObjectBaseDTO>> getChildren);
   }

   public class ObjectBaseDTOToSpatialStructureNodeMapper : IObjectBaseDTOToSpatialStructureNodeMapper
   {
      private Func<ObjectBaseDTO, IEnumerable<ObjectBaseDTO>> _getChildren;

      public HierarchicalStructureNode MapFrom(ObjectBaseDTO objectBase)
      {
         var node = new HierarchicalStructureNode(objectBase)
         {
            Icon = ApplicationIcons.IconByName(objectBase.Icon),
            Text = objectBase.Name,
            GetChildren = x => _getChildren(x).MapAllUsing(this),
         };

         node.AddToolTipPart(descriptionFor(objectBase));
         return node;
      }

      public void Initialize(Func<ObjectBaseDTO, IEnumerable<ObjectBaseDTO>> getChildren)
      {
         _getChildren = getChildren;
      }

      private ToolTipPart descriptionFor(ObjectBaseDTO objectBase)
      {
         return new ToolTipPart {Title = objectBase.Name, Content = objectBase.Description};
      }
   }
}