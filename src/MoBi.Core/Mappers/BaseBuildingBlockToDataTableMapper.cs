using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public abstract class BaseBuildingBlockToDataTableMapper<TBuildingBlock> : IMapper<TBuildingBlock, List<DataTable>> where TBuildingBlock : IBuildingBlock
   {
      protected static readonly string _path = AppConstants.Captions.ContainerPath;
      protected static readonly string _value = AppConstants.Captions.Value;
      private static readonly string _name = AppConstants.Captions.Name;
      private static readonly string _unit = AppConstants.Captions.Unit;

      private readonly Dictionary<string, int> _columnOrdinals = new Dictionary<string, int>();

      public List<DataTable> MapFrom(TBuildingBlock buildingBlock) =>
         buildingBlockToDataTable(buildingBlock);

      private List<DataTable> buildingBlockToDataTable(TBuildingBlock buildingBlock)
      {
         var dt = GenerateEmptyDataTable();
         var elements = GetElements(buildingBlock);

         foreach (var element in elements)
         {
            var row = dt.Rows.Add();
            row[_path] = element.ContainerPath;
            row[_name] = element.Name;
            row[_value] = element.ConvertToDisplayUnit(element.Value);
            row[_unit] = element.DisplayUnit;
            SetSpecificColumns(row, element);
         }

         SetColumnOrdinals(dt);

         return new List<DataTable> { dt };
      }

      protected DataTable GenerateEmptyDataTable()
      {
         var dt = new DataTable();
         AddColumn(dt, _path, typeof(string), GetColumnIndexForPath());
         AddColumn(dt, _value, typeof(double), GetColumnIndexForValue());
         AddColumn(dt, _name, typeof(string), GetColumnIndexForName());
         AddColumn(dt, _unit, typeof(string), GetColumnIndexForUnit());
         AddSpecificColumns(dt);
         return dt;
      }

      protected void AddColumn(DataTable dt, string columnName, Type dataType, int index)
      {
         var column = new DataColumn(columnName, dataType);
         dt.Columns.Add(column);
         _columnOrdinals[columnName] = index;
      }

      private void SetColumnOrdinals(DataTable dt)
      {
         foreach (var columnOrdinal in _columnOrdinals)
         {
            var columnName = columnOrdinal.Key;
            var index = columnOrdinal.Value;

            if (index >= 0 && index < dt.Columns.Count)
            {
               dt.Columns[columnName].SetOrdinal(index);
            }
            else
            {
               throw new ArgumentOutOfRangeException(nameof(index), $"The index {index} is out of range for the column '{columnName}'.");
            }
         }
      }


      protected abstract IEnumerable<PathAndValueEntity> GetElements(TBuildingBlock buildingBlock);
      protected abstract void AddSpecificColumns(DataTable dataTable);
      protected abstract void SetSpecificColumns(DataRow row, PathAndValueEntity element);
      protected abstract int GetColumnIndexForPath();
      protected abstract int GetColumnIndexForValue();
      protected abstract int GetColumnIndexForName();
      protected abstract int GetColumnIndexForUnit();
   }
}