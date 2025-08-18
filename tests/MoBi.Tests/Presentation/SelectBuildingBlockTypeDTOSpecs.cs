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
         sut.AllowedBuildingBlockTypes.ShouldOnlyContainInOrder(
            BuildingBlockType.SpatialStructure,
            BuildingBlockType.Molecules,
            BuildingBlockType.Reactions,
            BuildingBlockType.PassiveTransports,
            BuildingBlockType.Observers,
            BuildingBlockType.Events,
            BuildingBlockType.InitialConditions,
            BuildingBlockType.ParameterValues);
      }
   }

   public class When_initializing_with_full_module : concern_for_SelectBuildingBlockTypeDTO
   {
      protected override void Because()
      {
         _module.Add(new MoleculeBuildingBlock());
         _module.Add(new ReactionBuildingBlock());
         _module.Add(new SpatialStructure());
         _module.Add(new PassiveTransportBuildingBlock());
         _module.Add(new EventGroupBuildingBlock());
         _module.Add(new ObserverBuildingBlock());
         _module.Add(new ParameterValuesBuildingBlock());
         _module.Add(new InitialConditionsBuildingBlock());
         sut = new SelectBuildingBlockTypeDTO(_module);
      }

      [Observation]
      public void all_building_block_types_should_be_allowed()
      {
         sut.AllowedBuildingBlockTypes.Count.ShouldBeEqualTo(2);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.ParameterValues);
         sut.AllowedBuildingBlockTypes.ShouldContain(BuildingBlockType.InitialConditions);
      }
   }
}