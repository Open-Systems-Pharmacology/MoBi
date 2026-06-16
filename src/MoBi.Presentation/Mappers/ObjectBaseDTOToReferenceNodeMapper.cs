using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseDTOToReferenceNodeMapper : IMapper<IObjectBase, ITreeNode>, IMapper<ObjectBaseDTO, ITreeNode>
   {
      void Initialize(Func<ObjectBaseDTO, IEnumerable<ObjectBaseDTO>> getChildren);
   }

   public class ObjectBaseDTOToReferenceNodeMapper : IObjectBaseDTOToReferenceNodeMapper
   {
      private readonly IObjectBaseToObjectBaseDTOMapper _mapper;
      private Func<ObjectBaseDTO, IEnumerable<ObjectBaseDTO>> _getChildren;

      public ObjectBaseDTOToReferenceNodeMapper(IObjectBaseToObjectBaseDTOMapper mapper)
      {
         _mapper = mapper;
      }

      public void Initialize(Func<ObjectBaseDTO, IEnumerable<ObjectBaseDTO>> getChildren)
      {
         _getChildren = getChildren;
      }

      public ITreeNode MapFrom(ObjectBaseDTO objectBaseDTO)
      {
         var nodeText = objectBaseDTO.ObjectBase is BuildingBlock buildingBlock
            ? buildingBlock.DisplayName
            : objectBaseDTO.Name;

         return new ReferenceNode(objectBaseDTO)
         {
            Icon = objectBaseDTO.Icon,
            GetChildren = x => _getChildren(x).Select(MapFrom).ToList(),
            Text = nodeText
         };
      }

      public ITreeNode MapFrom(IObjectBase objectBase)
      {
         return MapFrom(_mapper.MapFrom(objectBase));
      }
   }
}