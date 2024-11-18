using System.Collections.Generic;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper : IMapper<ParameterValuesBuildingBlock, ParameterValuesBuildingBlockDTO>
   {
   }

   public class ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper :
      PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper<ParameterValuesBuildingBlock, ParameterValue, ParameterValuesBuildingBlockDTO, ParameterValueDTO>,
      IParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper
   {
      private readonly IParameterValueToParameterValueDTOMapper _parameterValueMapper;

      public ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper(IParameterValueToParameterValueDTOMapper parameterValueMapper,
         IPathAndValueEntityToDistributedParameterMapper mapper) : base(mapper)
      {
         _parameterValueMapper = parameterValueMapper;
      }

      protected override ParameterValueDTO BuilderDTOFor(ParameterValue pathAndValueEntity, ParameterValuesBuildingBlock buildingBlock)
      {
         return _parameterValueMapper.MapFrom(pathAndValueEntity, buildingBlock);
      }

      protected override ParameterValuesBuildingBlockDTO MapBuildingBlockDTO(ParameterValuesBuildingBlock buildingBlock, List<ParameterValueDTO> parameterDTOs)
      {
         return new ParameterValuesBuildingBlockDTO(buildingBlock) { ParameterDTOs = parameterDTOs };
      }
   }
}