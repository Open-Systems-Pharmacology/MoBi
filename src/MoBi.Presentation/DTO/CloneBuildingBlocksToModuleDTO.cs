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

         CanSelectInitialConditions = module.InitialConditionsCollection.Any();
         WithInitialConditions = CanSelectInitialConditions;
         
         CanSelectParameterValues = module.ParameterValuesCollection.Any();
         WithParameterValues = CanSelectParameterValues;
      }

      public IReadOnlyCollection<IBuildingBlock> BuildingBlocksToRemove {
         get
         {
            var buildingBlocksToRemove = new List<IBuildingBlock>();
            if (removeMolecule)
               buildingBlocksToRemove.Add(_module.Molecules);

            if (removeEventGroup)
               buildingBlocksToRemove.Add(_module.EventGroups);

            if (removeObserver)
               buildingBlocksToRemove.Add(_module.Observers);

            if (removePassiveTransport)
               buildingBlocksToRemove.Add(_module.PassiveTransports);

            if (removeReaction)
               buildingBlocksToRemove.Add(_module.Reactions);

            if (removeSpatialStructure)
               buildingBlocksToRemove.Add(_module.SpatialStructure);

            if (removeInitialConditions)
               _module.InitialConditionsCollection.Each(buildingBlocksToRemove.Add);

            if (removeParameterValues)
               _module.ParameterValuesCollection.Each(buildingBlocksToRemove.Add);

            return buildingBlocksToRemove;
         }
      }

      private bool removeMolecule => _module.Molecules != null && !WithMolecule;
      private bool removeReaction => _module.Reactions != null && !WithReaction;
      private bool removeSpatialStructure => _module.SpatialStructure != null && !WithSpatialStructure;
      private bool removePassiveTransport => _module.PassiveTransports != null && !WithPassiveTransport;
      private bool removeEventGroup => _module.EventGroups != null && !WithEventGroup;
      private bool removeObserver => _module.Observers != null && !WithObserver;
      private bool removeInitialConditions => _module.InitialConditionsCollection.Any() && !WithInitialConditions;
      private bool removeParameterValues => _module.ParameterValuesCollection.Any() && !WithParameterValues;
   }
}