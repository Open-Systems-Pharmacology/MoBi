using System;
using MoBi.Assets;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Extensions;
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

   public abstract class ModuleDTOMapper
   {
      protected readonly IMoBiContext _context;

      protected ModuleDTOMapper(IMoBiContext context)
      {
         _context = context;
      }

      protected TBuildingBlock conditionalCreate<TBuildingBlock>(bool shouldCreate, Func<TBuildingBlock> buildingBlockCreator)
         where TBuildingBlock : class, IBuildingBlock
      {
         return shouldCreate ? buildingBlockCreator() : null;
      }

      protected T addDefault<T>(string defaultName) where T : class, IBuildingBlock
      {
         return addDefault(defaultName, _context.Create<T>);
      }

      protected T addDefault<T>(string defaultName, Func<T> buildingBlockCreator) where T : IBuildingBlock
      {
         var buildingBlock = buildingBlockCreator().WithName(defaultName);
         return buildingBlock;
      }
   }

   public class CreateModuleDTOToModuleMapper : ModuleDTOMapper, ICreateModuleDTOToModuleMapper
   {
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;

      public CreateModuleDTOToModuleMapper(IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory,
         IMoBiSpatialStructureFactory spatialStructureFactory)
         : base(context)
      {
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
         _spatialStructureFactory = spatialStructureFactory;
      }

      public Module MapFrom(CreateModuleDTO createModuleDTO)
      {
         var module = _context.Create<Module>().WithIcon(ApplicationIcons.Module.IconName).WithName(createModuleDTO.Name);

         module.AddBuildingBlock(conditionalCreate(createModuleDTO.WithMolecule,
            () => addDefault<MoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock)));
         module.AddBuildingBlock(conditionalCreate(createModuleDTO.WithReaction,
            () => addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create())));
         module.AddBuildingBlock(conditionalCreate(createModuleDTO.WithSpatialStructure,
            () => addDefault(AppConstants.DefaultNames.SpatialStructure,
               () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure))));
         module.AddBuildingBlock(conditionalCreate(createModuleDTO.WithPassiveTransport,
            () => addDefault<PassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock)));
         module.AddBuildingBlock(conditionalCreate(createModuleDTO.WithEventGroup,
            () => addDefault<EventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock)));
         module.AddBuildingBlock(conditionalCreate(createModuleDTO.WithObserver,
            () => addDefault<ObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock)));

         if (createModuleDTO.WithParameterStartValues)
         {
            module.AddBuildingBlock(addDefault<ParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues));
         }

         if (createModuleDTO.WithMoleculeStartValues)
         {
            module.AddBuildingBlock(addDefault<MoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues));
         }

         return module;
      }
   }
}