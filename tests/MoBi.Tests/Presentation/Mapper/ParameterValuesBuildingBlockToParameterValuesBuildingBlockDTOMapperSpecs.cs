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
   public class concern_for_ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper : ContextSpecification<ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper>
   {
      private IPathAndValueEntityToDistributedParameterMapper _mapper;

      protected override void Context()
      {
         _mapper = A.Fake<IPathAndValueEntityToDistributedParameterMapper>();
         sut = new ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper(new ParameterValueToParameterValueDTOMapper(new FormulaToValueFormulaDTOMapper()), _mapper);
      }

      public class When_mapping_the_building_block : concern_for_ParameterValuesBuildingBlockToParameterValuesBuildingBlockDTOMapper
      {
         private ParameterValuesBuildingBlock _buildingBlock;
         private ParameterValuesBuildingBlockDTO _result;

         protected override void Context()
         {
            base.Context();
            _buildingBlock = new ParameterValuesBuildingBlock()
            {
               new ParameterValue { Value = 1, Path = new ObjectPath("path1", "path2")},
               new ParameterValue { Value = 2, Path = new ObjectPath("path1", "path3") },
               new ParameterValue { Value = 3, Path = new ObjectPath("path1", "path4") },
               new ParameterValue {DistributionType = DistributionType.Normal, Path = new ObjectPath("path1", "path5", "distributed")},
               new ParameterValue {Path = new ObjectPath("path1", "path5", "distributed", Constants.Distribution.MEAN)}
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
         public void the_the_distribution_dto_should_contain_a_mean_child_parameter()
         {
            _result.ParameterDTOs.FindByName("distributed").SubParameters.FindByName(Constants.Distribution.MEAN).ShouldNotBeNull();
         }

         [Observation]
         public void the_dto_should_not_contain_direct_children_that_are_parts_of_a_distribution()
         {
            _result.ParameterDTOs.FindByName(Constants.Distribution.MEAN).ShouldBeNull();
         }

         [Observation]
         public void the_dto_should_contain_individual_parameter_dtos()
         {
            _result.ParameterDTOs.Count.ShouldBeEqualTo(4);
            _result.ParameterDTOs.Count(x => x.Value.Value.Equals(1)).ShouldBeEqualTo(1);
            _result.ParameterDTOs.Count(x => x.Value.Value.Equals(2)).ShouldBeEqualTo(1);
            _result.ParameterDTOs.Count(x => x.Value.Value.Equals(3)).ShouldBeEqualTo(1);
         }
      }
   }
}