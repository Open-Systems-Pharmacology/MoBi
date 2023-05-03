using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.DTO
{
   public class AddBuildingBlocksToModuleDTO : ModuleContentDTO
   {
      public AddBuildingBlocksToModuleDTO(Module module)
      {
         Name = module.Name;

         CanSelectMolecule = module.Molecules == null;
         WithMolecule = !CanSelectMolecule;

         CanSelectReaction = module.Reactions == null;
         WithReaction = !CanSelectReaction;

         CanSelectSpatialStructure = module.SpatialStructure == null;
         WithSpatialStructure = !CanSelectSpatialStructure;

         CanSelectPassiveTransport = module.PassiveTransports == null;
         WithPassiveTransport = !CanSelectPassiveTransport;

         CanSelectEventGroup = module.EventGroups == null;
         WithEventGroup = !CanSelectEventGroup;

         CanSelectObserver = module.Observers == null;
         WithObserver = !CanSelectObserver;
      }

      public bool CreateMolecule => WithMolecule && CanSelectMolecule;
      public bool CreateReaction => WithReaction && CanSelectReaction;
      public bool CreateSpatialStructure => WithSpatialStructure && CanSelectSpatialStructure;
      public bool CreatePassiveTransport => WithPassiveTransport && CanSelectPassiveTransport;
      public bool CreateEventGroup => WithEventGroup && CanSelectEventGroup;
      public bool CreateObserver => WithObserver && CanSelectObserver;
      
   }
}