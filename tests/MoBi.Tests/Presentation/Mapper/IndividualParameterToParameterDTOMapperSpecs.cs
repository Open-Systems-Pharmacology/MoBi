using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_IndividualParameterToParameterDTOMapper : ContextSpecification<IndividualParameterToParameterDTOMapper>
   {
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      protected IFormulaFactory _formulaFactory;
      protected IParameterValueToParameterMapper _parameterValueToParameterMapper;
      protected IParameterToParameterDTOMapper _parameterToParameterDTOMapper;

      protected override void Context()
      {
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         _formulaFactory = A.Fake<IFormulaFactory>();
         _parameterValueToParameterMapper = A.Fake<IParameterValueToParameterMapper>();
         _parameterToParameterDTOMapper = A.Fake<IParameterToParameterDTOMapper>();
         sut = new IndividualParameterToParameterDTOMapper(_parameterToParameterDTOMapper, _parameterValueToParameterMapper, _formulaFactory, _cloneManagerForBuildingBlock);

         A.CallTo(() => _parameterToParameterDTOMapper.MapFrom(A<IParameter>._)).ReturnsLazily(x => new ParameterDTO(x.GetArgument<IParameter>(0)));
      }
   }

   internal abstract class When_mapping_from_individual_to_dto_for_individual_parameter : concern_for_IndividualParameterToParameterDTOMapper
   {
      protected IndividualParameter _individualParameter;
      protected IndividualBuildingBlock _buildingBlock;
      protected ParameterDTO _dto;

      protected override void Context()
      {
         base.Context();
         _individualParameter = GetParameter();
         _buildingBlock = new IndividualBuildingBlock { _individualParameter };
      }

      protected abstract IndividualParameter GetParameter();

      protected override void Because()
      {
         _dto = sut.MapFrom(_buildingBlock, _individualParameter);
      }

      [Observation]
      public void the_parameter_mapper_is_used_to_create_the_parameter()
      {
         A.CallTo(() => _parameterValueToParameterMapper.MapFrom(_individualParameter)).MustHaveHappened();
      }

      [Observation]
      public void the_dto_mapper_is_used_to_create_the_dto()
      {
         A.CallTo(() => _parameterToParameterDTOMapper.MapFrom(A<IParameter>._)).MustHaveHappened();
      }
   }

   internal class When_mapping_from_individual_to_dto_for_simple_parameter_with_value : When_mapping_from_individual_to_dto_for_individual_parameter
   {
      protected override IndividualParameter GetParameter()
      {
         return new IndividualParameter { Value = 4 };
      }

      [Observation]
      public void the_formula_factory_creates_a_formula_from_the_value()
      {
         A.CallTo(() => _formulaFactory.ConstantFormula(_individualParameter.Value.Value, _individualParameter.Dimension)).MustHaveHappened();
      }
   }

   internal class When_mapping_from_individual_to_dto_for_simple_parameter_with_formula : When_mapping_from_individual_to_dto_for_individual_parameter
   {
      protected override IndividualParameter GetParameter()
      {
         return new IndividualParameter { Formula = new ConstantFormula(4) };
      }

      [Observation]
      public void the_formula_factory_creates_a_formula_from_the_value()
      {
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_individualParameter.Formula, A<IFormulaCache>._)).MustHaveHappened();
      }
   }

   internal class When_mapping_from_individual_to_dto_for_distributed_parameter : When_mapping_from_individual_to_dto_for_individual_parameter
   {
      protected override void Context()
      {
         base.Context();
         _buildingBlock.Add(new IndividualParameter
         {
            ContainerPath = new ObjectPath(_individualParameter.Path.ToArray()),
            Name = "SubParameter"
         });

         A.CallTo(() => _parameterValueToParameterMapper.MapFrom(_individualParameter)).Returns(new DistributedParameter().WithName(_individualParameter.Name));
      }

      protected override IndividualParameter GetParameter()
      {
         return new IndividualParameter
         {
            DistributionType = DistributionType.Normal,
            Name = "Parameter"
         };
      }

      [Observation]
      public void the_dto_should_be_distributed()
      {
         _dto.Parameter.IsDistributed().ShouldBeTrue();
      }

      [Observation]
      public void the_sub_parameters_should_be_contained_in_the_parameter()
      {
         var distributedParameter = _dto.Parameter as DistributedParameter;
         
         distributedParameter.Count().ShouldBeEqualTo(1);
      }
   }
}