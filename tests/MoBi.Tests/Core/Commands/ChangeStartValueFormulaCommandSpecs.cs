using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeValueFormulaCommand<TCommand, TBuildingBlock, T> : ContextSpecification<TCommand> where T : PathAndValueEntity, IObjectBase, IUsingFormula where TCommand : ChangeValueFormulaCommand<T> where TBuildingBlock : IBuildingBlock<T>, new()
   {
      protected IFormula _newFormula;
      protected IFormula _oldFormula;
      protected T _changedBuilder;
      protected TBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected abstract TCommand GetCommand();

      protected override void Context()
      {
         _changedBuilder = A.Fake<T>();
         _newFormula = new ExplicitFormula { Id = "newFormulaId" };
         _oldFormula = new ExplicitFormula { Id = "oldFormulaId" };
         _buildingBlock = GetBuildingBlock();
         sut = GetCommand();

         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IFormula>(_oldFormula.Id)).Returns(_oldFormula);
         A.CallTo(() => _context.Get<IFormula>(_newFormula.Id)).Returns(_newFormula);
         _buildingBlock.Add(_changedBuilder);
         A.CallTo(() => _context.Get<IBuildingBlock<T>>(A<string>.Ignored)).Returns(_buildingBlock);
      }

      protected TBuildingBlock GetBuildingBlock()
      {
         return new TBuildingBlock();
      }
   }

   class When_inverting_a_psv_change_command : When_inverting_a_formula_change_command<ChangeValueFormulaCommand<MoleculeStartValue>, MoleculeStartValuesBuildingBlock, MoleculeStartValue>
   {
      protected override ChangeValueFormulaCommand<MoleculeStartValue> GetCommand()
      {
         return new ChangeValueFormulaCommand<MoleculeStartValue>(_buildingBlock, _changedBuilder, _newFormula, _oldFormula);
      }
   }

   class inverting_an_expression_formula_change_command : When_inverting_a_formula_change_command<ChangeValueFormulaCommand<ExpressionParameter>, ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      protected override ChangeValueFormulaCommand<ExpressionParameter> GetCommand()
      {
         return new ChangeValueFormulaCommand<ExpressionParameter>(_buildingBlock, _changedBuilder, _newFormula, _oldFormula);
      }
   }

   abstract class When_inverting_a_formula_change_command<TCommand, TBuildingBlock, TBuilder> : concern_for_ChangeValueFormulaCommand<TCommand, TBuildingBlock, TBuilder> 
      where TCommand : ChangeValueFormulaCommand<TBuilder> where TBuildingBlock : IBuildingBlock<TBuilder>, new() where TBuilder : PathAndValueEntity, IUsingFormula
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_change_start_value_formula_back_to_original()
      {
         _changedBuilder.Formula.ShouldBeEqualTo(_oldFormula);
      }

   }

   class executing_change_expression_value_formula_command : executing_change_formula_command<ChangeValueFormulaCommand<ExpressionParameter>, ExpressionProfileBuildingBlock, ExpressionParameter>
   {
      protected override ChangeValueFormulaCommand<ExpressionParameter> GetCommand()
      {
         return new ChangeValueFormulaCommand<ExpressionParameter>(_buildingBlock, _changedBuilder, _newFormula, _oldFormula);
      }
   }

   class executing_change_start_value_formula_command : executing_change_formula_command<ChangeValueFormulaCommand<MoleculeStartValue>, MoleculeStartValuesBuildingBlock, MoleculeStartValue>
   {
      protected override ChangeValueFormulaCommand<MoleculeStartValue> GetCommand()
      {
         return new ChangeValueFormulaCommand<MoleculeStartValue>(_buildingBlock, _changedBuilder, _newFormula, _oldFormula);
      }
   }

   abstract class executing_change_formula_command<TCommand, TBuildingBlock, TBuilder> : concern_for_ChangeValueFormulaCommand<TCommand, TBuildingBlock, TBuilder>
      where TCommand : ChangeValueFormulaCommand<TBuilder> where TBuildingBlock : IBuildingBlock<TBuilder>, new() where TBuilder : PathAndValueEntity, IUsingFormula
   {
      private const string _newFormulaId = "new";
   
      protected override void Context()
      {
         base.Context();
         _newFormula.Id = _newFormulaId;
      }
   
      protected override void Because()
      {
         sut.Execute(_context);
      }
   
      [Observation]
      public void should_set_molecule_startValues_Formula()
      {
         _changedBuilder.Formula.ShouldBeEqualTo(_newFormula);
      }
   }
}	