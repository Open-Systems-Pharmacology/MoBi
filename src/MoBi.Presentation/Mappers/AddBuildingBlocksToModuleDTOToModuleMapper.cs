using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   //TO DELETE I WOULD SAY
   public interface IAddBuildingBlocksToModuleDTOToModuleMapper : IMapper<AddBuildingBlocksToModuleDTO, IReadOnlyList<IBuildingBlock>>
   {
   }

   public class AddBuildingBlocksToModuleDTOToModuleMapper :  IAddBuildingBlocksToModuleDTOToModuleMapper
   {
      private List<IBuildingBlock>  _newBuildingBlocks;
      private readonly IMoBiContext _context;
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public AddBuildingBlocksToModuleDTOToModuleMapper(IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory, IMoBiSpatialStructureFactory spatialStructureFactory)
      {
         _context = context;
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
         _spatialStructureFactory = spatialStructureFactory;
      }

      public IReadOnlyList<IBuildingBlock> MapFrom(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO)
      {
         var listOfBuildingBlocks = new List<IBuildingBlock>
         {
            conditionalCreate(addBuildingBlocksToModuleDTO.WithMolecule && !addBuildingBlocksToModuleDTO.AlreadyHasMolecule, () => addDefault<IMoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithReaction && !addBuildingBlocksToModuleDTO.AlreadyHasReaction, () => addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create())),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithSpatialStructure && !addBuildingBlocksToModuleDTO.AlreadyHasSpatialStructure, () => addDefault(AppConstants.DefaultNames.SpatialStructure, () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure))),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithPassiveTransport && !addBuildingBlocksToModuleDTO.AlreadyHasPassiveTransport, () => addDefault<IPassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithEventGroup && !addBuildingBlocksToModuleDTO.AlreadyHasEventGroup, () => addDefault<IEventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithObserver && !addBuildingBlocksToModuleDTO.AlreadyHasObserver, () => addDefault<IObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithParameterStartValues, () => addDefault<IParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues)),
            conditionalCreate(addBuildingBlocksToModuleDTO.WithMoleculeStartValues, () => addDefault<IMoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues))
         };

         _newBuildingBlocks = listOfBuildingBlocks.Where(x => x != null).ToList();


         return _newBuildingBlocks;
      }


      //all these here underneath we could move to a task to avoid code duplication
      //===========
      private TBuildingBlock conditionalCreate<TBuildingBlock>(bool shouldCreate, Func<TBuildingBlock> buildingBlockCreator) where TBuildingBlock : class, IBuildingBlock
      {
         return shouldCreate ? buildingBlockCreator() : null;
      }

      private T addDefault<T>(string defaultName) where T : class, IBuildingBlock
      {
         return addDefault(defaultName, _context.Create<T>);
      }

      private T addDefault<T>(string defaultName, Func<T> buildingBlockCreator) where T : IBuildingBlock
      {
         var buildingBlock = buildingBlockCreator().WithName(defaultName);
         return buildingBlock;
      }
   }
}