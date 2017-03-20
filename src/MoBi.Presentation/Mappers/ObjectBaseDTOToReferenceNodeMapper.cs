using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseDTOToReferenceNodeMapper :IMapper<IObjectBase, ITreeNode>
   {
      ITreeNode MapFrom(IObjectBaseDTO objectBaseDTO);
  
      void Initialize(Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> getChildren);
   }

   public class ObjectBaseDTOToReferenceNodeMapper : IObjectBaseDTOToReferenceNodeMapper
   {
      private readonly IObjectBaseToObjectBaseDTOMapper _mapper;
      private Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> _getChildren;

      public ObjectBaseDTOToReferenceNodeMapper(IObjectBaseToObjectBaseDTOMapper mapper)
      {
         _mapper = mapper;
      }

      public void Initialize(Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> getChildren)
      {
         _getChildren = getChildren;
      }

      public ITreeNode MapFrom(IObjectBaseDTO objectBaseDTO)
      {
         return new ReferenceNode(objectBaseDTO)
         {
            Icon = ApplicationIcons.IconByName(objectBaseDTO.Icon),
            GetChildren = x => _getChildren(x).Select(MapFrom).ToList()
         };
      }

      public ITreeNode MapFrom(IObjectBase objectBase)
      {
         return MapFrom(_mapper.MapFrom(objectBase));
      }
   }
}