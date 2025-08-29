using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

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

         Rules.AddRange(AllRules.All);
      }

      private void addExistingNeighborhoods(IReadOnlyList<NeighborhoodBuilder> existingNeighborhoods)
      {
         _connections = new Cache<string, List<string>>();
         existingNeighborhoods.Each(x => addConnection(_connections, x.FirstNeighborPath, x.SecondNeighborPath));
      }

      private static void addConnection(Cache<string, List<string>> connections, ObjectPath path1, ObjectPath path2)
      {
         connectPaths(connections, path1, path2);
         connectPaths(connections, path2, path1);
      }

      private static void connectPaths(Cache<string, List<string>> connections, ObjectPath path1, ObjectPath path2)
      {
         if (isNullPath(path1) || isNullPath(path2))
            return;

         if (!connections.Contains(path1))
            connections[path1] = new List<string>();

         connections[path1].Add(path2);
      }

      private static bool isNullPath(ObjectPath path1)
      {
         return path1 == null || string.IsNullOrEmpty(path1.PathAsString);
      }

      public bool HasConnectionBetween(string path, string secondNeighborPath)
      {
         if (!_connections.Contains(path))
            return false;

         return _connections[path].Contains(secondNeighborPath);
      }

      private static class AllRules
      {
         private static IBusinessRule noEquivalentForFirstNeighbor { get; } = CreateRule.For<NeighborhoodBuilderDTO>()
            .Property(x => x.FirstNeighborPath)
            .WithRule((dto, path) => !dto.HasConnectionBetween(path, dto.SecondNeighborPath))
            .WithError((dto, path) => AppConstants.Validation.HasEquivalentNeighborhood(path, dto.SecondNeighborPath));

         private static IBusinessRule noEquivalentForSecondNeighbor { get; } = CreateRule.For<NeighborhoodBuilderDTO>()
            .Property(x => x.SecondNeighborPath)
            .WithRule((dto, path) => !dto.HasConnectionBetween(path, dto.FirstNeighborPath))
            .WithError((dto, path) => AppConstants.Validation.HasEquivalentNeighborhood(path, dto.FirstNeighborPath));

         private static IBusinessRule notEqualToSecondNeighbor { get; } = CreateRule.For<NeighborhoodBuilderDTO>()
            .Property(x => x.FirstNeighborPath)
            .WithRule((dto, path) => !Equals(path, dto.SecondNeighborPath))
            .WithError((dto, path) => AppConstants.Validation.CannotCreateANeighborhoodThatConnectsAContainerToItself);

         private static IBusinessRule notEqualToFirstNeighbor { get; } = CreateRule.For<NeighborhoodBuilderDTO>()
            .Property(x => x.SecondNeighborPath)
            .WithRule((dto, path) => !Equals(path, dto.FirstNeighborPath))
            .WithError((dto, path) => AppConstants.Validation.CannotCreateANeighborhoodThatConnectsAContainerToItself);

         public static IReadOnlyList<IBusinessRule> All { get; } = new[]
         {
            noEquivalentForFirstNeighbor,
            noEquivalentForSecondNeighbor,
            notEqualToSecondNeighbor,
            notEqualToFirstNeighbor
         };
      }
   }
}