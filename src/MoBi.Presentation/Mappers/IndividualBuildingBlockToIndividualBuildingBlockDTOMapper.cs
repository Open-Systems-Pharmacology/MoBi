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

      public IndividualBuildingBlockToIndividualBuildingBlockDTOMapper(IIndividualParameterToIndividualParameterDTOMapper individualParameterToDTOMapper)
      {
         _individualParameterToDTOMapper = individualParameterToDTOMapper;
      }

      public IndividualBuildingBlockDTO MapFrom(IndividualBuildingBlock individualBuildingBlock)
      {
         return new IndividualBuildingBlockDTO(individualBuildingBlock)
         {
            Parameters = individualBuildingBlock.MapAllUsing(_individualParameterToDTOMapper)
         };
      }
   }
}