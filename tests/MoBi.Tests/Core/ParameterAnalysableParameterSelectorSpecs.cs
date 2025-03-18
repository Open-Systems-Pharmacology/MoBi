using FakeItEasy;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_ParameterAnalysableParameterSelector : ContextSpecification<IParameterAnalysableParameterSelector>
   {
      protected override void Context()
      {
         sut = new ParameterAnalysableParameterSelector();
      }
   }

   public class When_checking_if_a_parameter_can_be_used_in_a_parameter_analysis : concern_for_ParameterAnalysableParameterSelector
   {
      private Parameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter();
      }

      [Observation]
      public void should_return_false_if_the_parameter_cannot_be_varied()
      {
         _parameter.CanBeVaried = false;
         sut.CanUseParameter(_parameter).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_parameter_formula_is_table()
      {
         _parameter.CanBeVaried = true;
         _parameter.Formula = new TableFormula();
         sut.CanUseParameter(_parameter).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_parmaeter_is_a_categorial_parameter()
      {
         _parameter.CanBeVaried = true;
         _parameter.Name = Constants.Parameters.COMPOUND_TYPE1;
         sut.CanUseParameter(_parameter).ShouldBeFalse();
      }

      [Observation]
      public void should_return_true_otherwise()
      {
         _parameter.CanBeVaried = true;
         _parameter.Name = "Hello Parameter";
         sut.CanUseParameter(_parameter).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_distributed_sub_parameter()
      {
         _parameter.CanBeVaried = true;
         _parameter.Name = "Hello Parameter";
         new DistributedParameter { _parameter };
         sut.CanUseParameter(_parameter).ShouldBeFalse();
      }
   }
}	