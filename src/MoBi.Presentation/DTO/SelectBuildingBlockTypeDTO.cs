using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public enum BuildingBlockType
   {
      Reaction,
      Events,
      SpatialStructure,
      PassiveTransport,
      Molecule,
      Observer,
      InitialConditions,
      ParameterValues,
      None
   }

   public class SelectBuildingBlockTypeDTO : ValidatableDTO
   {
      public IReadOnlyList<BuildingBlockType> AllowedBuildingBlockTypes => _allowedBuildingBlocks;
      public BuildingBlockType SelectedBuildingBlockType { get; set; }

      private readonly List<BuildingBlockType> _allowedBuildingBlocks;

      public SelectBuildingBlockTypeDTO(Module module)
      {
         SelectedBuildingBlockType = BuildingBlockType.None;
         _allowedBuildingBlocks = new List<BuildingBlockType>();

         if (module.Molecules == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Molecule);

         if (module.Reactions == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Reaction);

         if (module.SpatialStructure == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.SpatialStructure);

         if (module.PassiveTransports == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.PassiveTransport);

         if (module.EventGroups == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Events);

         if (module.Observers == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Observer);

         _allowedBuildingBlocks.Add(BuildingBlockType.InitialConditions);
         _allowedBuildingBlocks.Add(BuildingBlockType.ParameterValues);
      }
   }
}