using MoBi.Core.Domain.Extensions;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core
{
   public abstract class concern_forModuleExtensions : StaticContextSpecification
   {
      protected Module _module;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _module = new Module();
      }
   }

   class When_adding_a_building_block_to_a_module : concern_forModuleExtensions
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlock = new ReactionBuildingBlock().WithId("newReactionBuildingBlock");
      }

      protected override void Because()
      {
         _module.AddBuildingBlock(_buildingBlock);
      }

      [Observation]
      public void should_add_a_reaction()
      {
         _module.Reactions.ShouldBeEqualTo(_buildingBlock);
         _module.AllBuildingBlocks().Count.ShouldBeEqualTo(1);
      }
   }

   class When_adding_a_not_supported_building_block_to_a_module : concern_forModuleExtensions
   {
      protected override void Because()
      {
         _buildingBlock = new ExpressionProfileBuildingBlock();
      }

      [Observation]
      public void nothing_should_be_added()
      {
         _module.AllBuildingBlocks().Count.ShouldBeEqualTo(0);
      }
   }

   class When_adding_a_null_building_block_to_a_module : concern_forModuleExtensions
   {
      protected override void Because()
      {
         _buildingBlock = null;
      }

      [Observation]
      public void nothing_should_be_added()
      {
         _module.AllBuildingBlocks().Count.ShouldBeEqualTo(0);
      }
   }
}