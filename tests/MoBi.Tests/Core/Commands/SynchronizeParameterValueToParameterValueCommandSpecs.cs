using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SynchronizeParameterValueToParameterValueCommand : ContextSpecification<SynchronizeParameterValueToParameterValueCommand>
   {
      protected IParameter _sourceParameter;
      protected IParameter _targetParameter;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _sourceParameter = new Parameter().WithFormula(new ConstantFormula(5));
         _sourceParameter.Dimension = DomainHelperForSpecs.AmountDimension;
         _targetParameter = new Parameter().WithFormula(new ConstantFormula(10));
         _targetParameter.Dimension = DomainHelperForSpecs.TimeDimension;

         sut = new SynchronizeParameterValueToParameterValueCommand(_sourceParameter, _targetParameter);

         _context = A.Fake<IMoBiContext>();
      }
   }

   public class When_executing_the_synchronize_parameter_value_command : concern_for_SynchronizeParameterValueToParameterValueCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_ensure_that_the_target_parameter_has_the_same_value_as_the_source_parameter()
      {
         _targetParameter.Value.ShouldBeEqualTo(_sourceParameter.Value);
      }

      [Observation]
      public void should_ensure_that_the_target_parameter_has_the_same_value_origin_as_the_source_parameter()
      {
         _targetParameter.ValueOrigin.ShouldBeEqualTo(_sourceParameter.ValueOrigin);
      }

      [Observation]
      public void should_ensure_that_the_target_parameter_has_the_same_dimension_as_the_source_parameter()
      {
         _targetParameter.Dimension.ShouldBeEqualTo(_sourceParameter.Dimension);
      }

      [Observation]
      public void should_ensure_that_the_target_parameter_has_the_same_display_unit_as_the_source_parameter()
      {
         _targetParameter.DisplayUnit.ShouldBeEqualTo(_sourceParameter.DisplayUnit);
      }
   }
}