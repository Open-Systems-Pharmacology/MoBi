using System;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ExpressionProfileToParameterStartValuesBBMapper : ContextSpecification<ExpressionProfileToParameterStartValuesMapper>
   {
      private IObjectBaseFactory _objectFactory;
      private ICloneManagerForModel _cloneManager;
      private ExpressionParameterToParameterStartValueMapper _expressionParameterToParameterStartValueMapper;

      protected override void Context()
      {
         _objectFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectFactory.Create<ParameterValue>()).ReturnsLazily(x => new ParameterValue());
         A.CallTo(() => _objectFactory.Create<ParameterValuesBuildingBlock>()).ReturnsLazily(x => new ParameterValuesBuildingBlock());
         A.CallTo(() => _objectFactory.CreateObjectBaseFrom(A<IFormula>._)).ReturnsLazily(x => new ExplicitFormula().WithId(Guid.NewGuid().ToString()));

         _cloneManager = new CloneManagerForModel(_objectFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());

         _expressionParameterToParameterStartValueMapper = new ExpressionParameterToParameterStartValueMapper(_objectFactory, _cloneManager);
         sut = new ExpressionProfileToParameterStartValuesMapper(_expressionParameterToParameterStartValueMapper, _objectFactory);
      }
   }

   public class When_mapping_from_expression_profile_to_start_values_bb_building_block : concern_for_ExpressionProfileToParameterStartValuesBBMapper
   {
      private ExpressionProfileBuildingBlock _expressionProfile;
      private ParameterValuesBuildingBlock _result;

      protected override void Context()
      {
         base.Context();
         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("BB Name");

         var expressionParameter = createExpressionParameterWithName("parameter1");
         addExpressionParameter(expressionParameter);
         var formula1 = expressionParameter.Formula;

         expressionParameter = createExpressionParameterWithName("parameter2");
         addExpressionParameter(expressionParameter);

         expressionParameter = createExpressionParameterWithName("parameter3");
         expressionParameter.Formula = formula1;

         // Substitute the formula for an existing one to make sure formulae are unique by name
         addExpressionParameter(expressionParameter);
      }

      private void addExpressionParameter(ExpressionParameter expressionParameter)
      {
         _expressionProfile.Add(expressionParameter);
         if (expressionParameter.Formula != null)
            _expressionProfile.AddFormula(expressionParameter.Formula);
      }

      private static ExpressionParameter createExpressionParameterWithName(string parameterName)
      {
         var expressionParameter = new ExpressionParameter().WithName(parameterName);
         expressionParameter.Formula = new ExplicitFormula("X").WithName($"Formula for {parameterName}").WithId($"Id for {parameterName}");

         return expressionParameter;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_expressionProfile);
      }

      [Observation]
      public void should_map_building_block_name()
      {
         _result.Name.ShouldBeEqualTo(_expressionProfile.Name);
      }

      [Observation]
      public void there_should_be_corresponding_parameter_start_values_for_each_expression_parameter()
      {
         _expressionProfile.Each(x => _result.Count(y => x.Name.Equals(y.Name)).ShouldBeEqualTo(1));
      }

      [Observation]
      public void the_formula_cache_should_only_contain_a_single_instance_of_a_formula_with_name()
      {
         _result.FormulaCache.Count.ShouldBeEqualTo(2);
         _result.FormulaCache.Distinct(new NameComparer<IFormula>()).Count().ShouldBeEqualTo(_result.FormulaCache.Count);
      }
   }
}