using System;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Format;

namespace MoBi.Presentation.Formatters
{
   public static class FormatterExtensions
   {
      public static IFormatter<double?> InitialConditionFormatter(this InitialConditionDTO initialConditionDTO)
      {
         return new ValueAllowingNaNFormatter(initialConditionDTO);
      }

      public static IFormatter<double?> ParameterValueFormatter(this ParameterValueDTO parameterValueDTO)
      {
         return new ValueAllowingNaNFormatter(parameterValueDTO);
      }

      public static IFormatter<double?> OriginalQuantityValueFormatter(this OriginalQuantityValueDTO originalQuantityValueDTO, Func<OriginalQuantityValueDTO, Unit> displayUnitRetriever)
      {
         return new NullableWithRetrievableDisplayUnitFormatter(() => displayUnitRetriever(originalQuantityValueDTO));
      }

      public static IFormatter<double?> IndividualParameterFormatter(this IndividualParameterDTO individualParameterDTO)
      {
         return new ValueAllowingNaNFormatter(individualParameterDTO);
      }

      public static IFormatter<double?> ExpressionParameterFormatter(this ExpressionParameterDTO expressionParameterDTO)
      {
         return new ValueAllowingNaNFormatter(expressionParameterDTO);
      }

      public static IFormatter<double?> PathAndValueEntityFormatter<T>(this PathAndValueEntityDTO<T> pathAndValueEntityDTO) where T : PathAndValueEntity
      {
         return new ValueAllowingNaNFormatter(pathAndValueEntityDTO);
      }
 
      public static IFormatter<double> ValuePointXFormatter(this DTOValuePoint valuePointDTO)
      {
         return new ValuePointFormatter(valuePointDTO.X);
      }

      public static IFormatter<double> ValuePointYFormatter(this DTOValuePoint valuePointDTO)
      {
         return new ValuePointFormatter(valuePointDTO.Y);
      }
   }
}