using System.Collections.Generic;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper : IMapper<IndividualBuildingBlock, IndividualBuildingBlockDTO>
   {
   }

   public class IndividualBuildingBlockToIndividualBuildingBlockDTOMapper : 
      PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper<IndividualBuildingBlock, IndividualParameter, IndividualBuildingBlockDTO, IndividualParameterDTO>, 
      IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper
   {
      private readonly IIndividualParameterToIndividualParameterDTOMapper _individualParameterToDTOMapper;

      public IndividualBuildingBlockToIndividualBuildingBlockDTOMapper(IIndividualParameterToIndividualParameterDTOMapper individualParameterToDTOMapper,
         IPathAndValueEntityToDistributedParameterMapper mapper) : base(mapper)
      {
         _individualParameterToDTOMapper = individualParameterToDTOMapper;
      }

      protected override IndividualParameterDTO BuilderDTOFor(IndividualParameter pathAndValueEntity, IndividualBuildingBlock buildingBlock)
      {
         return _individualParameterToDTOMapper.MapFrom(pathAndValueEntity);
      }

      protected override IndividualBuildingBlockDTO MapBuildingBlockDTO(IndividualBuildingBlock buildingBlock, List<IndividualParameterDTO> parameterDTOs)
      {
         return new IndividualBuildingBlockDTO(buildingBlock) {  Parameters = parameterDTOs };
      }
   }
}