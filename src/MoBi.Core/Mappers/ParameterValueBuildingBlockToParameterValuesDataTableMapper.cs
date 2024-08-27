using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IParameterValueBuildingBlockToParameterValuesDataTableMapper : IMapper<ParameterValuesBuildingBlock, List<DataTable>>
   {
   }

   public class ParameterValueBuildingBlockToParameterValuesDataTableMapper : PathAndValueBuildingBlockToDataTableMapper<ParameterValuesBuildingBlock, ParameterValue>, IParameterValueBuildingBlockToParameterValuesDataTableMapper
   {
      protected override string Name => AppConstants.Captions.ParameterName;

      protected override Dictionary<string, int> GetColumnIndexes()
      {
         return new Dictionary<string, int>
         {
            { Path, ColumnIndexes.Parameters.CONTAINER_PATH },
            { Name, ColumnIndexes.Parameters.NAME },
            { Value, ColumnIndexes.Parameters.VALUE },
            { Unit, ColumnIndexes.Parameters.UNIT }
         };
      }
   }
}