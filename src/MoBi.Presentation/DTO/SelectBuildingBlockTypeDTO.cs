using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public enum BuildingBlockType
   {
      Reactions,
      Events,
      SpatialStructure,
      PassiveTransport,
      Molecules,
      Observers,
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

         if (module.SpatialStructure == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.SpatialStructure);

         if (module.Molecules == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Molecules);

         if (module.Reactions == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Reactions);

         if (module.PassiveTransports == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.PassiveTransport);

         if (module.Observers == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Observers);

         if (module.EventGroups == null)
            _allowedBuildingBlocks.Add(BuildingBlockType.Events);

         _allowedBuildingBlocks.Add(BuildingBlockType.InitialConditions);
         _allowedBuildingBlocks.Add(BuildingBlockType.ParameterValues);
      }
   }
}