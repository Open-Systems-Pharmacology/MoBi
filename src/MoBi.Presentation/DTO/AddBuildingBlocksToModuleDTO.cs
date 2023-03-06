using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class AddBuildingBlocksToModuleDTO : CreateModuleDTO
   { 
      public AddBuildingBlocksToModuleDTO(Module module)
      {
         if (module.Molecule != null)
         {
            AlreadyHasMolecule = true;
            WithMolecule = false;
         }

         if (module.Reaction != null)
         {
            AlreadyHasReaction = true;
            WithReaction = false;
         }


         if (module.SpatialStructure != null)
         {
            AlreadyHasSpatialStructure = true;
            WithSpatialStructure = false;
         }

         if (module.PassiveTransport != null)
         {
            AlreadyHasPassiveTransport = true;
            WithPassiveTransport = false;
         }

         if (module.EventGroup != null)
         {
            AlreadyHasEventGroup = true;
            WithEventGroup = false;
         }

         if (module.Observer != null)
         {
            AlreadyHasObserver = true;
            WithObserver = false;
         }

         if (module.ParameterStartValuesCollection.Any())
         {
            AlreadyHasParameterStartValues = true;
            WithParameterStartValues = false;
         }

         if (module.MoleculeStartValuesCollection.Any())
         {
            AlreadyHasMoleculeStartValues = true;
            WithMoleculeStartValues = false;
         }
      }

      public bool AlreadyHasReaction { get; set; }
      public bool AlreadyHasEventGroup { get; set; }
      public bool AlreadyHasSpatialStructure { get; set; }
      public bool AlreadyHasPassiveTransport { get; set; }
      public bool AlreadyHasMolecule { get; set; }
      public bool AlreadyHasObserver { get; set; }
      public bool AlreadyHasMoleculeStartValues { get; set; }
      public bool AlreadyHasParameterStartValues { get; set; }
   }
}