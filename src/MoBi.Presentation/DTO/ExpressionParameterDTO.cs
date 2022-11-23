using MoBi.Core.Helper;
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
         ExpressionParameter.PropertyChanged += underlyingObjectOnPropertyChanged;
      }

      public double? Value
      {
         get
         {
            if (Formula == null || Formula.Formula == null)
               return ExpressionParameter.ConvertToDisplayUnit(ExpressionParameter.Value);
            return double.NaN;
         }
      }

      public ExpressionParameter ExpressionParameter { get; }

      protected override IObjectPath GetContainerPath(ExpressionParameter startValueObject)
      {
         return startValueObject.ContainerPath;
      }
   }
}