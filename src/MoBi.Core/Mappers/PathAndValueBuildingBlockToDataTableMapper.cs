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
   public abstract class PathAndValueBuildingBlockToDataTableMapper<TBuildingBlock, TBuilder> : IMapper<TBuildingBlock, List<DataTable>>
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder>
      where TBuilder : PathAndValueEntity
   {
      protected DataTable _dt;
      protected string Path => AppConstants.Captions.ContainerPath;
      protected string Value => AppConstants.Captions.Value;
      protected string Unit => AppConstants.Captions.Unit;
      protected abstract string Name { get; }

      public List<DataTable> MapFrom(TBuildingBlock buildingBlock)
      {
         _dt = new DataTable();
         AddColumnsToDataTable();
         buildingBlock.Where(x => x.Value != null).Each(AddDataFromBuildingBlockToDataTable);
         SetColumnOrdinals();
         return new List<DataTable> { _dt };
      }

      protected virtual void AddColumnsToDataTable()
      {
         AddColumn(_dt, Path, typeof(string));
         AddColumn(_dt, Value, typeof(double));
         AddColumn(_dt, Name, typeof(string));
         AddColumn(_dt, Unit, typeof(string));
      }

      protected virtual void AddDataFromBuildingBlockToDataTable(TBuilder element)
      {
         var row = _dt.Rows.Add();
         SetSpecificColumns(row, element);
      }

      protected void AddColumn(DataTable dt, string columnName, Type dataType)
      {
         var column = new DataColumn(columnName, dataType);
         dt.Columns.Add(column);
      }

      protected virtual void SetSpecificColumns(DataRow row, TBuilder element)
      {
         row[Path] = element.ContainerPath;
         row[Name] = element.Name;
         row[Value] = element.ConvertToDisplayUnit(element.Value);
         row[Unit] = element.DisplayUnit;
      }

      protected abstract void SetColumnOrdinals();
   }
}