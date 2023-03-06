using System;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface ICreateModuleDTOToModuleMapper : IMapper<CreateModuleDTO, Module>
   {
      Module AddSelectedBuildingBlocks(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO, Module module);
   }
   
   public class CreateModuleDTOToModuleMapper : ICreateModuleDTOToModuleMapper
   {
      private readonly IMoBiContext _context;
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public CreateModuleDTOToModuleMapper(IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory, IMoBiSpatialStructureFactory spatialStructureFactory)
      {
         _context = context;
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
         _spatialStructureFactory = spatialStructureFactory;
      }
      
      public Module MapFrom(CreateModuleDTO createModuleDTO)
      {
         var module = _context.Create<Module>().WithIcon(ApplicationIcons.Module.IconName).WithName(createModuleDTO.Name);

         module.Molecule = conditionalCreate(createModuleDTO.WithMolecule, () => addDefault<IMoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock));
         module.Reaction = conditionalCreate(createModuleDTO.WithReaction, () => addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create()));
         module.SpatialStructure = conditionalCreate(createModuleDTO.WithSpatialStructure, () => addDefault(AppConstants.DefaultNames.SpatialStructure, () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure)));
         module.PassiveTransport = conditionalCreate(createModuleDTO.WithPassiveTransport, () => addDefault<IPassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock));
         module.EventGroup = conditionalCreate(createModuleDTO.WithEventGroup, () => addDefault<IEventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock));
         module.Observer = conditionalCreate(createModuleDTO.WithObserver, () => addDefault<IObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock));

         if (createModuleDTO.WithParameterStartValues)
         {
            module.AddParameterStartValueBlock(addDefault<IParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues));
         }

         if (createModuleDTO.WithMoleculeStartValues)
         {
            module.AddMoleculeStartValueBlock(addDefault<IMoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues));
         }

         return module;
      }

      public Module AddSelectedBuildingBlocks(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO, Module module)
      {
         module.Molecule = conditionalCreate(addBuildingBlocksToModuleDTO.WithMolecule, () => addDefault<IMoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock));
         module.Reaction = conditionalCreate(addBuildingBlocksToModuleDTO.WithReaction, () => addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create()));
         module.SpatialStructure = conditionalCreate(addBuildingBlocksToModuleDTO.WithSpatialStructure, () => addDefault(AppConstants.DefaultNames.SpatialStructure, () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure)));
         module.PassiveTransport = conditionalCreate(addBuildingBlocksToModuleDTO.WithPassiveTransport, () => addDefault<IPassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock));
         module.EventGroup = conditionalCreate(addBuildingBlocksToModuleDTO.WithEventGroup, () => addDefault<IEventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock));
         module.Observer = conditionalCreate(addBuildingBlocksToModuleDTO.WithObserver, () => addDefault<IObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock));

         if (addBuildingBlocksToModuleDTO.WithParameterStartValues)
         {
            module.AddParameterStartValueBlock(addDefault<IParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues));
         }

         if (addBuildingBlocksToModuleDTO.WithMoleculeStartValues)
         {
            module.AddMoleculeStartValueBlock(addDefault<IMoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues));
         }

      }

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
