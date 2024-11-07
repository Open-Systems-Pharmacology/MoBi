using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodBuilderDTO : ContainerDTO
   {
      private Cache<string, List<string>> _connections;
      public NeighborhoodObjectPathDTO FirstNeighborDTO { get; }
      public NeighborhoodObjectPathDTO SecondNeighborDTO { get; }
      public string FirstNeighborPath => FirstNeighborDTO.Path;
      public string SecondNeighborPath => SecondNeighborDTO.Path;

      public NeighborhoodBuilderDTO(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<NeighborhoodBuilder> existingNeighborhoods) : base(neighborhoodBuilder)
      {
         Icon = ApplicationIcons.Neighborhood;
         FirstNeighborDTO = new NeighborhoodObjectPathDTO(this, () => SecondNeighborDTO) { Path = neighborhoodBuilder.FirstNeighborPath?.ToPathString() };
         SecondNeighborDTO = new NeighborhoodObjectPathDTO(this, () => FirstNeighborDTO) { Path = neighborhoodBuilder.SecondNeighborPath?.ToPathString() };
         addExistingNeighborhoods(existingNeighborhoods);
      }

      private void addExistingNeighborhoods(IReadOnlyList<NeighborhoodBuilder> existingNeighborhoods)
      {
         var uniqueNeighbors = existingNeighborhoods.SelectMany(x => new[] { x.FirstNeighborPath, x.SecondNeighborPath }).Distinct().Where(x => x != null);
         _connections = new Cache<string, List<string>>();

         uniqueNeighbors.Each(x =>
         {
            existingNeighborhoods.Where(y => Equals(y.FirstNeighborPath, x)).Each(y => addConnection(_connections, x, y.SecondNeighborPath));
            existingNeighborhoods.Where(y => Equals(y.SecondNeighborPath, x)).Each(y => addConnection(_connections, x, y.FirstNeighborPath));
         });
      }

      private static void addConnection(Cache<string, List<string>> connections, ObjectPath x, ObjectPath connectedObjectPath)
      {
         if (!connections.Contains(x))
            connections[x] = new List<string>();

         connections[x].Add(connectedObjectPath);
      }

      public bool HasConnectionBetween(string path, string secondNeighborPath)
      {
         if (!_connections.Contains(path))
            return false;

         return _connections[path].Contains(secondNeighborPath);
      }
   }
}