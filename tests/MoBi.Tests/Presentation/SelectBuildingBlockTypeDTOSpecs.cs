using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_SelectBuildingBlockTypeDTO : ContextSpecification<SelectBuildingBlockTypeDTO>
   {
      protected Module _module;

      protected override void Context()
      {
         _module = new Module();
      }
   }

   public class When_initializing_with_empty_module : concern_for_SelectBuildingBlockTypeDTO
   {
      protected override void Because()
      {
         sut = new SelectBuildingBlockTypeDTO(_module);
      }

      [Observation]
      public void all_building_block_types_should_be_allowed()
      {
         sut.AllowedBuildingBlockTypes.Count.ShouldBeEqualTo(8);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.Reaction);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.Molecule);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.Observer);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.EventGroup);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.SpatialStructure);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.ParameterStartValues);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.MoleculeStartValues);
      }
   }

   public class When_initializing_with_full_module : concern_for_SelectBuildingBlockTypeDTO
   {
      protected override void Because()
      {
         _module.AddBuildingBlock(new MoleculeBuildingBlock());
         _module.AddBuildingBlock(new ReactionBuildingBlock());
         _module.AddBuildingBlock(new SpatialStructure());
         _module.AddBuildingBlock(new PassiveTransportBuildingBlock());
         _module.AddBuildingBlock(new EventGroupBuildingBlock());
         _module.AddBuildingBlock(new ObserverBuildingBlock());
         _module.AddBuildingBlock(new ParameterStartValuesBuildingBlock());
         _module.AddBuildingBlock(new MoleculeStartValuesBuildingBlock());
         sut = new SelectBuildingBlockTypeDTO(_module);
      }

      [Observation]
      public void all_building_block_types_should_be_allowed()
      {
         sut.AllowedBuildingBlockTypes.Count.ShouldBeEqualTo(2);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.ParameterStartValues);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.MoleculeStartValues);
      }
   }
}