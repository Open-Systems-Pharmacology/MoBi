using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditUseDerivedValuesCommandSpecs : ContextSpecification<EditUseDerivedValuesCommand>
   {
      protected TableFormula _tableFormula;
      protected bool _newValue;
      protected bool _oldValue;

      protected override void Context()
      {
         _tableFormula = A.Fake<TableFormula>();
         _newValue = true;
         _oldValue = _tableFormula.UseDerivedValues;
         sut = new EditUseDerivedValuesCommand(_tableFormula,_newValue,_oldValue, A.Fake<IBuildingBlock>());
      }
   }

   public class When_executing_Edit_Use_Derived_Values_Command : concern_for_EditUseDerivedValuesCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }
      [Observation]
      public void should_set_Use_Derived_Values_property_to_new_value()
      {
         _tableFormula.UseDerivedValues.ShouldBeEqualTo(_newValue);
      }
   }
}	