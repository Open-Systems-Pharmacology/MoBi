using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Mappers
{
   public abstract class ParametersToDataTableMapper<TBuilder> : PathAndValuesToDataTableMapper<TBuilder> where TBuilder : PathAndValueEntity
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