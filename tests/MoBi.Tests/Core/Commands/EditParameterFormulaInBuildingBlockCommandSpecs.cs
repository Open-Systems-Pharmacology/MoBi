using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterFormulaInBuildingBlockCommand : ContextSpecification<EditParameterFormulaInBuildingBlockCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected IParameter _parameter;
      protected IFormula _newFormula;
      protected IMoBiContext _context;
      protected IFormula _oldFormula;

      protected override void Context()
      {
         _oldFormula = new ExplicitFormula().WithId("oldId");
         _parameter = new Parameter().WithFormula(_oldFormula);
         _newFormula = new ExplicitFormula().WithId("newId");
         _buildingBlock = A.Fake<IBuildingBlock>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         A.CallTo(() => _context.Get<IFormula>(_oldFormula.Id)).Returns(_oldFormula);
         A.CallTo(() => _context.Get<IFormula>(_newFormula.Id)).Returns(_newFormula);
         sut = new EditParameterFormulaInBuildingBlockCommand(_newFormula, _parameter.Formula, _parameter, _buildingBlock);
      }
   }


   public class When_reverting_the_update_of_a_formula_in_a_parameter : concern_for_EditParameterFormulaInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_formula_should_be_reverted()
      {
         _parameter.Formula.ShouldBeEqualTo(_oldFormula);
      }
   }

   public class When_updating_the_formula_of_a_parameter : concern_for_EditParameterFormulaInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_formula_should_be_changed()
      {
         _parameter.Formula.ShouldBeEqualTo(_newFormula);
      }
   }
}
