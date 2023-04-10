using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public enum BuildingBlockType
   {
      Reaction,
      EventGroup,
      SpatialStructure,
      PassiveTransport,
      Molecule,
      Observer,
      MoleculeStartValues,
      ParameterStartValues,
      None
   }

   public class SelectBuildingBlockTypeDTO : ValidatableDTO
   {
      //probably a list internally, readonlyList to the public
      public List<BuildingBlockType> AllowedBuildingBlockTypes { get; set; }

      public BuildingBlockType SelectedBuildingBlockType { get; set; }
      public SelectBuildingBlockTypeDTO(Module module)
      {
         SelectedBuildingBlockType = BuildingBlockType.None;
         AllowedBuildingBlockTypes = new List<BuildingBlockType>();

         if (module.Molecules == null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.Molecule);
         
         if (module.Reactions == null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.Reaction);

         if (module.SpatialStructure == null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.SpatialStructure);

         if (module.PassiveTransports == null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.PassiveTransport);

         if (module.EventGroups == null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.EventGroup);

         if (module.Observers == null)
            AllowedBuildingBlockTypes.Add(BuildingBlockType.Observer);

         AllowedBuildingBlockTypes.Add(BuildingBlockType.MoleculeStartValues);
         AllowedBuildingBlockTypes.Add(BuildingBlockType.ParameterStartValues);
      }
   }
}