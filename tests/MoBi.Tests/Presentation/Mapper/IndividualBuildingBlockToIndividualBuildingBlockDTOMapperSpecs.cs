using System.Linq;
using FakeItEasy;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_IndividualBuildingBlockToIndividualBuildingBlockDTOMapper : ContextSpecification<IndividualBuildingBlockToIndividualBuildingBlockDTOMapper>
   {
      private IPathAndValueEntityToDistributedParameterMapper _mapper;

      protected override void Context()
      {
         _mapper = A.Fake<IPathAndValueEntityToDistributedParameterMapper>();
         sut = new IndividualBuildingBlockToIndividualBuildingBlockDTOMapper(new IndividualParameterToIndividualParameterDTOMapper(new FormulaToValueFormulaDTOMapper()), _mapper);
      }

      public class When_mapping_the_building_block : concern_for_IndividualBuildingBlockToIndividualBuildingBlockDTOMapper
      {
         private IndividualBuildingBlock _buildingBlock;
         private IndividualBuildingBlockDTO _result;

         protected override void Context()
         {
            base.Context();
            _buildingBlock = new IndividualBuildingBlock
            {
               new IndividualParameter { Value = 1, Path = new ObjectPath("path1", "path2")},
               new IndividualParameter { Value = 2, Path = new ObjectPath("path1", "path3") },
               new IndividualParameter { Value = 3, Path = new ObjectPath("path1", "path4") },
               new IndividualParameter {DistributionType = DistributionType.Normal, Path = new ObjectPath("path1", "path5", "distributed")},
               new IndividualParameter {Path = new ObjectPath("path1", "path5", "distributed", Constants.Distribution.MEAN)}
            };
         }

         protected override void Because()
         {
            _result = sut.MapFrom(_buildingBlock);
         }

         [Observation]
         public void the_dto_should_contain_distributed_dtos()
         {
            var distributedParameter = _result.Parameters.First(x => x.PathWithValueObject.DistributionType != null);
            distributedParameter.PathWithValueObject.Path.ShouldOnlyContainInOrder("path1", "path5", "distributed");
            distributedParameter.SubParameters.First().PathWithValueObject.Path.ShouldOnlyContainInOrder("path1", "path5", "distributed", Constants.Distribution.MEAN);
         }

         [Observation]
         public void the_the_distribution_dto_should_contain_a_mean_child_parameter()
         {
            _result.Parameters.FindByName("distributed").SubParameters.FindByName(Constants.Distribution.MEAN).ShouldNotBeNull();
         }

         [Observation]
         public void the_dto_should_not_contain_direct_children_that_are_parts_of_a_distribution()
         {
            _result.Parameters.FindByName(Constants.Distribution.MEAN).ShouldBeNull();
         }

         [Observation]
         public void the_dto_should_contain_individual_parameter_dtos()
         {
            _result.Parameters.Count.ShouldBeEqualTo(4);
            _result.Parameters.Count(x => x.Value.Value.Equals(1)).ShouldBeEqualTo(1);
            _result.Parameters.Count(x => x.Value.Value.Equals(2)).ShouldBeEqualTo(1);
            _result.Parameters.Count(x => x.Value.Value.Equals(3)).ShouldBeEqualTo(1);
         }
      }
   }
}