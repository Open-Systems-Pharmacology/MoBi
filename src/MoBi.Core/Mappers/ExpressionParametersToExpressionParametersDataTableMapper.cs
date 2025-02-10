using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IExpressionParametersToExpressionParametersDataTableMapper : IMapper<IEnumerable<ExpressionParameter>, List<DataTable>>
   {
   }

   public class ExpressionParametersToExpressionParametersDataTableMapper : ParametersToDataTableMapper<ExpressionParameter>, IExpressionParametersToExpressionParametersDataTableMapper
   {
      protected override string TableName => AppConstants.Captions.ExpressionParameters;
   }
}