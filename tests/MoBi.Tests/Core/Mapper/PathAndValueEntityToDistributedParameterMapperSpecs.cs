using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Mapper
{
   public class concern_for_PathAndValueEntityToDistributedParameterMapper : ContextSpecification<PathAndValueEntityToDistributedParameterMapper>
   {
      protected IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new PathAndValueEntityToDistributedParameterMapper(_parameterFactory);
      }
   }

   public class When_creating_distributed_parameter_for_individual : concern_for_PathAndValueEntityToDistributedParameterMapper
   {
      private IndividualParameter _individualParameter;
      private IDistributedParameter _distributedParameter;
      private IReadOnlyList<IndividualParameter> _subParameters;
      private IDistributedParameter _result;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter { Name = "Name", Value = 5.0, DistributionType = DistributionType.Discrete};
         _subParameters = new[] { new IndividualParameter().WithName("Mean"), new IndividualParameter().WithName("Percentile") };

         _distributedParameter = new DistributedParameter().WithName("Name");
         A.CallTo(() => _parameterFactory.CreateDistributedParameter(_individualParameter.Name, _individualParameter.DistributionType.Value, A<double?>._, _individualParameter.Dimension, A<string>._, A<Unit>._)).Returns(_distributedParameter);
         A.CallTo(() => _parameterFactory.CreateParameter(A<string>._, A<double?>._, A<IDimension>._, A<string>._, A<IFormula>._, A<Unit>._)).ReturnsLazily(x => new Parameter().WithName(x.Arguments.Get<string>(0)));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_individualParameter, DistributionType.Discrete, _subParameters);
      }

      [Observation]
      public void the_value_should_be_saved_in_the_distributed_parameter()
      {
         _result.Value.ShouldBeEqualTo(_individualParameter.Value.Value);
      }

      [Observation]
      public void should_create_a_distributed_parameter()
      {
         _result.Name.ShouldBeEqualTo("Name");
      }

      [Observation]
      public void should_create_a_distributed_parameter_with_sub_parameters()
      {
         _result.Children.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_creating_a_distributed_parameter_whose_factory_already_pre_created_overlapping_sub_parameters : concern_for_PathAndValueEntityToDistributedParameterMapper
   {
      private IndividualParameter _individualParameter;
      private IDistributedParameter _distributedParameter;
      private IReadOnlyList<IndividualParameter> _subParameters;
      private IDistributedParameter _result;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter { Name = "Name", Value = 5.0, DistributionType = DistributionType.Discrete };
         _subParameters = new[] { new IndividualParameter { Name = "Mean", Value = 42.0 }, new IndividualParameter { Name = "Percentile", Value = 0.7 } };

         //factory now returns a distributed parameter that already has Mean and Percentile placeholders
         _distributedParameter = new DistributedParameter().WithName("Name");
         _distributedParameter.Add(new Parameter().WithName("Mean"));
         _distributedParameter.Add(new Parameter().WithName("Percentile"));

         A.CallTo(() => _parameterFactory.CreateDistributedParameter(_individualParameter.Name, _individualParameter.DistributionType.Value, A<double?>._, _individualParameter.Dimension, A<string>._, A<Unit>._)).Returns(_distributedParameter);
         A.CallTo(() => _parameterFactory.CreateParameter(A<string>._, A<double?>._, A<IDimension>._, A<string>._, A<IFormula>._, A<Unit>._))
            .ReturnsLazily(x => new Parameter().WithName(x.Arguments.Get<string>(0)).WithValue(x.Arguments.Get<double?>(1) ?? 0));
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_individualParameter, DistributionType.Discrete, _subParameters);
      }

      [Observation]
      public void should_not_throw_due_to_duplicate_sub_parameter_names()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_keep_a_single_child_per_sub_parameter_name()
      {
         _result.Children.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_caller_supplied_sub_parameter_values_should_win_over_the_factory_placeholders()
      {
         _result.GetSingleChildByName<IParameter>("Mean").Value.ShouldBeEqualTo(42.0);
      }
   }
}
