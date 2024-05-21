using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface INeighborhoodToNeighborDTOMapper : IMapper<Neighborhood, IEnumerable<NeighborDTO>>, IMapper<NeighborhoodBuilder, IEnumerable<NeighborDTO>>
   {
   }

   public class NeighborhoodToNeighborDTOMapper : INeighborhoodToNeighborDTOMapper
   {
      private readonly IObjectPathFactory _objectPathFactory;

      public NeighborhoodToNeighborDTOMapper(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public IEnumerable<NeighborDTO> MapFrom(Neighborhood neighborhood)
      {
         return createDTOFromNeighborhoodPaths(neighborhood.Id, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.FirstNeighbor), _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.SecondNeighbor));
      }

      public IEnumerable<NeighborDTO> MapFrom(NeighborhoodBuilder neighborhoodBuilder)
      {
         return createDTOFromNeighborhoodPaths(neighborhoodBuilder.Id, neighborhoodBuilder.FirstNeighborPath, neighborhoodBuilder.SecondNeighborPath);
      }

      private IEnumerable<NeighborDTO> createDTOFromNeighborhoodPaths(string parentId, ObjectPath firstNeighborPath, ObjectPath secondNeighborPath)
      {
         if(firstNeighborPath != null)
            yield return new NeighborDTO(firstNeighborPath) { Icon = ApplicationIcons.Neighbor, Id = createNeighborhoodId(parentId, firstNeighborPath) };
         if(secondNeighborPath != null)
            yield return new NeighborDTO(secondNeighborPath) { Icon = ApplicationIcons.Neighbor, Id = createNeighborhoodId(parentId, secondNeighborPath) };
      }

      /// <summary>
      ///    Creates an Id for neighbors in a neighborhood.
      ///    The Id must be distinct for each neighborhood and neighbor, so it cannot be just the
      ///    <paramref name="neighborPath" />, but must include the <paramref name="parentId"/>
      /// </summary>
      /// <returns>An Id that combines the two Ids of the neighborhood and neighbor</returns>
      private string createNeighborhoodId(string parentId, ObjectPath neighborPath)
      {
         return $"{parentId}-{neighborPath}";
      }
   }
}
