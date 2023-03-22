using FakeItEasy;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_AddBuildingBlocksToModuleDTOToModuleMapper : ContextSpecification<AddBuildingBlocksToModuleDTOToModuleMapper>
   {
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected IReactionBuildingBlockFactory _reactionBuildingBlockFactory;
      private IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _reactionBuildingBlockFactory = A.Fake<IReactionBuildingBlockFactory>();
         sut = new AddBuildingBlocksToModuleDTOToModuleMapper(_context, _reactionBuildingBlockFactory, _spatialStructureFactory);
      }
   }

   public class When_mapping_dto_to_module_with_all_building_blocks_already_existing : concern_for_AddBuildingBlocksToModuleDTOToModuleMapper
   {
      private AddBuildingBlocksToModuleDTO _dto;
      private Module _existingModule;
      private Module _result;

      protected override void Context()
      {
         base.Context();
         _existingModule = new Module
         {
            Observer = new ObserverBuildingBlock(),
            Reaction = new ReactionBuildingBlock(),
            SpatialStructure = new SpatialStructure(),
            EventGroup = new EventGroupBuildingBlock(),
            Molecule = new MoleculeBuildingBlock(),
            PassiveTransport = new PassiveTransportBuildingBlock()
         };
         _existingModule.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         _existingModule.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());

         _dto = new AddBuildingBlocksToModuleDTO(_existingModule)
         {
            WithReaction = true,
            WithEventGroup = true,
            WithSpatialStructure = true,
            WithMolecule = true,
            WithObserver = true,
            WithPassiveTransport = true,
            WithParameterStartValues = true,
            WithMoleculeStartValues = true,
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void the_module_should_contain_only_the_start_values()
      {
         _result.SpatialStructure.ShouldBeNull();
         _result.Reaction.ShouldBeNull();
         _result.EventGroup.ShouldBeNull();
         _result.MoleculeStartValuesCollection.ShouldNotBeEmpty();
         _result.ParameterStartValuesCollection.ShouldNotBeEmpty();
         _result.PassiveTransport.ShouldBeNull();
         _result.Observer.ShouldBeNull();
         _result.Molecule.ShouldBeNull();
      }
   }
}