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
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithMolecule && !addBuildingBlocksToModuleDTO.AlreadyHasMolecule,
               () => CreateDefault<MoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock)),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithReaction && !addBuildingBlocksToModuleDTO.AlreadyHasReaction,
               () => CreateDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create())),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithSpatialStructure && !addBuildingBlocksToModuleDTO.AlreadyHasSpatialStructure,
               () => CreateDefault(AppConstants.DefaultNames.SpatialStructure,
                  () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure))),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithPassiveTransport && !addBuildingBlocksToModuleDTO.AlreadyHasPassiveTransport,
               () => CreateDefault<PassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock)),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithEventGroup && !addBuildingBlocksToModuleDTO.AlreadyHasEventGroup,
               () => CreateDefault<EventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock)),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithObserver && !addBuildingBlocksToModuleDTO.AlreadyHasObserver,
               () => CreateDefault<ObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock)),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithParameterStartValues,
               () => CreateDefault<ParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues)),
            ConditionalCreate(addBuildingBlocksToModuleDTO.WithMoleculeStartValues,
               () => CreateDefault<MoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues))
         };

         _newBuildingBlocks = listOfBuildingBlocks.Where(x => x != null).ToList();


         return _newBuildingBlocks;
      }
   }
}