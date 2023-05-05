using System.Collections.Generic;
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
   public class concern_for_AddBuildingBlocksToModuleDTOToBuildingBlocksListMapper : ContextSpecification<
      AddBuildingBlocksToModuleDTOToBuildingBlocksListMapper>
   {
      protected IMoBiSpatialStructureFactory _spatialStructureFactory;
      protected IReactionBuildingBlockFactory _reactionBuildingBlockFactory;
      private IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _spatialStructureFactory = A.Fake<IMoBiSpatialStructureFactory>();
         _reactionBuildingBlockFactory = A.Fake<IReactionBuildingBlockFactory>();
         sut = new AddBuildingBlocksToModuleDTOToBuildingBlocksListMapper(_context, _reactionBuildingBlockFactory, _spatialStructureFactory);
      }
   }

   public class
      When_mapping_dto_to_list_with_all_building_blocks_already_existing : concern_for_AddBuildingBlocksToModuleDTOToBuildingBlocksListMapper
   {
      private AddBuildingBlocksToModuleDTO _dto;
      private Module _existingModule;
      private IReadOnlyList<IBuildingBlock> _result;

      protected override void Context()
      {
         base.Context();
         _existingModule = new Module
         {
            new ObserverBuildingBlock(),
            new ReactionBuildingBlock(),
            new SpatialStructure(),
            new EventGroupBuildingBlock(),
            new MoleculeBuildingBlock(),
            new PassiveTransportBuildingBlock(),
            new InitialConditionsBuildingBlock(),
            new ParameterValuesBuildingBlock()
         };

         _dto = new AddBuildingBlocksToModuleDTO(_existingModule)
         {
            WithReaction = true,
            WithEventGroup = true,
            WithSpatialStructure = true,
            WithMolecule = true,
            WithObserver = true,
            WithPassiveTransport = true,
            WithParameterStartValues = false,
            WithMoleculeStartValues = false,
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_dto);
      }

      [Observation]
      public void the_list_should_have_no_values()
      {
         _result.ShouldBeEmpty();
      }
   }
}