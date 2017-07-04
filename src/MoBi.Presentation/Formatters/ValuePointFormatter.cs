using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Formatters
{
   public class ValuePointFormatter : UnitFormatter
   {
      private readonly ValuePointParameterDTO _valuePointParameterDTO;

      public ValuePointFormatter(ValuePointParameterDTO valuePointParameterDTO)
      {
         _valuePointParameterDTO = valuePointParameterDTO;
      }

      public override string Format(double valueToFormat)
      {
         return base.Format(valueToFormat, _valuePointParameterDTO.DisplayUnit);
      }
   }
}