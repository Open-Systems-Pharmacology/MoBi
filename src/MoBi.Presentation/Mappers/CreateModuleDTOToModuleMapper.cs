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
   public interface ICreateModuleDTOToModuleMapper : IMapper<ModuleContentDTO, Module>
   {
   }

   public abstract class ModuleDTOMapper
   {
      protected readonly IMoBiContext _context;

      protected ModuleDTOMapper(IMoBiContext context)
      {
         _context = context;
      }

      protected TBuildingBlock ConditionalCreate<TBuildingBlock>(bool shouldCreate, Func<TBuildingBlock> buildingBlockCreator)
         where TBuildingBlock : class, IBuildingBlock
      {
         return shouldCreate ? buildingBlockCreator() : null;
      }

      protected T CreateDefault<T>(string defaultName) where T : class, IBuildingBlock => CreateDefault(defaultName, _context.Create<T>);

      protected T CreateDefault<T>(string defaultName, Func<T> buildingBlockCreator) where T : IBuildingBlock => buildingBlockCreator().WithName(defaultName);
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

      private void conditionalAdd<T>(Module module, T buildingBlock) where T : class, IBuildingBlock
      {
         if (buildingBlock == null)
            return;
         
         module.Add(buildingBlock);
      }

      public Module MapFrom(ModuleContentDTO createModuleDTO)
      {
         var module = _context.Create<Module>().WithIcon(ApplicationIcons.Module.IconName).WithName(createModuleDTO.Name);

         conditionalAdd(module, ConditionalCreate(createModuleDTO.WithSpatialStructure, () => CreateDefault(AppConstants.DefaultNames.SpatialStructure,
            () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure))));
         
         conditionalAdd(module, ConditionalCreate(createModuleDTO.WithReaction, () => CreateDefault(AppConstants.DefaultNames.ReactionBuildingBlock,
            () => _reactionBuildingBlockFactory.Create())));

         conditionalAdd(module, ConditionalCreate(createModuleDTO.WithMolecule, () => CreateDefault<MoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock)));
         conditionalAdd(module, ConditionalCreate(createModuleDTO.WithPassiveTransport, () => CreateDefault<PassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock)));
         conditionalAdd(module, ConditionalCreate(createModuleDTO.WithEventGroup, () => CreateDefault<EventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock)));
         conditionalAdd(module, ConditionalCreate(createModuleDTO.WithObserver, () => CreateDefault<ObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock)));

         if (createModuleDTO.WithParameterStartValues)
         {
            module.Add(CreateDefault<ParameterStartValuesBuildingBlock>(AppConstants.DefaultNames.ParameterStartValues));
         }

         if (createModuleDTO.WithMoleculeStartValues)
         {
            module.Add(CreateDefault<MoleculeStartValuesBuildingBlock>(AppConstants.DefaultNames.MoleculeStartValues));
         }

         return module;
      }
   }
}