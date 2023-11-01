using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class SelectSpatialStructureAndMoleculesDTO : DxValidatableDTO
   {
      private readonly List<MoleculeSelectionDTO> _molecules;
      public MoBiSpatialStructure SpatialStructure { get; set; }
      public IReadOnlyList<MoleculeSelectionDTO> Molecules => _molecules;

      public SelectSpatialStructureAndMoleculesDTO()
      {
         _molecules = new List<MoleculeSelectionDTO>();

         Rules.Add(AllRules.MoleculeSelected);
         Rules.Add(AllRules.SpatialStructureSelected);
      }

      private static class AllRules
      {
         public static IBusinessRule MoleculeSelected { get; } =
            CreateRule.For<SelectSpatialStructureAndMoleculesDTO>()
               .Property(x => x.Molecules)
               .WithRule((dto, moleculeBuildingBlock) => dto.Molecules != null && dto.Molecules.Any())
               .WithError(AppConstants.Validation.ExtendingRequiresMoleculeBuildingBlock);

         public static IBusinessRule SpatialStructureSelected { get; } =
            CreateRule.For<SelectSpatialStructureAndMoleculesDTO>()
               .Property(x => x.SpatialStructure)
               .WithRule((dto, spatialStructure) => dto.SpatialStructure != null)
               .WithError(AppConstants.Validation.ExtendingRequiresSpatialStructure);
      }

      public void AddMolecules(List<MoleculeSelectionDTO> molecules)
      {
         molecules.Each(x => x.AddSelectedMoleculeRetriever(() => Molecules.Where(m => m.Selected).ToList()));
         _molecules.AddRange(molecules);
      }
   }
}