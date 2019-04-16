using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
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

      protected override void Context()
      {
         _newName = "NEW";
         _oldName = "OLD";
         _sumFormula = A.Fake<SumFormula>().WithId("SUMFORMULA").WithName(_oldName);
         sut = new ChangeVariableNameCommand(_sumFormula, _newName, A.Fake<IBuildingBlock>());
      }
   }

   public class When_executing_the_ChangeVariableNameCommand : concern_for_ChangeVariableNameCommandSpecs
   {
      protected override void Because()
      {
         sut.Execute(A.Fake<IMoBiContext>());
      }

      [Observation]
      public void should_set_sum_formulas_variable_property_to_new_variable_name()
      {
         _sumFormula.Variable.ShouldBeEqualTo(_newName);
      }
   }

   public class When_restoring_execution_Data : concern_for_ChangeVariableNameCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

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

   public class When_asking_for_inverse_Command : concern_for_ChangeVariableNameCommandSpecs
   {
      private ChangeVariableNameCommand _inverseCommand;

      protected override void Because()
      {
         _inverseCommand = sut.InverseCommand(A.Fake<IMoBiContext>()).DowncastTo<ChangeVariableNameCommand>();
      }

      [Observation]
      public void The_InverseCommand_should_modifie_the_same_formula()
      {
         _inverseCommand.ChangedFormulaId.ShouldBeEqualTo(_sumFormula.Id);
      }

      [Observation]
      public void The_InverseCommands_new_varaible_name_property_should_be_old_name()
      {
         _inverseCommand.NewVariableName.ShouldBeEqualTo(_oldName);
      }

      [Observation]
      public void The_InverseCommands_old_varaible_name_property_should_be_new_name()
      {
         _inverseCommand.OldVariableName.ShouldBeEqualTo(_newName);
      }
   }
}