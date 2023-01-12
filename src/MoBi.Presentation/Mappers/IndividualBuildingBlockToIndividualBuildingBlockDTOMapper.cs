using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper : IMapper<IndividualBuildingBlock, IndividualBuildingBlockDTO>
   {
   }

   public class IndividualBuildingBlockToIndividualBuildingBlockDTOMapper : IIndividualBuildingBlockToIndividualBuildingBlockDTOMapper
   {
      private readonly IIndividualParameterToIndividualParameterDTOMapper _individualParameterToDTOMapper;
      private readonly IOriginDataToOriginDataDTOMapper _originDataToOriginDataDTOMapper;

      public IndividualBuildingBlockToIndividualBuildingBlockDTOMapper(IIndividualParameterToIndividualParameterDTOMapper individualParameterToDTOMapper, IOriginDataToOriginDataDTOMapper originDataToOriginDataDTOMapper)
      {
         _individualParameterToDTOMapper = individualParameterToDTOMapper;
         _originDataToOriginDataDTOMapper = originDataToOriginDataDTOMapper;
      }

      public IndividualBuildingBlockDTO MapFrom(IndividualBuildingBlock individualBuildingBlock)
      {
         return new IndividualBuildingBlockDTO(individualBuildingBlock)
         {
            OriginDataDTO = _originDataToOriginDataDTOMapper.MapFrom(individualBuildingBlock.OriginData),
            ParameterDTOs = individualBuildingBlock.MapAllUsing(_individualParameterToDTOMapper)
         };
      }
   }
}