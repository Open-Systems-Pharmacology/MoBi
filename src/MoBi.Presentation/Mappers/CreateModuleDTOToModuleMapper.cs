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

         module.Molecules = conditionalCreate(createModuleDTO.WithMolecule, () => addDefault<MoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock));
         module.Reactions = conditionalCreate(createModuleDTO.WithReaction, () => addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create()));
         module.SpatialStructure = conditionalCreate(createModuleDTO.WithSpatialStructure, () => addDefault(AppConstants.DefaultNames.SpatialStructure, () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure)));
         module.PassiveTransports = conditionalCreate(createModuleDTO.WithPassiveTransport, () => addDefault<IPassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock));
         module.EventGroups = conditionalCreate(createModuleDTO.WithEventGroup, () => addDefault<IEventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock));
         module.Observers = conditionalCreate(createModuleDTO.WithObserver, () => addDefault<IObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock));

         if (createModuleDTO.WithParameterStartValues)
         {
            module.AddParameterStartValueBlock(addDefault<ParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues));
         }

         if (createModuleDTO.WithMoleculeStartValues)
         {
            module.AddMoleculeStartValueBlock(addDefault<MoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues));
         }

         return module;
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
