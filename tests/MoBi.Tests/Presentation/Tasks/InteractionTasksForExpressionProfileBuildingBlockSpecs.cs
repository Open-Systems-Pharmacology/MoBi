using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_InteractionTasksForExpressionProfileBuildingBlock : ContextSpecification<IInteractionTasksForExpressionProfileBuildingBlock>
   {
      protected IMoBiFormulaTask _formulaTask;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IEditTasksForExpressionProfileBuildingBlock _editTask;
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected ExpressionParameter _expressionParameter;
      protected IFormula _formula;

      protected override void Context()
      {
         _editTask = A.Fake<IEditTasksForExpressionProfileBuildingBlock>();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();


         sut = new InteractionTasksForExpressionProfileBuildingBlock(_interactionTaskContext, _editTask, _formulaTask);

         _formula = new ExplicitFormula("y=mx+b");
         _expressionParameter = GetExpressionParameter();
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter
         };
      }

      protected virtual ExpressionParameter GetExpressionParameter()
      {
         return new ExpressionParameter { Formula = null, Value = 1.0 };
      }
   }

   public class When_cloning_an_expression_profile : concern_for_InteractionTasksForExpressionProfileBuildingBlock
   {
      protected override void Because()
      {
         sut.Clone(_buildingBlock);
      }

      [Observation]
      public void the_new_name_should_be_collected_using_the_expression_profile_namer()
      {
         // Ignore the Category because it will contain a suggested name change due to cloning
         A.CallTo(() => _editTask.NewNameFromSuggestions(_buildingBlock.MoleculeName, _buildingBlock.Species, A<string>._, _buildingBlock.Type, A<IReadOnlyList<string>>._)).MustHaveHappened();
      }
   }

   public class When_setting_a_formula_in_a_building_block : concern_for_InteractionTasksForExpressionProfileBuildingBlock
   {
      protected override void Because()
      {
         sut.SetFormula(_buildingBlock, _expressionParameter, _formula);
      }

      [Observation]
      public void the_formula_should_be_set_in_the_expression_parameter()
      {
         _expressionParameter.Formula.ShouldBeEqualTo(_formula);
      }

      [Observation]
      public void the_value_should_be_NotANumber()
      {
         _expressionParameter.Value.ShouldBeEqualTo(double.NaN);
      }


   }

   public class When_setting_a_value_in_a_building_block : concern_for_InteractionTasksForExpressionProfileBuildingBlock
   {

      protected override void Because()
      {
         sut.SetValue(_buildingBlock, 1.0, _expressionParameter);
      }

      protected override ExpressionParameter GetExpressionParameter()
      {
         return new ExpressionParameter { Formula = new ExplicitFormula("y=b"), Value = null };
      }

      [Observation]
      public void the_formula_should_be_cleared_in_the_expression_parameter()
      {
         _expressionParameter.Formula.ShouldBeEqualTo(null);
      }

      [Observation]
      public void the_value_should_be_set_in_the_expression_parameter()
      {
         _expressionParameter.Value.ShouldBeEqualTo(1.0);
      }
   }

   public class When_setting_a_unit_in_a_building_block : concern_for_InteractionTasksForExpressionProfileBuildingBlock
   {
      private Unit _unit1;
      private Dimension _dimension;
      private Unit _unit2;

      protected override ExpressionParameter GetExpressionParameter()
      {
         _dimension = new Dimension(new BaseDimensionRepresentation(), "dimensionName", "baseUnit");
         _unit1 = new Unit("displayUnit", 1, 0);
         _unit2 = new Unit("anotherUnit", 10, 0);
         _dimension.AddUnit(_unit1);
         _dimension.AddUnit(_unit2);

         return new ExpressionParameter { Formula = null, Value = 1.0 };
      }

      protected override void Because()
      {
         sut.SetUnit(_buildingBlock, _expressionParameter, _unit2);
      }

      [Observation]
      public void the_unit_should_be_changed()
      {
         _expressionParameter.DisplayUnit.ShouldBeEqualTo(_unit2);
      }
   }
}
