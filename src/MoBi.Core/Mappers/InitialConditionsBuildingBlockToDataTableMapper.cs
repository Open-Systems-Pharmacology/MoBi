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
      private static readonly string _moleculeName = AppConstants.Captions.MoleculeName;
      private static readonly string _isPresent = AppConstants.Captions.IsPresent;
      private static readonly string _unit = AppConstants.Captions.Unit;
      private static readonly string _scaleDivisor = AppConstants.Captions.ScaleDivisor;
      private static readonly string _negativeValuesAllowed = AppConstants.Captions.NegativeValuesAllowed;

      protected override IEnumerable<PathAndValueEntity> GetElements(InitialConditionsBuildingBlock buildingBlock) =>
         buildingBlock.Select(x => x).Where(x => x.Value != null);

      protected override DataTable GenerateEmptyDataTable()
      {
         var dt = new DataTable();
         dt.AddColumn(_path);
         dt.AddColumn<string>(_moleculeName);
         dt.AddColumn<bool>(_isPresent);
         dt.AddColumn<double>(_value);
         dt.AddColumn(_scaleDivisor);
         dt.AddColumn(_unit);
         dt.AddColumn<bool>(_negativeValuesAllowed);
         dt.TableName = AppConstants.Captions.InitialConditions;
         return dt;
      }

      protected override void SetSpecificColumns(DataRow row, PathAndValueEntity element)
      {
         var initialCondition = (InitialCondition)element;
         row[_moleculeName] = initialCondition.Name;
         row[_isPresent] = initialCondition.IsPresent;
         row[_scaleDivisor] = initialCondition.ScaleDivisor;
         row[_negativeValuesAllowed] = initialCondition.NegativeValuesAllowed;
      }
   }
}