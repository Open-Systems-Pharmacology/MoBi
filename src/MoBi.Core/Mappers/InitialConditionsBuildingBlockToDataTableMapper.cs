using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

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

      protected override IEnumerable<PathAndValueEntity> GetElements(InitialConditionsBuildingBlock buildingBlock) =>
         buildingBlock.Select(x => x).Where(x => x.Value != null);

      protected override void SetSpecificColumns(DataRow row, PathAndValueEntity element)
      {
         var initialCondition = (InitialCondition)element;
         row[_isPresent] = initialCondition.IsPresent;
         row[_scaleDivisor] = initialCondition.ScaleDivisor;
         row[_negativeValuesAllowed] = initialCondition.NegativeValuesAllowed;
      }

      protected override void AddSpecificColumns(DataTable dataTable)
      {
         AddColumn(dataTable, _isPresent, typeof(bool), ColumnIndexes.InitialConditionsRowIndexes.IS_PRESENT);
         AddColumn(dataTable, _scaleDivisor, typeof(string), ColumnIndexes.InitialConditionsRowIndexes.SCALE_DIVISOR);
         AddColumn(dataTable, _negativeValuesAllowed, typeof(bool), ColumnIndexes.InitialConditionsRowIndexes.NEGATIVE_VALUES_ALLOWED);
      }

      protected override int GetColumnIndexForPath() => ColumnIndexes.InitialConditionsRowIndexes.PATH;

      protected override int GetColumnIndexForValue() => ColumnIndexes.InitialConditionsRowIndexes.VALUE;
      
      protected override int GetColumnIndexForName() => ColumnIndexes.InitialConditionsRowIndexes.MOLECULE;
      protected override int GetColumnIndexForUnit() => ColumnIndexes.InitialConditionsRowIndexes.UNIT;
   }
}