using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IInitialConditionsBuildingBlockToDataTableMapper : IMapper<InitialConditionsBuildingBlock, List<DataTable>>
   {
   }

   public class InitialConditionsBuildingBlockToDataTableMapper : PathAndValueBuildingBlockToDataTableMapper<InitialConditionsBuildingBlock, InitialCondition>, IInitialConditionsBuildingBlockToDataTableMapper
   {
      private string isPresent => AppConstants.Captions.IsPresent;
      private string scaleDivisor => AppConstants.Captions.ScaleDivisor;
      private string negativeValuesAllowed => AppConstants.Captions.NegativeValuesAllowed;

      protected override string Name => AppConstants.Captions.MoleculeName;

      protected override void AddColumnsToDataTable()
      {
         base.AddColumnsToDataTable();
         AddColumn(_dt, isPresent, typeof(bool));
         AddColumn(_dt, scaleDivisor, typeof(string));
         AddColumn(_dt, negativeValuesAllowed, typeof(bool));
      }

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
            { Path, ColumnIndexes.InitialConditionsRowIndexes.PATH },
            { Name, ColumnIndexes.InitialConditionsRowIndexes.MOLECULE },
            { isPresent, ColumnIndexes.InitialConditionsRowIndexes.IS_PRESENT },
            { Value, ColumnIndexes.InitialConditionsRowIndexes.VALUE },
            { Unit, ColumnIndexes.InitialConditionsRowIndexes.UNIT },
            { scaleDivisor, ColumnIndexes.InitialConditionsRowIndexes.SCALE_DIVISOR },
            { negativeValuesAllowed, ColumnIndexes.InitialConditionsRowIndexes.NEGATIVE_VALUES_ALLOWED }
         };
      }

      protected override void SetSpecificColumns(DataRow row, InitialCondition element)
      {
         base.SetSpecificColumns(row, element);
         row[isPresent] = element.IsPresent;
         row[scaleDivisor] = element.ScaleDivisor;
         row[negativeValuesAllowed] = element.NegativeValuesAllowed;
      }
   }
}