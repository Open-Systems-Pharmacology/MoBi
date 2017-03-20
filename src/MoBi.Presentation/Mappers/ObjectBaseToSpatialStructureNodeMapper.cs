using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseToSpatialStructureNodeMapper : IMapper<IObjectBase, ITreeNode>
   {
      void Initialize(Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> getChildren);
   }

   public class ObjectBaseToSpatialStructureNodeMapper : IObjectBaseToSpatialStructureNodeMapper
   {
      private readonly IObjectBaseDTOToSpatialStructureNodeMapper _nodeMapper;
      private readonly IObjectBaseToObjectBaseDTOMapper _objectBaseMapper;

      public ObjectBaseToSpatialStructureNodeMapper(IObjectBaseDTOToSpatialStructureNodeMapper nodeMapper, IObjectBaseToObjectBaseDTOMapper objectBaseMapper)
      {
         _nodeMapper = nodeMapper;
         _objectBaseMapper = objectBaseMapper;
      }

      public ITreeNode MapFrom(IObjectBase objectBase)
      {
         return _nodeMapper.MapFrom(_objectBaseMapper.MapFrom(objectBase));
      }

      public void Initialize(Func<IObjectBaseDTO, IEnumerable<IObjectBaseDTO>> getChildren)
      {
         _nodeMapper.Initialize(getChildren);
      }
   }
}