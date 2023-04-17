using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper : IMapper<AddBuildingBlocksToModuleDTO, IReadOnlyList<IBuildingBlock>>
   {
   }

   public class AddBuildingBlocksToModuleDTOToBuildingBlocksListMapper : ModuleDTOMapper, IAddBuildingBlocksToModuleDTOToBuildingBlocksListMapper
   {
      private List<IBuildingBlock> _newBuildingBlocks;
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public AddBuildingBlocksToModuleDTOToBuildingBlocksListMapper(IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory,
         IMoBiSpatialStructureFactory spatialStructureFactory)
         : base(context)
      {
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
         _spatialStructureFactory = spatialStructureFactory;
      }

      public IReadOnlyList<IBuildingBlock> MapFrom(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO)
      {
         var listOfBuildingBlocks = new List<IBuildingBlock>
         {
            conditionalCreate(addBuildingBlocksToModuleDTO.WithMolecule && !addBuildingBlocksToModuleDTO.AlreadyHasMolecule,
               () => addDefault<MoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithReaction && !addBuildingBlocksToModuleDTO.AlreadyHasReaction,
               () => addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create())),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithSpatialStructure && !addBuildingBlocksToModuleDTO.AlreadyHasSpatialStructure,
               () => addDefault(AppConstants.DefaultNames.SpatialStructure,
                  () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure))),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithPassiveTransport && !addBuildingBlocksToModuleDTO.AlreadyHasPassiveTransport,
               () => addDefault<PassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithEventGroup && !addBuildingBlocksToModuleDTO.AlreadyHasEventGroup,
               () => addDefault<EventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithObserver && !addBuildingBlocksToModuleDTO.AlreadyHasObserver,
               () => addDefault<ObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithParameterStartValues,
               () => addDefault<ParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithMoleculeStartValues,
               () => addDefault<MoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues))
         };

         _newBuildingBlocks = listOfBuildingBlocks.Where(x => x != null).ToList();


         return _newBuildingBlocks;
      }
   }
}