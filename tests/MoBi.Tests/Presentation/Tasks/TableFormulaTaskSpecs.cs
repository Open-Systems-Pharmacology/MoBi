using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Maths.Interpolations;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_TableFormulaTask : ContextSpecification<TableFormulaTask>
   {
      private IDialogCreator _dialogCreator;
      protected IDataImporter _dataImporter;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _dataImporter = A.Fake<IDataImporter>();
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new TableFormulaTask(_dataImporter, _context, _dialogCreator);
      }
   }

   public abstract class When_changing_the_Value_for_ValuePoint_from_Display_value_and_changing_display_value_is_selectedBase : concern_for_TableFormulaTask
   {
      protected ValuePoint _valuePoint;
      protected Unit _xDisplayUnit;
      protected Unit _YDisplayUnit;
      protected TableFormula _tableFormula;
      protected double _newDisplayValue;
      protected IMoBiCommand _resultCommand;

      protected override void Context()
      {
         base.Context();

         _valuePoint = new ValuePoint(10, 20);
         _xDisplayUnit = A.Fake<Unit>();
         _YDisplayUnit = A.Fake<Unit>();
         _tableFormula = A.Fake<TableFormula>();
         _tableFormula.XDisplayUnit = _xDisplayUnit;
         _tableFormula.YDisplayUnit = _YDisplayUnit;
         _newDisplayValue = 12;
      }

      protected override void Because()
      {
         _resultCommand = sut.SetXValuePoint(_tableFormula, _valuePoint, _newDisplayValue, A.Fake<IBuildingBlock>());
      }
   }

   public class When_changing_the_Value_for_ValuePoint_from_Display_value_and_changing_base_value_is_selected : When_changing_the_Value_for_ValuePoint_from_Display_value_and_changing_display_value_is_selectedBase
   {
      protected override void Context()
      {
         base.Context();
         _valuePoint = new ValuePoint(2.1, 1.2);
         _tableFormula.Id = "TFID";
      }

      [Observation]
      public void should_convert_display_value_to_base_value()
      {
         A.CallTo(() => _tableFormula.XBaseValueFor(_newDisplayValue)).MustHaveHappened();
      }
   }

   public class When_importing_new_table_formula_value_points : concern_for_TableFormulaTask
   {
      private (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) _dataRepositories;
      private TableFormula _formula;
      private DataColumn _dataColumn;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         base.Context();
         _dataRepositories = (new List<DataRepository>
         {
            new DataRepository("data_repository_id")
         }, A.Fake<ImporterConfiguration>());

         _baseGrid = new BaseGrid("time", "time", DimensionFactoryForSpecs.TimeDimension);
         _dataColumn = new DataColumn("mass", "mass", DimensionFactoryForSpecs.MassDimension, _baseGrid);

         _baseGrid.InternalValues.AddRange(new[] { 0.0f, 1.0f });
         _dataColumn.ValuesAsArray = new[] { 10.0, 11.0 };
         _dataRepositories.DataRepositories.First().Add(_dataColumn);

         A.CallTo(() => _dataImporter.ImportDataSets(
            A<IReadOnlyList<MetaDataCategory>>._,
            A<IReadOnlyList<ColumnInfo>>._,
            A<DataImporterSettings>._,
            A<string>._)).Returns(_dataRepositories);

         A.CallTo(() => _context.Create<TableFormula>()).ReturnsLazily(() => new TableFormula(new LinearInterpolation()));
      }

      protected override void Because()
      {
         _formula = sut.ImportTableFormula();
      }

      [Observation]
      public void the_data_importer_is_used_to_import_a_data_repository()
      {
         A.CallTo(() => _dataImporter.ImportDataSets(
            A<IReadOnlyList<MetaDataCategory>>._,
            A<IReadOnlyList<ColumnInfo>>._,
            A<DataImporterSettings>._,
            A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_formula_should_have_corresponding_value_points()
      {
         _formula.AllPoints.Count.ShouldBeEqualTo(2);
         _formula.AllPoints[0].X.ShouldBeEqualTo(0.0);
         _formula.AllPoints[0].Y.ShouldBeEqualTo(10.0);
         _formula.AllPoints[1].X.ShouldBeEqualTo(1.0);
         _formula.AllPoints[1].Y.ShouldBeEqualTo(11.0);
      }
   }
}