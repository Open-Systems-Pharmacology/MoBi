using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_SelectSpatialStructureDTOMapper : ContextSpecification<SelectSpatialStructureDTOMapper>
   {
      protected override void Context()
      {
         sut = new SelectSpatialStructureDTOMapper();
      }
   }

   internal class When_mapping_from_spatial_structure_to_a_dto : concern_for_SelectSpatialStructureDTOMapper
   {
      private SelectSpatialStructureDTO _dto;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _spatialStructure = new MoBiSpatialStructure();
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(_spatialStructure);
      }

      [Observation]
      public void should_map_the_spatial_structure()
      {
         _dto.SpatialStructure.ShouldBeEqualTo(_spatialStructure);
      }
   }
}