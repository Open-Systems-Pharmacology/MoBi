using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterRHSFormulaInBuildingBlockCommand : ContextSpecification<EditParameterRHSFormulaInBuildingBlockCommand>
   {
      protected IBuildingBlock _buildingBlock;
      protected IParameter _parameter;
      protected IFormula _newFormula;
      protected IMoBiContext _context;
      protected IFormula _oldFormula;

      protected override void Context()
      {
         _oldFormula = new ExplicitFormula().WithName("oldFormula").WithId("oldId");
         _parameter = new Parameter().WithRHS(_oldFormula);
         _newFormula = new ExplicitFormula().WithName("newFormula").WithId("newId");
         _buildingBlock = A.Fake<IBuildingBlock>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         A.CallTo(() => _context.Get<IFormula>(_oldFormula.Id)).Returns(_oldFormula);
         A.CallTo(() => _context.Get<IFormula>(_newFormula.Id)).Returns(_newFormula);
         sut = new EditParameterRHSFormulaInBuildingBlockCommand(_newFormula, _parameter.RHSFormula, _parameter, _buildingBlock);
      }
   }

   public class When_reverting_the_update_of_a_rhs_formula_in_a_parameter : concern_for_EditParameterRHSFormulaInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_rhs_formula_should_be_reverted()
      {
         _parameter.RHSFormula.ShouldBeEqualTo(_oldFormula);
      }
   }

   public class When_updating_the_rhs_formula_of_a_parameter : concern_for_EditParameterRHSFormulaInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_rhs_formula_should_be_changed()
      {
         _parameter.RHSFormula.ShouldBeEqualTo(_newFormula);
      }
   }

   public class When_updating_the_rhs_formula_of_a_parameter_with_a_null_formula : concern_for_EditParameterRHSFormulaInBuildingBlockCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new EditParameterRHSFormulaInBuildingBlockCommand(null, _parameter.RHSFormula, _parameter, _buildingBlock);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_rhs_formula_should_be_changed()
      {
         _parameter.RHSFormula.ShouldBeNull();
      }
   }

}
