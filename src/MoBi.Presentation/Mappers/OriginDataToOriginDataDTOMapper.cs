using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IOriginDataToOriginDataDTOMapper : IMapper<OriginDataItems, OriginDataDTO>
   {

   }

   public class OriginDataToOriginDataDTOMapper : IOriginDataToOriginDataDTOMapper
   {
      public OriginDataDTO MapFrom(OriginDataItems originDataItems)
      {
         return new OriginDataDTO(originDataItems);
      }
   }
}