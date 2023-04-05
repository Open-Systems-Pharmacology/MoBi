using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class AddBuildingBlocksToModuleDTO : CreateModuleDTO
   {
      public AddBuildingBlocksToModuleDTO(Module module)
      {
         if (module.Molecules != null)
         {
            AlreadyHasMolecule = true;
            WithMolecule = false;
         }

         if (module.Reactions != null)
         {
            AlreadyHasReaction = true;
            WithReaction = false;
         }


         if (module.SpatialStructure != null)
         {
            AlreadyHasSpatialStructure = true;
            WithSpatialStructure = false;
         }

         if (module.PassiveTransports != null)
         {
            AlreadyHasPassiveTransport = true;
            WithPassiveTransport = false;
         }

         if (module.EventGroups != null)
         {
            AlreadyHasEventGroup = true;
            WithEventGroup = false;
         }

         if (module.Observers != null)
         {
            AlreadyHasObserver = true;
            WithObserver = false;
         }
      }

      public bool AlreadyHasReaction { get; set; }
      public bool AlreadyHasEventGroup { get; set; }
      public bool AlreadyHasSpatialStructure { get; set; }
      public bool AlreadyHasPassiveTransport { get; set; }
      public bool AlreadyHasMolecule { get; set; }
      public bool AlreadyHasObserver { get; set; }
   }
}