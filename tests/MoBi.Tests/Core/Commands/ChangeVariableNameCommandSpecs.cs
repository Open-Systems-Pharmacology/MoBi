using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeVariableNameCommandSpecs : ContextSpecification<ChangeVariableNameCommand>
   {
      protected SumFormula _sumFormula;
      protected string _newName;
      protected string _oldName;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _newName = "NEW";
         _oldName = "OLD";
         _sumFormula = new SumFormula().WithId("SUMFORMULA").WithName(_oldName);
         _sumFormula.Variable = _oldName;

         sut = new ChangeVariableNameCommand(_sumFormula, _newName, A.Fake<IBuildingBlock>());

         _context= A.Fake<IMoBiContext>();
      }
   }

   public class When_executing_the_ChangeVariableNameCommand : concern_for_ChangeVariableNameCommandSpecs
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_sum_formulas_variable_property_to_new_variable_name()
      {
         _sumFormula.Variable.ShouldBeEqualTo(_newName);
      }
   }

   public class When_restoring_execution_Data : concern_for_ChangeVariableNameCommandSpecs
   {
      protected override void Because()
      {
         sut.RestoreExecutionData(_context);
      }

      [Observation]
      public void should_ask_context_for_formula()
      {
         A.CallTo(() => _context.Get<SumFormula>(_sumFormula.Id)).MustHaveHappened();
      }
   }

   public class When_executing_the_inverse_command : concern_for_ChangeVariableNameCommandSpecs
   {
      private ChangeVariableNameCommand _inverseCommand;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<SumFormula>(_sumFormula.Id)).Returns(_sumFormula);
      }

      protected override void Because()
      {
         _inverseCommand = sut.ExecuteAndInvokeInverse(_context) as ChangeVariableNameCommand;
      }

      [Observation]
      public void the_inverse_command_should_be_a_change_variable_command()
      {
         _inverseCommand.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_reset_the_formula_to_its_original_value()
      {
         _sumFormula.Variable.ShouldBeEqualTo(_oldName);
      }
   }
}