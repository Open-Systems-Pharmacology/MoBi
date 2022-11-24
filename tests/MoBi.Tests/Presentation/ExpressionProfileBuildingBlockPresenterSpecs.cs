using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public class concern_for_ExpressionProfileBuildingBlockPresenter : ContextSpecification<ExpressionProfileBuildingBlockPresenter>
   {
      protected IExpressionProfileBuildingBlockView _view;
      protected ExpressionParameterToExpressionParameterDTOMapper _expressionParameterToExpressionParameterDTOMapper;
      protected ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _expressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper;
      protected ExpressionProfileBuildingBlock _buildingBlock;
      protected ExpressionParameter _expressionParameter1;
      protected ExpressionParameter _expressionParameter2;
      protected ExpressionProfileBuildingBlockDTO _buildingBlockDTO;
      protected IInteractionTasksForExpressionProfileBuildingBlock _interactionTaskForExpressionProfile;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         _expressionParameterToExpressionParameterDTOMapper = new ExpressionParameterToExpressionParameterDTOMapper(new FormulaToValueFormulaDTOMapper());
         _expressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper = new ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper(_expressionParameterToExpressionParameterDTOMapper);
         _view = A.Fake<IExpressionProfileBuildingBlockView>();
         _interactionTaskForExpressionProfile = A.Fake<IInteractionTasksForExpressionProfileBuildingBlock>();
         sut = new ExpressionProfileBuildingBlockPresenter(_view, _expressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper, _interactionTaskForExpressionProfile);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);

         _expressionParameter1 = new ExpressionParameter { Path = new ObjectPath("Path1", "Path2", "Name"), Value = 10 };
         _expressionParameter2 = new ExpressionParameter { Path = new ObjectPath("Path1", "Path3", "Name"), Value = 1 };
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter1,
            _expressionParameter2
         };

         A.CallTo(() => _view.BindTo(A<ExpressionProfileBuildingBlockDTO>._)).Invokes(x => _buildingBlockDTO = x.GetArgument<ExpressionProfileBuildingBlockDTO>(0));
      }

      public class When_searching_for_multiple_distinct_paths : concern_for_ExpressionProfileBuildingBlockPresenter
      {
         protected override void Context()
         {
            base.Context();
            sut.Edit(_buildingBlock);
         }

         [Observation]
         public void should_find_distinct_paths_only_where_appropriate()
         {
            sut.HasAtLeastTwoDistinctValues(0).ShouldBeFalse();
            sut.HasAtLeastTwoDistinctValues(1).ShouldBeTrue();
         }
      }

      public class When_editing_the_building_block : concern_for_ExpressionProfileBuildingBlockPresenter
      {
         protected override void Because()
         {
            sut.Edit(_buildingBlock);
         }

         [Observation]
         public void the_view_should_bind_to_an_appropriate_dto()
         {
            A.CallTo(() => _view.BindTo(A<ExpressionProfileBuildingBlockDTO>._)).MustHaveHappened();
         }
      }
   }

   public class When_setting_the_unit_for_a_builder : concern_for_ExpressionProfileBuildingBlockPresenter
   {
      private Unit _unit;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
      }

      protected override void Because()
      {
         _unit = new Unit("", 1, 0);
         sut.SetUnit(_buildingBlockDTO.ExpressionParameters.First(), _unit);
      }

      [Observation]
      public void the_presenter_uses_the_task_to_set_the_unit()
      {
         A.CallTo(() => _interactionTaskForExpressionProfile.SetUnit(_buildingBlock, _expressionParameter1, _unit)).MustHaveHappened();
      }

   }

   public class When_adding_a_new_formula_to_the_building_block : concern_for_ExpressionProfileBuildingBlockPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
      }

      protected override void Because()
      {
         sut.AddNewFormula(_buildingBlockDTO.ExpressionParameters.First());
      }

      [Observation]
      public void the_presenter_uses_the_interaction_task_to_add_the_formula()
      {
         A.CallTo(() => _interactionTaskForExpressionProfile.AddNewFormulaAtBuildingBlock(_buildingBlock, _expressionParameter1, null)).MustHaveHappened();
      }
   }

   public class When_changing_the_expression_formula : concern_for_ExpressionProfileBuildingBlockPresenter
   {
      private ExplicitFormula _formula;
      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
         _formula = new ExplicitFormula();
      }

      protected override void Because()
      {
         sut.SetFormula(_buildingBlockDTO.ExpressionParameters.First(), _formula);
      }

      [Observation]
      public void the_presenter_uses_the_task_to_set_the_formula()
      {
         A.CallTo(() => _interactionTaskForExpressionProfile.SetFormula(_buildingBlock, _expressionParameter1, _formula)).MustHaveHappened();
      }
   }

   public class When_changing_the_expression_parameter_value : concern_for_ExpressionProfileBuildingBlockPresenter
   {
      private double? _newValue;

      protected override void Context()
      {
         base.Context();
         sut.Edit(_buildingBlock);
         _newValue = 99;
      }

      protected override void Because()
      {
         sut.SetExpressionParameterValue(_buildingBlockDTO.ExpressionParameters.First(), _newValue);
      }

      [Observation]
      public void the_presenter_uses_the_task_to_set_the_value()
      {
         A.CallTo(() => _interactionTaskForExpressionProfile.SetValue(_buildingBlock, _newValue, _expressionParameter1)).MustHaveHappened();
      }
   }
}