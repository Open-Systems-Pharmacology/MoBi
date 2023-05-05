using System.Data;
using System.Globalization;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mapper
{
   public abstract class concern_for_DataTableToImportQuantityDTOMapperForParameters : ContextSpecificationForImportStartValues<DataTableToImportQuantityDTOMapperForParameters>
   {
      protected ParameterValuesBuildingBlock _startValuesBuildingBlock;

      protected override void Context()
      {
         CreateDimensionFactory();

         sut = new DataTableToImportQuantityDTOMapperForParameters(_dimensionFactory);
         _startValuesBuildingBlock = new ParameterValuesBuildingBlock();
      }
   }

   public class When_converting_and_validating_for_updates_where_the_dimensions_dont_match : concern_for_DataTableToImportQuantityDTOMapperForParameters
   {
      private DataTable _tables;
      private QuantityImporterDTO _result;

      protected override void Context()
      {
         base.Context();
         _tables = new DataTableProvider().ImportTables();
         _startValuesBuildingBlock.Add(new ParameterValue
         {
            ContainerPath = ContainerPathFromDataTableRow(_tables, 0),
            Name = "ParameterName",
            Dimension = DomainHelperForSpecs.AmountPerTimeDimension,
            DisplayUnit = DomainHelperForSpecs.AmountPerTimeDimension.BaseUnit
         });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void a_warning_should_have_been_produced()
      {
         _result.QuantityDTOs[0].HasWarning().ShouldBeTrue();
      }
   }

   public class When_converting_and_validating_for_updates_where_the_dimensions_dont_match_but_share_a_common_unit : concern_for_DataTableToImportQuantityDTOMapperForParameters
   {
      private DataTable _tables;
      private QuantityImporterDTO _result;
      private Dimension _similarDimension;

      protected override void Context()
      {
         base.Context();
         _similarDimension = new Dimension(new BaseDimensionRepresentation(), "XXX", "1/mol");
         _similarDimension.AddUnit("mol", 10, 0);

         _tables = new DataTableProvider(numberOfRowsToImport: 1).ImportTables();
         _startValuesBuildingBlock.Add(new ParameterValue
         {
            ContainerPath = ContainerPathFromDataTableRow(_tables, 0),
            Name = "ParameterName",
            Dimension = _similarDimension,
            DisplayUnit = _similarDimension.DefaultUnit
         });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_have_changed_the_imported_dimension()
      {
         _result.QuantityDTOs[0].Dimension.ShouldBeEqualTo(_similarDimension);
      }

      [Observation]
      public void should_have_changed_the_imported_value_to_the_new_base_value()
      {
         _result.QuantityDTOs[0].QuantityInBaseUnit.ShouldBeEqualTo(10);
      }
   }

   public class When_converting_and_validating_for_full_insert_parameters : concern_for_DataTableToImportQuantityDTOMapperForParameters
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new ParameterValuesBuildingBlock();
         _tables = new DataTableProvider().ImportTables();
         _tables.Rows[0][2] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_fail_to_validate_When_missing_a_quantity()
      {
         _result.QuantityDTOs.ShouldBeEmpty();
      }
   }

   public class When_converting_and_validating_for_full_insert_parameters_with_dimension_column : concern_for_DataTableToImportQuantityDTOMapperForParameters
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new ParameterValuesBuildingBlock();
         _tables = new WithDimensionColumnDataTable().ImportTables();
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_fail_to_validate_When_missing_a_quantity()
      {
         _result.QuantityDTOs[0].Dimension.Name.ShouldBeEqualTo("Becquerel");
         _result.QuantityDTOs[1].Dimension.Name.ShouldBeEqualTo("Inversed time");
      }
   }

   public abstract class WhenConvertingDataTableToImportQuantityDTOThatDontConform : concern_for_DataTableToImportQuantityDTOMapperForParameters
   {
      protected QuantityImporterDTO _result;

      [Observation]
      public void no_results_should_be_mapped()
      {
         _result.QuantityDTOs.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_converting_data_table_to_import_quantity_dto_with_framework_exception : WhenConvertingDataTableToImportQuantityDTOThatDontConform
   {
      protected override void Because()
      {
         _result = sut.MapFrom(new TableNotImplementedExceptionIsThrown().ImportTables(), A.Fake<IStartValuesBuildingBlock<ParameterValue>>());
      }

      [Observation]
      public void error_message_contained_in_log()
      {
         _result.Log.Any(x => x.Contains(AppConstants.Exceptions.FrameworkExceptionOccurred)).ShouldBeTrue();
      }
   }

   public class When_converting_data_table_to_import_quantity_dto_with_invalid_values : WhenConvertingDataTableToImportQuantityDTOThatDontConform
   {
      protected override void Because()
      {
         _result = sut.MapFrom(new InvalidImportDataTable().ImportTables(), A.Fake<IStartValuesBuildingBlock<ParameterValue>>());
      }

      [Observation]
      public void error_message_contained_in_log()
      {
         _result.Log.Any(x => x.Contains(AppConstants.Exceptions.ColumnNMustBeNumeric("", 3))).ShouldBeTrue();
      }
   }

   public class When_converting_data_table_to_import_quantity_dto_with_invalid_units : WhenConvertingDataTableToImportQuantityDTOThatDontConform
   {
      protected override void Because()
      {
         _result = sut.MapFrom(new InvalidUnitsDataTable().ImportTables(), A.Fake<IStartValuesBuildingBlock<ParameterValue>>());
      }

      [Observation]
      public void error_message_contained_in_log()
      {
         _result.Log.Any(x => x.Contains(AppConstants.Exceptions.CouldNotFindDimensionFromUnits(""))).ShouldBeTrue();
      }
   }

   public class When_converting_data_table_to_import_quantity_dto_with_fewer_than_three_columns : WhenConvertingDataTableToImportQuantityDTOThatDontConform
   {
      protected override void Because()
      {
         _result = sut.MapFrom(new IncorrectFormatDataTableProvider().ImportTables(), A.Fake<IStartValuesBuildingBlock<ParameterValue>>());
      }

      [Observation]
      public void error_message_contained_in_log()
      {
         _result.Log.Any(x => x.Contains(AppConstants.Exceptions.TableShouldBeNColumns(4))).ShouldBeTrue();
      }
   }

   public class When_converting_data_table_to_import_quantity_dto_that_match_specs : concern_for_DataTableToImportQuantityDTOMapperForParameters
   {
      private QuantityImporterDTO _result;

      protected override void Because()
      {
         _result = sut.MapFrom(new DataTableProvider().ImportTables(), _startValuesBuildingBlock);
      }

      [Observation]
      public void should_convert_values_to_base_units_When_necessary()
      {
         _result.QuantityDTOs[1].QuantityInBaseUnit.ShouldBeEqualTo(2 / 1000.0);
         _result.QuantityDTOs[2].QuantityInBaseUnit.ShouldBeEqualTo(3 / 60.0);
      }

      [Observation]
      public void should_return_correct_number_of_quantities()
      {
         _result.QuantityDTOs.Count.ShouldBeEqualTo(3);
      }

      [Observation]
      public void should_determine_appropriate_path_arguments()
      {
         foreach (var t in _result.QuantityDTOs)
         {
            t.ContainerPath.Count.ShouldBeEqualTo(5);
         }
      }
   }

   internal class TableNotImplementedExceptionIsThrown : DataTableProvider
   {
      protected override string GetUnits(int i)
      {
         return "";
      }
   }

   internal class InvalidImportDataTable : DataTableProvider
   {
      protected override string GetQuantities(int tableIndex, int i)
      {
         return "fourpointfour";
      }
   }

   internal class InvalidUnitsDataTable : DataTableProvider
   {
      protected override string GetUnits(int i)
      {
         return "r/h";
      }
   }

   internal class WithDimensionColumnDataTable : DataTableProvider
   {
      public override DataTable ImportTables()
      {
         var dt = new DataTable("SpecsTable");

         dt.Columns.Add("PathColumn");
         dt.Columns.Add("NameColumn");
         dt.Columns.Add("ValueColumn");
         dt.Columns.Add("UnitsColumn");
         dt.Columns.Add("DimensionColumn");

         for (var i = 0; i < 2; i++)
         {
            var row = dt.NewRow();
            row[0] = "This" + ObjectPath.PATH_DELIMITER + "Is" + ObjectPath.PATH_DELIMITER + "The" + ObjectPath.PATH_DELIMITER + "Path" +
                     ObjectPath.PATH_DELIMITER + i;
            row[1] = "ParameterName";
            row[2] = GetQuantities(1, i);
            row[3] = "1/min";
            row[4] = i == 0 ? "Becquerel" : "Inversed time";
            dt.Rows.Add(row);
         }

         return dt;
      }
   }

   internal class IncorrectFormatDataTableProvider : DataTableProvider
   {
      public override DataTable ImportTables()
      {
         var dt = new DataTable("SpecsTable");

         dt.Columns.Add("PathColumn");
         dt.Columns.Add("ValueColumn");

         for (var i = 0; i < 3; i++)
         {
            var row = dt.NewRow();
            row[0] = "This" + ObjectPath.PATH_DELIMITER + "Is" + ObjectPath.PATH_DELIMITER + "The" + ObjectPath.PATH_DELIMITER + "Path";
            row[1] = 0;
            dt.Rows.Add(row);
         }

         return dt;
      }
   }

   internal class DataTableProvider
   {
      private readonly int _numberOfRowsToImport;

      public DataTableProvider(int numberOfRowsToImport = 3)
      {
         _numberOfRowsToImport = numberOfRowsToImport;
      }

      private static readonly string[] _units = {"mol", "ml", "s"};

      protected virtual string GetUnits(int i)
      {
         return _units[i];
      }

      protected virtual string GetQuantities(int tableIndex, int i)
      {
         return (tableIndex * (i + 1)).ToString(CultureInfo.InvariantCulture);
      }

      public virtual DataTable ImportTables()
      {
         return makeATable();
      }

      private DataTable makeATable()
      {
         var dt = new DataTable("SpecsTable");

         dt.Columns.Add("PathColumn");
         dt.Columns.Add("NameColumn");
         dt.Columns.Add("ValueColumn");
         dt.Columns.Add("UnitsColumn");

         for (var i = 0; i < _numberOfRowsToImport; i++)
         {
            var row = dt.NewRow();
            row[0] = "This" + ObjectPath.PATH_DELIMITER + "Is" + ObjectPath.PATH_DELIMITER + "The" + ObjectPath.PATH_DELIMITER + "Path" +
                     ObjectPath.PATH_DELIMITER + i;
            row[1] = "ParameterName";
            row[2] = GetQuantities(1, i);
            row[3] = GetUnits(i);
            dt.Rows.Add(row);
         }

         return dt;
      }
   }
}