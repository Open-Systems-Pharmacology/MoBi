using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   internal class IndividualExpressionAndFilePathDTOMapperSpecs : ContextSpecification<IndividualExpressionAndFilePathDTOMapper>
   {
      protected override void Context()
      {
         sut = new IndividualExpressionAndFilePathDTOMapper();
      }
   }

   internal class When_mapping_from_expression_profile_building_blocks : IndividualExpressionAndFilePathDTOMapperSpecs
   {
      private IEnumerable<ExpressionProfileBuildingBlock> _expressionProfileBuildingBlocks;
      private IndividualExpressionAndFilePathDTO _result;

      protected override void Context()
      {
         base.Context();
         _expressionProfileBuildingBlocks = new List<ExpressionProfileBuildingBlock>
         {
            new ExpressionProfileBuildingBlock(),
            new ExpressionProfileBuildingBlock()
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_expressionProfileBuildingBlocks);
      }

      [Observation]
      public void should_map_the_expression_profile_building_blocks_to_the_dto()
      {
         _result.SelectableExpressionProfiles.Count.ShouldBeEqualTo(2);
      }
   }
}
