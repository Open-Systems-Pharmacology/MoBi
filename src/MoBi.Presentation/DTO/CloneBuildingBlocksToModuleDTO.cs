using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

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

      public IReadOnlyCollection<IBuildingBlock> BuildingBlocksToRemove {
         get
         {
            var buildingBlocks = new List<IBuildingBlock>();
            if (removeMolecule)
               buildingBlocks.Add(_module.Molecules);

            if (removeEventGroup)
               buildingBlocks.Add(_module.EventGroups);

            if (removeObserver)
               buildingBlocks.Add(_module.Observers);

            if (removePassiveTransport)
               buildingBlocks.Add(_module.PassiveTransports);

            if (removeReaction)
               buildingBlocks.Add(_module.Reactions);

            if (removeSpatialStructure)
               buildingBlocks.Add(_module.SpatialStructure);

            if (removeMoleculeStartValues)
               _module.MoleculeStartValuesCollection.Each(buildingBlocks.Add);

            if (removeParameterStartValues)
               _module.ParameterStartValuesCollection.Each(buildingBlocks.Add);

            return buildingBlocks;
         }
      }

      private bool removeMolecule => _module.Molecules != null && !WithMolecule;
      private bool removeReaction => _module.Reactions != null && !WithReaction;
      private bool removeSpatialStructure => _module.SpatialStructure != null && !WithSpatialStructure;
      private bool removePassiveTransport => _module.PassiveTransports != null && !WithPassiveTransport;
      private bool removeEventGroup => _module.EventGroups != null && !WithEventGroup;
      private bool removeObserver => _module.Observers != null && !WithObserver;
      private bool removeMoleculeStartValues => _module.MoleculeStartValuesCollection.Any() && !WithMoleculeStartValues;
      private bool removeParameterStartValues => _module.ParameterStartValuesCollection.Any() && !WithParameterStartValues;
   }
}