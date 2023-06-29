using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class SelectSpatialStructureAndMoleculesDTO : DxValidatableDTO
   {
      public MoleculeBuildingBlock Molecules { get; set; }
      public MoBiSpatialStructure SpatialStructure { get; set; }

      public SelectSpatialStructureAndMoleculesDTO(bool moleculeRequired = true)
      {
         if(moleculeRequired)
            Rules.Add(AllRules.MoleculeSelected);
         Rules.Add(AllRules.SpatialStructureSelected);
      }
      
      private static class AllRules
      {
         public static IBusinessRule MoleculeSelected { get; } =
            CreateRule.For<SelectSpatialStructureAndMoleculesDTO>()
               .Property(x => x.Molecules)
               .WithRule((dto, moleculeBuildingBlock) => dto.Molecules != null)
               .WithError(AppConstants.Validation.ExtendingRequiresMoleculeBuildingBlock);

         public static IBusinessRule SpatialStructureSelected { get; } =
            CreateRule.For<SelectSpatialStructureAndMoleculesDTO>()
               .Property(x => x.SpatialStructure)
               .WithRule((dto, spatialStructure) => dto.SpatialStructure != null)
               .WithError(AppConstants.Validation.ExtendingRequiresSpatialStructure);
      }
   }
}