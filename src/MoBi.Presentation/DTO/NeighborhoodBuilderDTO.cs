using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class NeighborhoodBuilderDTO : ContainerDTO
   {
      public string FirstNeighborPath { get; set; }
      public string SecondNeighborPath { get; set; }
      public NeighborhoodBuilderDTO(NeighborhoodBuilder neighborhoodBuilder) : base(neighborhoodBuilder)
      {
         Icon = ApplicationIcons.Neighborhood;
      }
   }
}