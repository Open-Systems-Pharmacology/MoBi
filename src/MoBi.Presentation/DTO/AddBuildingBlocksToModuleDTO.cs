using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.DTO
{
   public class CloneBuildingBlocksToModuleDTO : ModuleContentDTO
   {
      private readonly Module _module;

      public CloneBuildingBlocksToModuleDTO(Module module)
      {
         Name = AppConstants.CloneName(module);
         _module = module;

         CanSelectMolecule = module.Molecules != null;
         WithMolecule = CanSelectMolecule;

         CanSelectReaction = module.Reactions != null;
         WithReaction = CanSelectReaction;

         CanSelectSpatialStructure = module.SpatialStructure != null;
         WithSpatialStructure = CanSelectSpatialStructure;

         CanSelectPassiveTransport = module.PassiveTransports != null;
         WithPassiveTransport = CanSelectPassiveTransport;

         CanSelectEventGroup = module.EventGroups != null;
         WithEventGroup = CanSelectEventGroup;

         CanSelectObserver = module.Observers != null;
         WithObserver = CanSelectObserver;

         CanSelectMoleculeStartValues = module.MoleculeStartValuesCollection.Any();
         WithMoleculeStartValues = CanSelectMoleculeStartValues;
         
         CanSelectParameterStartValues = module.ParameterStartValuesCollection.Any();
         WithParameterStartValues = CanSelectParameterStartValues;
      }

      public bool RemoveMolecule => _module.Molecules != null && !WithMolecule;
      public bool RemoveReaction => _module.Reactions != null && !WithReaction;
      public bool RemoveSpatialStructure => _module.SpatialStructure != null && !WithSpatialStructure;
      public bool RemovePassiveTransport => _module.PassiveTransports != null && !WithPassiveTransport;
      public bool RemoveEventGroup => _module.EventGroups != null && !WithEventGroup;
      public bool RemoveObserver => _module.Observers != null && !WithObserver;
      public bool RemoveMoleculeStartValues => _module.MoleculeStartValuesCollection.Any() && !WithMoleculeStartValues;
      public bool RemoveParameterStartValues => _module.ParameterStartValuesCollection.Any() && !WithParameterStartValues;
   }

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