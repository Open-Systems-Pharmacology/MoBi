using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeStartValueFormulaCommand : ContextSpecification<ChangeValueFormulaCommand<IMoleculeStartValue>>
   {
      protected IMoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      protected IMoleculeStartValue _changedMoleculeStartValue;
      protected IFormula _newFormula;
      protected IFormula _oldFormula;

      protected override void Context()
      {
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _changedMoleculeStartValue = A.Fake<IMoleculeStartValue>();
         _newFormula = new ExplicitFormula{Id = "newFormulaId"};
         _oldFormula = new ExplicitFormula{Id = "oldFormulaId"};
         
         sut = new StartValueFormulaChangedCommand<IMoleculeStartValue>(_moleculeStartValuesBuildingBlock, _changedMoleculeStartValue, _newFormula, _oldFormula);
      }
   }

   class When_inverting_a_psv_change_command : concern_for_ChangeStartValueFormulaCommand
   {
      protected IMoBiContext _context;
      protected override void Context()
      {
         base.Context();
         
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IFormula>(_oldFormula.Id)).Returns(_oldFormula);
         A.CallTo(() => _context.Get<IFormula>(_newFormula.Id)).Returns(_newFormula);
         _moleculeStartValuesBuildingBlock.Add(_changedMoleculeStartValue);
         A.CallTo(() => _context.Get<IBuildingBlock<IMoleculeStartValue>>(A<string>.Ignored)).Returns(_moleculeStartValuesBuildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_change_start_value_formula_back_to_original()
      {
         _changedMoleculeStartValue.Formula.ShouldBeEqualTo(_oldFormula);
      }
   }

   class executing_change_start_value_formula_command : concern_for_ChangeStartValueFormulaCommand
   {
      private IMoBiContext _context;
      private const string _newFormulaId = "new";

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _newFormula.Id = _newFormulaId;
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_set_molecule_startValues_Formula()
      {
         _changedMoleculeStartValue.Formula.ShouldBeEqualTo(_newFormula);
      }
   }
}	