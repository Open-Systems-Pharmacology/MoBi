using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_ValuePointFormatter : ContextSpecification<ValuePointFormatter>
   {
      private ValuePointParameterDTO _valuePointParameterDTO;

      protected override void Context()
      {
         _valuePointParameterDTO = new ValuePointParameterDTO {DisplayUnit = new Unit("mg", 1, 0)};
         sut = new ValuePointFormatter(_valuePointParameterDTO);
      }
   }

   public class When_formatting_a_value_point_parameter : concern_for_ValuePointFormatter
   {
      [Observation]
      public void should_return_the_formatted_value_with_unit()
      {
         sut.Format(5).ShouldBeEqualTo(new UnitFormatter().Format(5, "mg"));
      }
   }
}	