using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IParameterValueBuildingBlockToParameterValuesDataTableMapper : IMapper<ParameterValuesBuildingBlock, List<DataTable>>
   {
   }

   public class ParameterValueBuildingBlockToParameterValuesDataTableMapper : BaseBuildingBlockToDataTableMapper<ParameterValuesBuildingBlock>, IParameterValueBuildingBlockToParameterValuesDataTableMapper
   {
      protected override void SetColumnOrdinals()
      {
         var columnIndexes = GetColumnIndexes();

         foreach (var columnName in columnIndexes.Keys)
         {
            int index = columnIndexes[columnName];

            if (index >= 0 && index < _dt.Columns.Count)
            {
               _dt.Columns[columnName].SetOrdinal(index);
            }
         }
      }

      protected override Dictionary<string, int> GetColumnIndexes()
      {
         return new Dictionary<string, int>
         {
            { _path, ColumnIndexes.ParameterRowIndexes.CONTAINER_PATH },
            { _name, ColumnIndexes.ParameterRowIndexes.NAME },
            { _value, ColumnIndexes.ParameterRowIndexes.VALUE },
            { _unit, ColumnIndexes.ParameterRowIndexes.UNIT }
         };
      }

      protected override IEnumerable<PathAndValueEntity> GetElements(ParameterValuesBuildingBlock buildingBlock) =>
         buildingBlock.Select(x => x).Where(x => x.Value != null);
   }
}