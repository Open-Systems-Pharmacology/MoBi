using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public class AddBuildingBlocksToModuleDTOToModuleMapper : CreateModuleDTOToModuleMapper
   {
      public AddBuildingBlocksToModuleDTOToModuleMapper(IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory,
         IMoBiSpatialStructureFactory spatialStructureFactory)
         : base(context, reactionBuildingBlockFactory, spatialStructureFactory)
      {
      }

      public Module MapFrom(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO)
      {
         addBuildingBlocksToModuleDTO.WithMolecule = addBuildingBlocksToModuleDTO.WithMolecule && !addBuildingBlocksToModuleDTO.AlreadyHasMolecule;
         addBuildingBlocksToModuleDTO.WithReaction = addBuildingBlocksToModuleDTO.WithReaction && !addBuildingBlocksToModuleDTO.AlreadyHasReaction;
         addBuildingBlocksToModuleDTO.WithSpatialStructure =
            addBuildingBlocksToModuleDTO.WithSpatialStructure && !addBuildingBlocksToModuleDTO.AlreadyHasSpatialStructure;
         addBuildingBlocksToModuleDTO.WithPassiveTransport =
            addBuildingBlocksToModuleDTO.WithPassiveTransport && !addBuildingBlocksToModuleDTO.AlreadyHasPassiveTransport;
         addBuildingBlocksToModuleDTO.WithEventGroup =
            addBuildingBlocksToModuleDTO.WithEventGroup && !addBuildingBlocksToModuleDTO.AlreadyHasEventGroup;
         addBuildingBlocksToModuleDTO.WithObserver = addBuildingBlocksToModuleDTO.WithObserver && !addBuildingBlocksToModuleDTO.AlreadyHasObserver;

         return base.MapFrom(addBuildingBlocksToModuleDTO);
      }
   }
}