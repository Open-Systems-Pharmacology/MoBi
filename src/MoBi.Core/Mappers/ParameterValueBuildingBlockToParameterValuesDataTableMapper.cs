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

      protected override DataTable GenerateEmptyDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn(_path);
         dt.AddColumn<string>(_name);
         dt.AddColumn<double>(_value);
         dt.AddColumn(_unit);
         dt.TableName = AppConstants.Captions.ParameterValue;
         return dt;
      }

      protected override void SetSpecificColumns(DataRow row, PathAndValueEntity element)
      {
         row[_name] = element.Name;
         row[_unit] = element.DisplayUnit;
      }
   }
}