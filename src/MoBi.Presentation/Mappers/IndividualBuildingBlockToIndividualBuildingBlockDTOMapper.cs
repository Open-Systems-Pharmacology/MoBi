using System.Collections.Generic;
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
      public IndividualBuildingBlockToIndividualBuildingBlockDTOMapper(IIndividualParameterToIndividualParameterDTOMapper individualParameterToDTOMapper) : base(individualParameterToDTOMapper)
      {
      }

      protected override IndividualBuildingBlockDTO MapBuildingBlockDTO(IndividualBuildingBlock buildingBlock, List<IndividualParameterDTO> parameterDTOs)
      {
         return new IndividualBuildingBlockDTO(buildingBlock) {  Parameters = parameterDTOs };
      }
   }
}