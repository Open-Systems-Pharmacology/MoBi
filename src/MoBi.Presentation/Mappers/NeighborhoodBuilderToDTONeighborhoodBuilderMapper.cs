using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface INeighborhoodBuilderToDTONeighborhoodBuilderMapper : IMapper<INeighborhoodBuilder, NeighborhoodBuilderDTO>
   {
   }

   public class NeighborhoodBuilderToDTONeighborhoodBuilderMappern : ObjectBaseToObjectBaseDTOMapperBase, INeighborhoodBuilderToDTONeighborhoodBuilderMapper
   {
      public NeighborhoodBuilderDTO MapFrom(INeighborhoodBuilder neighborhoodBuilder)
      {
         return Map<NeighborhoodBuilderDTO>(neighborhoodBuilder);
      }
   }
}