using System.Data;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.TeXReporting.Data;
using MoBi.Core.Extensions;

namespace MoBi.Core
{
   public class when_using_data_view_extensions : StaticContextSpecification
   {
      private DataTable _table;
      private DataTable _childTable;
      private DataRelation _relation;

      protected override void Context()
      {
         base.Context();
         _table = new DataTable("Parent");
         _childTable = new DataTable("Child");

         var parentPK = _table.Columns.Add("PK1");
         _table.Columns.Add("Data");
         var childFK = _childTable.Columns.Add("PK1");
         _childTable.Columns.Add("Description");

         var ds = new DataSet("Model");
         ds.Tables.Add(_table);
         ds.Tables.Add(_childTable);

         _relation = new DataRelation("Relation", parentPK, childFK);
         ds.Relations.Add(_relation);
      }

      [Observation]
      public void should_create_table_for_view_preserving_child_relations()
      {
         var newTable = _table.DefaultView.CreateDataTableWithPreservedChildRelations();
         newTable.DataSet.ShouldNotBeNull();
         newTable.ChildRelations.Count.ShouldBeEqualTo(_table.ChildRelations.Count);
         foreach (DataRelation rel in newTable.ChildRelations)
         {
            foreach (DataColumn col in rel.ChildTable.Columns)
               _childTable.Columns.Contains(col.ColumnName).ShouldBeTrue();
            foreach (DataColumn col in rel.ParentTable.Columns)
               _table.Columns.Contains(col.ColumnName).ShouldBeTrue();
         }
      }

      [Observation]
      public void should_hide_columns()
      {
         foreach (DataColumn col in _table.Columns)
         {
            col.IsHidden().ShouldBeFalse();
            _table.DefaultView.CheckColumnVisibility(col.ColumnName);
            col.IsHidden().ShouldBeTrue();
         }
      }

      [Observation]
      public void should_not_hide_columns_if_values_exist()
      {
         var table = _table.Clone();

         table.BeginLoadData();
         var newRow = table.NewRow();
         foreach (DataColumn col in table.Columns)
            newRow[col] = "Data";
         table.Rows.Add(newRow);
         table.EndLoadData();

         foreach (DataColumn col in table.Columns)
         {
            col.IsHidden().ShouldBeFalse();
            table.DefaultView.CheckColumnVisibility(col.ColumnName);
            col.IsHidden().ShouldBeFalse();
         }
      }

      [Observation]
      public void should_hide_columns_if_only_equal_values_exist()
      {
         var table = _table.Clone();

         table.BeginLoadData();
         var newRow = table.NewRow();
         foreach (DataColumn col in table.Columns)
            newRow[col] = "Data";
         table.Rows.Add(newRow);
         table.EndLoadData();

         foreach (DataColumn col in table.Columns)
         {
            col.IsHidden().ShouldBeFalse();
            table.DefaultView.CheckColumnVisibility(col.ColumnName, "Data");
            col.IsHidden().ShouldBeTrue();
         }
      }

      [Observation]
      public void should_not_hide_columns_if_different_values_exist()
      {
         var table = _table.Clone();

         table.BeginLoadData();
         var newRow = table.NewRow();
         foreach (DataColumn col in table.Columns)
            newRow[col] = "Data";
         table.Rows.Add(newRow);

         newRow = table.NewRow();
         foreach (DataColumn col in table.Columns)
            newRow[col] = "Data2";
         table.Rows.Add(newRow);
         table.EndLoadData();

         foreach (DataColumn col in table.Columns)
         {
            col.IsHidden().ShouldBeFalse();
            table.DefaultView.CheckColumnVisibility(col.ColumnName, "Data");
            col.IsHidden().ShouldBeFalse();
         }
      }
   }
}
