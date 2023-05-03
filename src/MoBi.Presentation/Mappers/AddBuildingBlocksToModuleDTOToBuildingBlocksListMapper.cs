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

      public IReadOnlyList<IBuildingBlock> MapFrom(AddBuildingBlocksToModuleDTO dto)
      {
         var listOfBuildingBlocks = new List<IBuildingBlock>
         {
            ConditionalCreate(dto.CreateReaction, () => CreateDefault(AppConstants.DefaultNames.ReactionBuildingBlock,
               () => _reactionBuildingBlockFactory.Create())),

            ConditionalCreate(dto.CreateSpatialStructure, () => CreateDefault(AppConstants.DefaultNames.SpatialStructure,
               () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure))),
            
            ConditionalCreate(dto.CreateMolecule, () => CreateDefault<MoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock)),
            ConditionalCreate(dto.CreatePassiveTransport, () => CreateDefault<PassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock)),
            ConditionalCreate(dto.CreateEventGroup, () => CreateDefault<EventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock)),
            ConditionalCreate(dto.CreateObserver, () => CreateDefault<ObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock)),
            ConditionalCreate(dto.WithParameterStartValues, () => CreateDefault<ParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues)),
            ConditionalCreate(dto.WithMoleculeStartValues, () => CreateDefault<MoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues))
         };

         _newBuildingBlocks = listOfBuildingBlocks.Where(x => x != null).ToList();


         return _newBuildingBlocks;
      }
   }
}