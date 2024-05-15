using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
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
            ConditionalCreate(dto.CreateReaction, () => CreateDefault(DefaultNames.ReactionBuildingBlock,
               () => _reactionBuildingBlockFactory.Create())),

            ConditionalCreate(dto.CreateSpatialStructure, () => CreateDefault(DefaultNames.SpatialStructure,
               () => _spatialStructureFactory.CreateDefault())),
            
            ConditionalCreate(dto.CreateMolecule, () => CreateDefault<MoleculeBuildingBlock>(DefaultNames.MoleculeBuildingBlock)),
            ConditionalCreate(dto.CreatePassiveTransport, () => CreateDefault<PassiveTransportBuildingBlock>(DefaultNames.PassiveTransportBuildingBlock)),
            ConditionalCreate(dto.CreateEventGroup, () => CreateDefault<EventGroupBuildingBlock>(DefaultNames.EventBuildingBlock)),
            ConditionalCreate(dto.CreateObserver, () => CreateDefault<ObserverBuildingBlock>(DefaultNames.ObserverBuildingBlock)),
            ConditionalCreate(dto.WithParameterValues, () => CreateDefault<ParameterValuesBuildingBlock>(DefaultNames.ParameterValues).WithName(dto.ParameterValuesName)),
            ConditionalCreate(dto.WithInitialConditions, () => CreateDefault<InitialConditionsBuildingBlock>(DefaultNames.InitialConditions).WithName(dto.InitialConditionsName))
         };

         _newBuildingBlocks = listOfBuildingBlocks.Where(x => x != null).ToList();


         return _newBuildingBlocks;
      }
   }
}