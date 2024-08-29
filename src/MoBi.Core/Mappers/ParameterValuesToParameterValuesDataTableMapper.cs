using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IParameterValuesToParameterValuesDataTableMapper : IMapper<IEnumerable<ParameterValue>, List<DataTable>>
   {
   }

   public class ParameterValuesToParameterValuesDataTableMapper : ParametersToDataTableMapper<ParameterValue>, IParameterValuesToParameterValuesDataTableMapper
   {
      protected override string TableName => AppConstants.Captions.ParameterValues;
   }
}