using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;

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
}