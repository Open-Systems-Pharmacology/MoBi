using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper : ContextSpecification<ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper>
   {
      protected override void Context()
      {
         sut = new ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper(new ExpressionParameterToExpressionParameterDTOMapper(new FormulaToValueFormulaDTOMapper()));
      }

      public class When_mapping_the_building_block : concern_for_ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper
      {
         private ExpressionProfileBuildingBlock _buildingBlock;
         private ExpressionProfileBuildingBlockDTO _result;

         protected override void Context()
         {
            base.Context();
            _buildingBlock = new ExpressionProfileBuildingBlock
            {
               new ExpressionParameter { Value = 1, Path = new ObjectPath("path1", "path2")},
               new ExpressionParameter { Value = 2, Path = new ObjectPath("path1", "path3") },
               new ExpressionParameter { Value = 3, Path = new ObjectPath("path1", "path4") },
               new ExpressionParameter {DistributionType = DistributionType.Normal, Path = new ObjectPath("path1", "path5", "distributed")},
               new ExpressionParameter {Path = new ObjectPath("path1", "path5", "distributed", Constants.Distribution.MEAN)}
            };
         }

         protected override void Because()
         {
            _result = sut.MapFrom(_buildingBlock);
         }

         [Observation]
         public void the_dto_should_contain_distributed_dtos()
         {
            var distributedParameter = _result.ParameterDTOs.First(x => x.PathWithValueObject.DistributionType != null);
            distributedParameter.PathWithValueObject.Path.ShouldOnlyContainInOrder("path1", "path5", "distributed");
            distributedParameter.SubParameters.First().PathWithValueObject.Path.ShouldOnlyContainInOrder("path1", "path5", "distributed", Constants.Distribution.MEAN);
         }

         [Observation]
         public void the_dto_should_contain_expression_parameter_dtos()
         {
            _result.ParameterDTOs.Count.ShouldBeEqualTo(4);
            _result.ParameterDTOs.Count(x => x.Value.Value.Equals(1)).ShouldBeEqualTo(1);
            _result.ParameterDTOs.Count(x => x.Value.Value.Equals(2)).ShouldBeEqualTo(1);
            _result.ParameterDTOs.Count(x => x.Value.Value.Equals(3)).ShouldBeEqualTo(1);
         }
      }
   }
}