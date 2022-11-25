using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
    public class concern_for_EditExpressionProfileBuildingBlockPresenter : ContextSpecification<EditExpressionProfileBuildingBlockPresenter>
   {
      protected IExpressionProfileBuildingBlockPresenter _expressionProfileBuildingBlockPresenter;
      protected IEditExpressionProfileBuildingBlockView _view;
      protected ExpressionProfileBuildingBlock _expressionProfile;
      private IFormulaCachePresenter _formulaCachePresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditExpressionProfileBuildingBlockView>();
         _expressionProfileBuildingBlockPresenter = A.Fake<IExpressionProfileBuildingBlockPresenter>();
         _expressionProfile = new ExpressionProfileBuildingBlock();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         sut = new EditExpressionProfileBuildingBlockPresenter(_view, _expressionProfileBuildingBlockPresenter, _formulaCachePresenter);
      }
   }

   public class When_editing_a_expression_profile_building_block : concern_for_EditExpressionProfileBuildingBlockPresenter
   {
      protected override void Because()
      {
         sut.Edit(_expressionProfile);
      }

      [Observation]
      public void the_sub_presenter_is_also_editing_the_profile()
      {
         A.CallTo(() => _expressionProfileBuildingBlockPresenter.Edit(_expressionProfile)).MustHaveHappened();
      }

      [Observation]
      public void the_caption_name_should_be_set()
      {
         _view.Caption.ShouldBeEqualTo(Assets.AppConstants.Captions.ExpressionProfileBuildingBlockCaption(_expressionProfile.Name));
      }
   }
}
