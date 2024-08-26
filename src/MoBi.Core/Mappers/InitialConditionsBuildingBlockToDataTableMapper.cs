using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IInitialConditionsBuildingBlockToDataTableMapper : IMapper<InitialConditionsBuildingBlock, List<DataTable>>
   {
   }

   public class InitialConditionsBuildingBlockToDataTableMapper : BaseBuildingBlockToDataTableMapper<InitialConditionsBuildingBlock>, IInitialConditionsBuildingBlockToDataTableMapper
   {
      private static readonly string _isPresent = AppConstants.Captions.IsPresent;
      private static readonly string _scaleDivisor = AppConstants.Captions.ScaleDivisor;
      private static readonly string _negativeValuesAllowed = AppConstants.Captions.NegativeValuesAllowed;

      protected override void AddColumnsToDataTable()
      {
         base.AddColumnsToDataTable();
         AddColumn(_dt, _isPresent, typeof(bool));
         AddColumn(_dt, _scaleDivisor, typeof(string));
         AddColumn(_dt, _negativeValuesAllowed, typeof(bool));
      }

      protected override void AddDataFromBuildingBlockToDataTable(InitialConditionsBuildingBlock buildingBlock)
      {
         base.AddDataFromBuildingBlockToDataTable(buildingBlock);
         var elements = GetElements(buildingBlock).ToList();

         for (int i = 0; i < elements.Count; i++)
         {
            var row = _dt.Rows[i];
            var element = elements[i];
            setSpecificColumns(row, element);
         }
      }

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
            { _path, ColumnIndexes.InitialConditionsRowIndexes.PATH },
            { _name, ColumnIndexes.InitialConditionsRowIndexes.MOLECULE },
            { _isPresent, ColumnIndexes.InitialConditionsRowIndexes.IS_PRESENT },
            { _value, ColumnIndexes.InitialConditionsRowIndexes.VALUE },
            { _unit, ColumnIndexes.InitialConditionsRowIndexes.UNIT },
            { _scaleDivisor, ColumnIndexes.InitialConditionsRowIndexes.SCALE_DIVISOR },
            { _negativeValuesAllowed, ColumnIndexes.InitialConditionsRowIndexes.NEGATIVE_VALUES_ALLOWED }
         };
      }

      protected override IEnumerable<PathAndValueEntity> GetElements(InitialConditionsBuildingBlock buildingBlock) =>
         buildingBlock.Select(x => x).Where(x => x.Value != null);

      private void setSpecificColumns(DataRow row, PathAndValueEntity element)
      {
         var initialCondition = (InitialCondition)element;
         row[_isPresent] = initialCondition.IsPresent;
         row[_scaleDivisor] = initialCondition.ScaleDivisor;
         row[_negativeValuesAllowed] = initialCondition.NegativeValuesAllowed;
      }
   }
}