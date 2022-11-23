using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_InteractionTaskForExpressionProfileBuildingBlock : ContextSpecification<InteractionTasksForExpressionProfileBuildingBlock>
   {
      private IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock> _editTask;
      private IInteractionTaskContext _interactionTaskContext;
      private IMoBiFormulaTask _formulaTask;

      protected override void Context()
      {
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _editTask = A.Fake<IEditTasksForBuildingBlock<ExpressionProfileBuildingBlock>>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();

         sut = new InteractionTasksForExpressionProfileBuildingBlock(_interactionTaskContext, _editTask, _formulaTask);
      }
   }

   public class When_setting_the_value_of_a_expression_parameter : concern_for_InteractionTaskForExpressionProfileBuildingBlock
   {
      protected ExpressionParameter _expressionParameter;
      protected double? _newValue;
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected double? _oldValue;

      protected override void Context()
      {
         base.Context();
         _oldValue = null;
         _expressionParameter = new ExpressionParameter {Value = _oldValue};
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter
         };
         _newValue = 3.0;
      }

      protected override void Because()
      {
         sut.SetValue(_buildingBlock, _newValue, _expressionParameter);
      }

      [Observation]
      public void the_command_should_set_the_new_value_in_the_expression_parameter()
      {
         _expressionParameter.Value.ShouldBeEqualTo(_newValue);
      }
   }
}