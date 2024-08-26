using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IParameterValueBuildingBlockToParameterValuesDataTableMapper : IMapper<ParameterValuesBuildingBlock, List<DataTable>>
   {
   }

   public class ParameterValueBuildingBlockToParameterValuesDataTableMapper : BaseBuildingBlockToDataTableMapper<ParameterValuesBuildingBlock>, IParameterValueBuildingBlockToParameterValuesDataTableMapper
   {
      private static readonly string _name = AppConstants.Captions.ParameterName;
      private static readonly string _unit = AppConstants.Captions.Unit;

      protected override IEnumerable<PathAndValueEntity> GetElements(ParameterValuesBuildingBlock buildingBlock) =>
         buildingBlock.Select(x => x).Where(x => x.Value != null);

      protected override void AddSpecificColumns(DataTable dataTable)
      {
      }

      protected override void SetSpecificColumns(DataRow row, PathAndValueEntity element)
      {
      }

      protected override int GetColumnIndexForPath() => ColumnIndexes.ParameterRowIndexes.CONTAINER_PATH;

      protected override int GetColumnIndexForValue() => ColumnIndexes.ParameterRowIndexes.VALUE;

      protected override int GetColumnIndexForName() => ColumnIndexes.ParameterRowIndexes.NAME;

      protected override int GetColumnIndexForUnit() => ColumnIndexes.ParameterRowIndexes.UNIT;
   }
}