using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_SelectSpatialStructureDTO : ContextSpecification<SelectSpatialStructureDTO>
   {
      protected override void Context()
      {
         sut = new SelectSpatialStructureDTO();
      }
   }

   public class When_validating_the_dto_when_no_spatial_structure_is_selected : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
