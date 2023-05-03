using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_AddBuildingBlocksToModuleDTO : ContextSpecification<AddBuildingBlocksToModuleDTO>
   {
      protected override void Context()
      {
         var module = new Module
         {
            new MoBiSpatialStructure(),
            new EventGroupBuildingBlock()
         }.WithName("module name");

         module.Add(new ParameterStartValuesBuildingBlock());
         module.Add(new MoleculeStartValuesBuildingBlock());

         sut = new AddBuildingBlocksToModuleDTO(module);
      }
   }

   public class When_adding_a_building_block_through_the_UI : concern_for_AddBuildingBlocksToModuleDTO
   {
      protected override void Because()
      {
         sut.WithObserver = true;
      }

      [Observation]
      public void the_dto_should_allow_adding_building_blocks_not_already_present()
      {
         sut.CanSelectEventGroup.ShouldBeFalse();
         sut.CanSelectMolecule.ShouldBeTrue();
         sut.CanSelectMoleculeStartValues.ShouldBeTrue();
         sut.CanSelectObserver.ShouldBeTrue();
         sut.CanSelectParameterStartValues.ShouldBeTrue();
         sut.CanSelectPassiveTransport.ShouldBeTrue();
         sut.CanSelectSpatialStructure.ShouldBeFalse();
         sut.CanSelectReaction.ShouldBeTrue();
      }

      [Observation]
      public void should_indicate_which_building_blocks_are_present()
      {
         sut.WithEventGroup.ShouldBeTrue();
         sut.WithMolecule.ShouldBeFalse();
         sut.WithMoleculeStartValues.ShouldBeFalse();
         sut.WithObserver.ShouldBeTrue();
         sut.WithParameterStartValues.ShouldBeFalse();
         sut.WithPassiveTransport.ShouldBeFalse();
         sut.WithSpatialStructure.ShouldBeTrue();
         sut.WithReaction.ShouldBeFalse();
      }

      [Observation]
      public void should_indicate_that_the_newly_selected_building_block_only_should_be_created()
      {
         sut.CreateEventGroup.ShouldBeFalse();
         sut.CreateMolecule.ShouldBeFalse();
         sut.CreateObserver.ShouldBeTrue();
         sut.CreatePassiveTransport.ShouldBeFalse();
         sut.CreateSpatialStructure.ShouldBeFalse();
         sut.CreateReaction.ShouldBeFalse();
      }
   }
}