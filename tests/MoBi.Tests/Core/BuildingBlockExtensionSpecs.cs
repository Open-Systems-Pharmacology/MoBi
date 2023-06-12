using MoBi.Core.Domain.Extensions;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core
{
   public class concern_for_BuildingBlockExtension : StaticContextSpecification
   {
      protected SpatialStructure _buildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new SpatialStructure().WithName("spatialStructure");
      }
   }

   public class When_getting_the_caption_of_a_building_block_without_a_module : concern_for_BuildingBlockExtension
   {
      [Observation]
      public void the_caption_contains_the_module_name_and_the_building_block_name()
      {
         _buildingBlock.Caption().ShouldBeEqualTo("spatialStructure");
      }
   }

   public class When_getting_the_caption_of_a_building_block_within_a_module : concern_for_BuildingBlockExtension
   {
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _module = new Module().WithName("moduleName");
         _module.Add(_buildingBlock);
      }

      [Observation]
      public void the_caption_contains_the_module_name_and_the_building_block_name()
      {
         _buildingBlock.Caption().Contains("moduleName").ShouldBeTrue();
         _buildingBlock.Caption().Contains("spatialStructure").ShouldBeTrue();
      }
   }
}
