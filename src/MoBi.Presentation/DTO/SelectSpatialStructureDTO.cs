using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class SelectSpatialStructureDTO : DxValidatableDTO
   {
      public SpatialStructure SpatialStructure { get; set; }

      public SelectSpatialStructureDTO()
      {
         Rules.Add(AllRules.SpatialStructureSelected);
      }

      private static class AllRules
      {
         public static IBusinessRule SpatialStructureSelected { get; } =
            CreateRule.For<SelectSpatialStructureDTO>()
               .Property(x => x.SpatialStructure)
               .WithRule((dto, spatialStructure) => dto.SpatialStructure != null)
               .WithError(AppConstants.Validation.ExtendingRequiresSpatialStructure);
      }
   }
}