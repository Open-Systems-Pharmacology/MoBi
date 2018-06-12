using System;
using System.Data;
using System.Globalization;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Mapper
{
   public abstract class ContextSpecificationForImportStartValues<T> : ContextSpecification<T>
   {
      protected IMoBiDimensionFactory _dimensionFactory;
      private IDimension _concentrationDimension;
      private IDimension _volumeDimension;
      private IDimension _timeDimension;

      protected ObjectPath ContainerPathFromDataTableRow(DataTable tables, int row)
      {
         return new ObjectPath(tables.Rows[row][0].ToString().Split(ObjectPath.PATH_DELIMITER.ToCharArray()));
      }

      protected void CreateDimensionFactory()
      {
         _concentrationDimension = new Dimension();
         _concentrationDimension.AddUnit("mol", 1.0, 0);

         _volumeDimension = new Dimension();
         _volumeDimension.AddUnit("ml", 0.001, 0);

         _timeDimension = new Dimension();
         _timeDimension.AddUnit("s", 1 / 60.0, 0);
         _dimensionFactory = A.Fake<IMoBiDimensionFactory>();

         A.CallTo(() => _dimensionFactory.DimensionForUnit("mol")).Returns(_concentrationDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("ml")).Returns(_volumeDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("s")).Returns(_timeDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("r/h")).Returns(null);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("")).Throws<Exception>();

      }
   }

   public abstract class concern_for_DataTableToImportQuantityDTOMapperForMolecules : ContextSpecificationForImportStartValues<DataTableToImportQuantityDTOMapperForMolecules>
   {
      protected IDimension _concentrationDimension;
      protected IDimension _amountDimension;
      private IDimension _timeDimension;
      private IMoleculeStartValuesCreator _msvCreator;

      protected IMoleculeStartValuesBuildingBlock _startValuesBuildingBlock;
      private IReactionDimensionRetriever _reactionDimensionRetriever;

      protected override void Context()
      {
         _msvCreator = A.Fake<IMoleculeStartValuesCreator>();

         A.CallTo(() => _msvCreator.CreateMoleculeStartValue(A<IObjectPath>.Ignored, A<string>.Ignored, A<IDimension>.Ignored, A<Unit>._, A<ValueOrigin>._))
            .ReturnsLazily((IObjectPath path, string moleculeName, IDimension dimension) => new MoleculeStartValue {ContainerPath = path, Name = moleculeName, Dimension = dimension});

         _concentrationDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.MOLAR_CONCENTRATION, "mol/l");

         _amountDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.AMOUNT, "mol");
         _amountDimension.AddUnit("mmol", 0.001, 0);

         _timeDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.TIME, "s");
         _timeDimension.Unit("s").Factor = 1.0 / 60;

         _dimensionFactory = A.Fake<IMoBiDimensionFactory>();
         _startValuesBuildingBlock = A.Fake<IMoleculeStartValuesBuildingBlock>();

         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();

         A.CallTo(() => _dimensionFactory.DimensionForUnit("mol")).Returns(_amountDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("mmol")).Returns(_amountDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("mol/l")).Returns(_concentrationDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("s")).Returns(_timeDimension);
         A.CallTo(() => _dimensionFactory.DimensionForUnit("")).Throws<Exception>();

         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.AMOUNT)).Returns(_amountDimension);
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION)).Returns(_concentrationDimension);

         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);

         sut = new DataTableToImportQuantityDTOMapperForMolecules(_dimensionFactory, _reactionDimensionRetriever);
      }
   }

   public class When_converting_and_validating_rows_without_container_path : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][0] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_not_allow_any_mapping()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }


   public class When_converting_and_validating_rows_resulting_in_duplicated_entriues: concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][0] = _tables.Rows[1][0];
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_not_allow_any_mapping()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }

   public class When_converting_and_validating_rows_without_name : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][1] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_not_allow_any_mapping()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }

   public class When_converting_and_validating_rows_without_scale_divisor : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][5] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_indicate_that_scale_divisor_is_not_specified()
      {
         _result.QuantitDTOs[0].ScaleDivisor.ShouldBeEqualTo(double.NaN);
      }
   }

   public class When_converting_and_validating_rows_without_negative_value_allowed : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][6] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_indicate_that_scale_divisor_is_not_specified()
      {
         _result.QuantitDTOs[0].NegativeValuesAllowed.ShouldBeFalse();
      }
   }

   public class When_converting_and_validating_without_value : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][3] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_not_allow_mapping_When_no_quantity_is_specified()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }

   public class When_converting_and_validating_for_full_insert : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;
      private DataTable _tables;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _tables = new MsvDataTableProvider().ImportTables();
         _tables.Rows[0][3] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_fail_to_validate_When_missing_a_quantity()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }

   public class When_converting_and_validating_for_update_existing : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private DataTable _tables;
      private QuantityImporterDTO _result;

      protected override void Context()
      {
         base.Context();

         // We'll need a real building block for this test so that it will return null when asked for a start value from a path
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         
         _tables = new MsvDataTableProvider().ImportTables();

         _startValuesBuildingBlock.Add(new MoleculeStartValue{Name="Drug", ContainerPath = ContainerPathFromDataTableRow(_tables, 0), StartValue = 9.0});
         _tables.Rows[0][3] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_tables, _startValuesBuildingBlock);
      }

      [Observation]
      public void should_allow_import_of_new_values_without_quantity_When_updating_only()
      {
         string.Equals(_result.QuantitDTOs[0].ContainerPath.ToString(), ContainerPathFromDataTableRow(_tables, 0).ToString()).ShouldBeTrue();
      }

      [Observation]
      public void the_value_for_quantity_is_not_specified()
      {
         _result.QuantitDTOs[0].IsQuantitySpecified.ShouldBeFalse();
      }
   }


   public class When_converting_msv_data_table_to_import_quantity_dto_that_match_specs : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;

      protected override void Because()
      {
         _result = sut.MapFrom(new MsvDataTableProvider().ImportTables(), _startValuesBuildingBlock);
      }

      [Observation]
      public void should_convert_values_to_base_units_When_necessary()
      {
         _result.QuantitDTOs[1].QuantityInBaseUnit.ShouldBeEqualTo(2 / 1000.0);
      }

      [Observation]
      public void should_return_correct_number_of_quantity()
      {
         _result.QuantitDTOs.Count.ShouldBeEqualTo(6);
      }

      [Observation]
      public void is_present_is_converted_When_true()
      {
         _result.QuantitDTOs[0].IsPresent.ShouldBeTrue();
         _result.QuantitDTOs[2].IsPresent.ShouldBeTrue();
         _result.QuantitDTOs[4].IsPresent.ShouldBeTrue();
      }

      [Observation]
      public void is_present_is_converted_When_false()
      {
         _result.QuantitDTOs[1].IsPresent.ShouldBeFalse();
         _result.QuantitDTOs[3].IsPresent.ShouldBeFalse();
         _result.QuantitDTOs[5].IsPresent.ShouldBeFalse();
      }

      [Observation]
      public void molecule_name_is_converted()
      {
         _result.QuantitDTOs.Each(msv => msv.Name.ShouldBeEqualTo("Drug"));
      }

      [Observation]
      public void should_determine_appropriate_path_arguments()
      {
         foreach (var t in _result.QuantitDTOs)
         {
            t.ContainerPath.Count.ShouldBeEqualTo(5);
         }
      }
   }

   public class When_converting_table_to_import_quantity_dto_with_no_display_unit_and_specifying_new_quantity : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private DataTable _importTables;
      private QuantityImporterDTO _result;

      protected override void Context()
      {
         base.Context();
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _importTables = new MsvDataTableProvider().ImportTables();
         _importTables.Rows[0][4] = string.Empty;
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_importTables, _startValuesBuildingBlock);
      }

      [Observation]
      public void must_not_allow_import_When_the_quantity()
      {
         _result.QuantitDTOs.ShouldBeEmpty();
      }
   }

   public class When_converting_table_to_import_quantity_dto_with_no_display_unit_and_not_specifying_quantity : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private DataTable _importTables;
      private QuantityImporterDTO _result;

      protected override void Context()
      {
         base.Context();
         _startValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _importTables = new MsvDataTableProvider().ImportTables();
         _importTables.Rows[0][4] = string.Empty;
         _importTables.Rows[0][3] = string.Empty;
         
         _startValuesBuildingBlock.Add(new MoleculeStartValue{ContainerPath = ContainerPathFromDataTableRow(_importTables, 0), Name = "Drug", StartValue = 9.0});
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_importTables, _startValuesBuildingBlock);
      }

      [Observation]
      public void must_not_allow_import_When_the_quantity()
      {
         _result.QuantitDTOs[0].IsQuantitySpecified.ShouldBeFalse();
      }
   }

   public class When_converting_table_to_import_quantity_dto_with_mismatching_dimension_modes : concern_for_DataTableToImportQuantityDTOMapperForMolecules
   {
      private QuantityImporterDTO _result;

      protected override void Because()
      {
         _result = sut.MapFrom(new ConcentrationDataTableProvider().ImportTables(), _startValuesBuildingBlock);
      }

      [Observation]
      public void log_message_should_warn_about_mismatching_dimension()
      {
         _result.Log[1].StartsWith(AppConstants.Exceptions.ImportedDimensionNotRecognized(_amountDimension.ToString(), _amountDimension.GetUnitNames())).ShouldBeTrue();
      }

      [Observation]
      public void should_result_in_no_imported_quantities()
      {
         _result.Count.ShouldBeEqualTo(0);
      }
   }

   public class ConcentrationDataTableProvider : MsvDataTableProvider
   {
      protected override string GetUnits(int i)
      {
         return "mol/l";
      }
   }

   public class MsvDataTableProvider
   {
      private static readonly string[] _units = { "mol", "mmol", "mol", "mol", "mol", "mol" };
      private readonly object[] _isPresentValues = {"true", "false", 1, 0, "TRUE", "FALSE"};
      private readonly object[] _negativeValuesAllowed = {"true", "false", 1, 0, "TRUE", "FALSE"};

      protected virtual string GetUnits(int i)
      {
         return _units[i];
      }

      protected virtual string GetQuantity(int tableIndex, int i)
      {
         return (tableIndex * i).ToString(CultureInfo.InvariantCulture);
      }

      protected virtual string GetScaleFactor(int tableIndex, int i)
      {
         return "1.0";
      }

      public virtual DataTable ImportTables()
      {
         return makeATable();
      }

      private DataTable makeATable()
      {
         var dt = new DataTable("SpecsTable");

         dt.Columns.Add("PathColumn");
         dt.Columns.Add("MoleculeName");
         dt.Columns.Add("IsPresent");
         dt.Columns.Add("ValueColumn");
         dt.Columns.Add("UnitsColumn");
         dt.Columns.Add("ScaleFactorColumn");
         dt.Columns.Add("NegativeValuesAllowedColumn");

         for (var i = 0; i < 6; i++)
         {
            var row = dt.NewRow();
            row[0] = "This" + ObjectPath.PATH_DELIMITER + "Is" + ObjectPath.PATH_DELIMITER + "The" + ObjectPath.PATH_DELIMITER + "Path" + ObjectPath.PATH_DELIMITER + i;
            row[1] = "Drug";
            row[2] = _isPresentValues[i];
            row[3] = GetQuantity(2, i);
            row[4] = GetUnits(i);
            row[5] = GetScaleFactor(2, i);
            row[6] = _negativeValuesAllowed[i];
            dt.Rows.Add(row);
         }

         return dt;
      }
   }
}