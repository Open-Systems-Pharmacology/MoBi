using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_ExpressionProfileBuildingBlockPresenter : ContextSpecification<ExpressionProfileBuildingBlockPresenter>
   {
      protected IExpressionProfileBuildingBlockView _view;
      protected ExpressionParameterToExpressionParameterDTOMapper _expressionParameterToExpressionParameterDTOMapper;
      protected ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper _expressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper;
      protected ExpressionProfileBuildingBlock _buildingBlock;
      private ExpressionParameter _expressionParameter1;
      private ExpressionParameter _expressionParameter2;

      protected override void Context()
      {
         _expressionParameterToExpressionParameterDTOMapper = new ExpressionParameterToExpressionParameterDTOMapper(new FormulaToValueFormulaDTOMapper());
         _expressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper = new ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper(_expressionParameterToExpressionParameterDTOMapper);
         _view = A.Fake<IExpressionProfileBuildingBlockView>();
         IInteractionTasksForExpressionProfileBuildingBlock interactionTaskForExpressionProfile = A.Fake<IInteractionTasksForExpressionProfileBuildingBlock>();
         sut = new ExpressionProfileBuildingBlockPresenter(_view, _expressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper, interactionTaskForExpressionProfile);

         _expressionParameter1 = new ExpressionParameter { Path = new ObjectPath("Path1", "Path2", "Name"), Value = 10 };
         _expressionParameter2 = new ExpressionParameter { Path = new ObjectPath("Path1", "Path3", "Name"), Value = 1 };
         _buildingBlock = new ExpressionProfileBuildingBlock
         {
            _expressionParameter1,
            _expressionParameter2
         };
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
}