using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class ExpressionParameterDTO : PathWithValueEntityDTO<ExpressionParameter>, IWithDisplayUnitDTO, IWithFormulaDTO
   {

      public ExpressionParameterDTO(ExpressionParameter expressionParameter) : base(expressionParameter)
      {
         ExpressionParameter = expressionParameter;
         ContainerPath = expressionParameter.ContainerPath;
      }

      public ExpressionParameter ExpressionParameter { get; }

      public double? Value
      {
         get
         {
            if (Formula == null || Formula.Formula == null)
               return ExpressionParameter.ConvertToDisplayUnit(ExpressionParameter.StartValue);
            return double.NaN;
         }
      }

      protected override IObjectPath GetContainerPath()
      {
         return ContainerPath;
      }
   }
}