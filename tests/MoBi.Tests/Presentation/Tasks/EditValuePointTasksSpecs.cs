using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditValuePointTasksSpecs : ContextSpecification<ITableFormulaTask>
   {
      private IDialogCreator _dialogCreator;
      private IDataImporter _dataImporter;
      private IMoBiContext _context;

      protected override void Context()
      {
         _dataImporter = A.Fake<IDataImporter>();
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new TableFormulaTask(_dataImporter, _context, _dialogCreator);
      }
   }

   public abstract class When_changing_the_Value_for_ValuePoint_from_Display_value_and_changing_display_value_is_selectedBase : concern_for_EditValuePointTasksSpecs
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
}