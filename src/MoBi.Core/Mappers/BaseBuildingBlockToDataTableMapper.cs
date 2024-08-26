using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public abstract class BaseBuildingBlockToDataTableMapper<TBuildingBlock> : IMapper<TBuildingBlock, List<DataTable>> where TBuildingBlock : IBuildingBlock
   {
      protected static readonly string _path = AppConstants.Captions.ContainerPath;
      protected static readonly string _value = AppConstants.Captions.Value;
      protected static readonly string _name = AppConstants.Captions.Name;
      protected static readonly string _unit = AppConstants.Captions.Unit;
      protected DataTable _dt;

      public List<DataTable> MapFrom(TBuildingBlock buildingBlock)
      {
         _dt = new DataTable();
         AddColumnsToDataTable();
         AddDataFromBuildingBlockToDataTable(buildingBlock);
         SetColumnOrdinals();
         return new List<DataTable> { _dt };
      }

      protected virtual void AddColumnsToDataTable()
      {
         AddColumn(_dt, _path, typeof(string));
         AddColumn(_dt, _value, typeof(double));
         AddColumn(_dt, _name, typeof(string));
         AddColumn(_dt, _unit, typeof(string));
      }
      
      protected virtual void AddDataFromBuildingBlockToDataTable(TBuildingBlock buildingBlock)
      {
         var elements = GetElements(buildingBlock);

         foreach (var element in elements)
         {
            var row = _dt.Rows.Add();
            row[_path] = element.ContainerPath;
            row[_name] = element.Name;
            row[_value] = element.ConvertToDisplayUnit(element.Value);
            row[_unit] = element.DisplayUnit;
         }
      }

      protected void AddColumn(DataTable dt, string columnName, Type dataType)
      {
         var column = new DataColumn(columnName, dataType);
         dt.Columns.Add(column);
      }

      protected abstract Dictionary<string, int> GetColumnIndexes();
      protected abstract void SetColumnOrdinals();
      protected abstract IEnumerable<PathAndValueEntity> GetElements(TBuildingBlock buildingBlock);
      
   }
}