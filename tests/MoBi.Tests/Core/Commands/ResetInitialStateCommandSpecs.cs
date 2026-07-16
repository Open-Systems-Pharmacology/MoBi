using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands;

internal class concern_for_ResetInitialStateCommand<TParameter, TBuildingBlock> : ContextSpecification<ResetInitialStateCommand<TParameter, TBuildingBlock>> where TParameter : ParameterValueWithInitialState, new() where TBuildingBlock : class, IBuildingBlock<TParameter>, new()
{
   protected TBuildingBlock _buildingBlock;
   protected TParameter _builder;

   protected override void Context()
   {
      _builder = new TParameter
      {
         Formula = null,
         Dimension = DimensionFactoryForSpecs.MassDimension,
         DisplayUnit = DimensionFactoryForSpecs.MassDimension.UnitAt(2),
         Path = new ObjectPath("path"),
         Value = 10
      };
      _buildingBlock = new TBuildingBlock().WithId("buildingBlockId");

      _buildingBlock.AddFormula(new ExplicitFormula().WithId("formulaID"));

      _builder.InitialValue = 4;
      _builder.InitialFormulaId = "formulaID";
      _builder.InitialUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit;

      _buildingBlock.Add(_builder);
      sut = new ResetInitialStateCommand<TParameter, TBuildingBlock>(_builder, _buildingBlock);
   }
}

internal class When_reversing_the_reset_initial_state_command_for_expression : When_reversing_the_reset_initial_state_command<ExpressionParameter, ExpressionProfileBuildingBlock>
{

}

internal class When_reversing_the_reset_initial_state_command_for_individual : When_reversing_the_reset_initial_state_command<IndividualParameter, IndividualBuildingBlock>
{

}

internal class When_reversing_the_reset_initial_state_command<TParameter, TBuildingBlock> : concern_for_ResetInitialStateCommand<TParameter, TBuildingBlock> where TParameter : ParameterValueWithInitialState, new() where TBuildingBlock : class, IBuildingBlock<TParameter>, new()
{
   private IMoBiContext _context;

   protected override void Context()
   {
      base.Context();
      _context = A.Fake<IMoBiContext>();

      A.CallTo(() => _context.Get<TBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
   }

   protected override void Because()
   {
      sut.ExecuteAndInvokeInverse(_context);
   }

   [Observation]
   public void the_value_should_be_restored_to_original()
   {
      _builder.Value.ShouldBeEqualTo(10);
   }

   [Observation]
   public void the_unit_should_be_restored_to_original()
   {
      _builder.DisplayUnit.ShouldBeEqualTo(DimensionFactoryForSpecs.MassDimension.UnitAt(2));
   }

   [Observation]
   public void the_formula_should_be_restored_to_null()
   {
      _builder.Formula.ShouldBeNull();
   }

   [Observation]
   public void the_initial_value_should_be_restored()
   {
      _builder.InitialValue.ShouldBeEqualTo(4);
   }

   [Observation]
   public void the_initial_unit_should_be_restored()
   {
      _builder.InitialUnit.ShouldBeEqualTo(DimensionFactoryForSpecs.MassDimension.DefaultUnit);
   }

   [Observation]
   public void the_initial_formula_id_should_be_restored()
   {
      _builder.InitialFormulaId.ShouldBeEqualTo("formulaID");
   }
}

internal class When_resetting_the_initial_state_for_expression : When_resetting_the_initial_state<ExpressionParameter, ExpressionProfileBuildingBlock>
{

}

internal class When_resetting_the_initial_state_for_individual : When_resetting_the_initial_state<IndividualParameter, IndividualBuildingBlock>
{

}

internal class When_resetting_the_initial_state<TParameter, TBuildingBlock> : concern_for_ResetInitialStateCommand<TParameter, TBuildingBlock> where TParameter : ParameterValueWithInitialState, new() where TBuildingBlock : class, IBuildingBlock<TParameter>, new()
{
   protected override void Context()
   {
      base.Context();
      _builder.Value = 10;
      _builder.Formula = null;
      _builder.DisplayUnit = DimensionFactoryForSpecs.MassDimension.UnitAt(2);
   }

   protected override void Because()
   {
      sut.RunCommand(A.Fake<IMoBiContext>());
   }

   [Observation]
   public void the_unit_should_be_reset()
   {
      _builder.DisplayUnit.ShouldBeEqualTo(DimensionFactoryForSpecs.MassDimension.DefaultUnit);
   }

   [Observation]
   public void the_value_should_be_reset()
   {
      _builder.Value.ShouldBeEqualTo(4);
   }

   [Observation]
   public void the_formula_should_be_reset()
   {
      _builder.Formula.Id.ShouldBeEqualTo("formulaID");
   }

   [Observation]
   public void the_initial_value_should_be_cleared()
   {
      _builder.InitialValue.ShouldBeNull();
   }

   [Observation]
   public void the_initial_unit_should_be_cleared()
   {
      _builder.InitialUnit.ShouldBeNull();
   }

   [Observation]
   public void the_initial_formula_id_should_be_cleared()
   {
      _builder.InitialFormulaId.ShouldBeNull();
   }
}