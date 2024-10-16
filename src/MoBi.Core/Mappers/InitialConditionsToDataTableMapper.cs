using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IInitialConditionsToDataTableMapper : IMapper<IEnumerable<InitialCondition>, List<DataTable>>
   {
   }

   public class InitialConditionsToDataTableMapper : PathAndValuesToDataTableMapper<InitialCondition>, IInitialConditionsToDataTableMapper
   {
      private string isPresent => AppConstants.Captions.IsPresent;
      private string scaleDivisor => AppConstants.Captions.ScaleDivisor;
      private string negativeValuesAllowed => AppConstants.Captions.NegativeValuesAllowed;

      protected override string Name => AppConstants.Captions.MoleculeName;
      protected override string TableName => AppConstants.Captions.InitialConditions;

      protected override void AddColumnsToDataTable()
      {
         base.AddColumnsToDataTable();
         AddColumn(_dt, isPresent, typeof(bool));
         AddColumn(_dt, scaleDivisor, typeof(string));
         AddColumn(_dt, negativeValuesAllowed, typeof(bool));
      }

      protected override Dictionary<string, int> GetColumnIndexes()
      {
         return new Dictionary<string, int>
         {
            { Path, ColumnIndexes.InitialConditions.PATH },
            { Name, ColumnIndexes.InitialConditions.MOLECULE },
            { isPresent, ColumnIndexes.InitialConditions.IS_PRESENT },
            { Value, ColumnIndexes.InitialConditions.VALUE },
            { Unit, ColumnIndexes.InitialConditions.UNIT },
            { scaleDivisor, ColumnIndexes.InitialConditions.SCALE_DIVISOR },
            { negativeValuesAllowed, ColumnIndexes.InitialConditions.NEGATIVE_VALUES_ALLOWED }
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