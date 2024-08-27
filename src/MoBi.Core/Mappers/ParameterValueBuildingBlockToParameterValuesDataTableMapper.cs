using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IParameterValueBuildingBlockToParameterValuesDataTableMapper : IMapper<ParameterValuesBuildingBlock, List<DataTable>>
   {
   }

   public class ParameterValueBuildingBlockToParameterValuesDataTableMapper : PathAndValueBuildingBlockToDataTableMapper<ParameterValuesBuildingBlock, ParameterValue>, IParameterValueBuildingBlockToParameterValuesDataTableMapper
   {
      protected override string Name => AppConstants.Captions.ParameterName;

      protected override void SetColumnOrdinals()
      {
         var columnIndexes = getColumnIndexes();

         foreach (var columnName in columnIndexes.Keys)
         {
            int index = columnIndexes[columnName];

            if (index >= 0 && index < _dt.Columns.Count)
            {
               _dt.Columns[columnName].SetOrdinal(index);
            }
         }
      }

      private Dictionary<string, int> getColumnIndexes()
      {
         return new Dictionary<string, int>
         {
            { Path, ColumnIndexes.ParameterRowIndexes.CONTAINER_PATH },
            { Name, ColumnIndexes.ParameterRowIndexes.NAME },
            { Value, ColumnIndexes.ParameterRowIndexes.VALUE },
            { Unit, ColumnIndexes.ParameterRowIndexes.UNIT }
         };
      }
   }
}