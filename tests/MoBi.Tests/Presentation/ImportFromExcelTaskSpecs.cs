using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Helpers;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_ImportFromExcelTask : ContextSpecification<ImportFromExcelTask>
   {
      protected override void Context()
      {
         sut = new ImportFromExcelTask();
      }
   }

   public class When_retrieving_non_empty_excel_sheets : concern_for_ImportFromExcelTask
   {
      private IEnumerable<string> _result;

      protected override void Because()
      {
         _result = sut.RetrieveExcelSheets(DomainHelperForSpecs.TestFileFullPath("psv.xlsx"), true);
      }

      [Observation]
      public void only_non_empty_sheets_should_be_listed()
      {
         _result.Count().ShouldBeEqualTo(4);
      }

   }

   public class When_retrieving_all_excel_sheets : concern_for_ImportFromExcelTask
   {
      private IEnumerable<string> _result;

      protected override void Because()
      {
         _result = sut.RetrieveExcelSheets(DomainHelperForSpecs.TestFileFullPath("psv.xlsx"), false);
      }

      [Observation]
      public void all_sheets_should_be_listed()
      {
         _result.Count().ShouldBeEqualTo(5);
      }

   }

   public class When_importing_excel_using_no_specific_sheet_name : concern_for_ImportFromExcelTask
   {
      private IEnumerable<DataTable> _results;

      protected override void Because()
      {
         _results = sut.GetAllDataTables(DomainHelperForSpecs.TestFileFullPath("psv.xlsx"), true);
      }

      [Observation]
      public void should_import_multiple_tables_When_asked_for_no_specific_sheet()
      {
         _results.Count().ShouldBeEqualTo(5);
      }
   }

   public class When_importing_excel_using_non_existing_sheet_name : concern_for_ImportFromExcelTask
   {
      private DataTable _results;

      protected override void Because()
      {
         _results = sut.GetDataTables(DomainHelperForSpecs.TestFileFullPath("psv.xlsx"), "not_a_table", true);
      }

      [Observation]
      public void should_only_import_one_table()
      {
         _results.ShouldBeNull();
      }
   }

   public class When_importing_excel_using_specific_sheet_name : concern_for_ImportFromExcelTask
   {
      private DataTable _results;

      protected override void Because()
      {
         _results = sut.GetDataTables(DomainHelperForSpecs.TestFileFullPath("psv.xlsx"), "Tabelle1", true);
      }

      [Observation]
      public void should_only_import_one_table()
      {
         _results.ShouldNotBeNull();
      }
   }
}
