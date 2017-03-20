using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation
{
   public abstract class concern_for_ParameterDTO : ContextSpecification<ParameterDTO>
   {
      protected IParameter _parameter;

      protected override void Context()
      {
         _parameter = new Parameter().WithName("A");
         sut = new ParameterDTO(_parameter) {Name = _parameter.Name};
      }
   }

   public class When_creating_a_parameter_dto_for_a_constant_aprameter_with_an_undefined_value : concern_for_ParameterDTO
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Formula = new ConstantFormula(double.NaN);
         sut.Formula = new ConstantFormulaBuilderDTO();
      }

      [Observation]
      public void should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_creating_a_parameter_dto_for_a_constant_aprameter_with_an_well_defined_value : concern_for_ParameterDTO
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Formula = new ConstantFormula(5);
         sut.Formula = new ConstantFormulaBuilderDTO();
      }

      [Observation]
      public void should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }
}