using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseDTOToSpatialStructureNodeMapper : IMapper<IObjectBaseDTO, HierarchicalStructureNode>
   {
      void Initialize(Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> getChildren);
   }

   public class ObjectBaseDTOToSpatialStructureNodeMapper : IObjectBaseDTOToSpatialStructureNodeMapper
   {
      private Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> _getChildren;

 
      public HierarchicalStructureNode MapFrom(IObjectBaseDTO objectBase)
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

      public void Initialize(Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> getChildren)
      {
         _getChildren = getChildren;
      }

      private ToolTipPart descriptionFor(IObjectBaseDTO objectBase)
      {
         return new ToolTipPart {Title = objectBase.Name, Content = objectBase.Description};
      }
   }
}