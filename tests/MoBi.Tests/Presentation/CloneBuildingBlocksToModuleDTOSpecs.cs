using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_CloneBuildingBlocksToModuleDTO : ContextSpecification<CloneBuildingBlocksToModuleDTO>
   {
      protected Module _module;

      protected override void Context()
      {
         _module = new Module
         {
            new MoBiSpatialStructure(),
            new EventGroupBuildingBlock()
         }.WithName("module name");
         _module.Add(new ParameterValuesBuildingBlock());
         _module.Add(new InitialConditionsBuildingBlock());

         sut = new CloneBuildingBlocksToModuleDTO(_module);
      }
   }
   
   public class When_cloning_a_module_building_block_through_the_UI : concern_for_CloneBuildingBlocksToModuleDTO
   {
      protected override void Because()
      {
         sut.WithSpatialStructure = false;
      }

      [Observation]
      public void the_dto_should_allow_retaining_building_blocks_only_if_already_present()
      {
         sut.CanSelectEventGroup.ShouldBeTrue();
         sut.CanSelectMolecule.ShouldBeFalse();
         sut.CanSelectInitialConditions.ShouldBeTrue();
         sut.CanSelectObserver.ShouldBeFalse();
         sut.CanSelectParameterValues.ShouldBeTrue();
         sut.CanSelectPassiveTransport.ShouldBeFalse();
         sut.CanSelectSpatialStructure.ShouldBeTrue();
         sut.CanSelectReaction.ShouldBeFalse();
      }

      [Observation]
      public void should_indicate_which_building_blocks_are_retained()
      {
         sut.WithEventGroup.ShouldBeTrue();
         sut.WithMolecule.ShouldBeFalse();
         sut.WithInitialConditions.ShouldBeTrue();
         sut.WithObserver.ShouldBeFalse();
         sut.WithParameterValues.ShouldBeTrue();
         sut.WithPassiveTransport.ShouldBeFalse();
         sut.WithSpatialStructure.ShouldBeFalse();
         sut.WithReaction.ShouldBeFalse();
      }

      [Observation]
      public void should_indicate_that_the_newly_deselected_building_block_only_should_be_removed()
      {
         sut.BuildingBlocksToRemove.ShouldOnlyContain(_module.SpatialStructure);
      }
   }
}