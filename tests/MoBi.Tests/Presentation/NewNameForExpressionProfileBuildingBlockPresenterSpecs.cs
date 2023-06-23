using FakeItEasy;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public class concern_for_NewNameForExpressionProfileBuildingBlockPresenter : ContextSpecification<NewNameForExpressionProfileBuildingBlockPresenter>
   {
      private IRenameExpressionProfileDTOCreator _renameExpressionProfileDTOCreator;
      protected INewNameExpressionProfileBuildingBlockView _view;
      protected ExpressionProfileBuildingBlock _expressionProfile;
      protected RenameExpressionProfileDTO _expressionProfileDTO;

      protected override void Context()
      {
         _view = A.Fake<INewNameExpressionProfileBuildingBlockView>();
         _renameExpressionProfileDTOCreator = A.Fake<IRenameExpressionProfileDTOCreator>();
         _expressionProfile = new ExpressionProfileBuildingBlock
         {
            Name = "Molecule|Species|Category",
            Type = ExpressionTypes.MetabolizingEnzyme
         };

         _expressionProfileDTO = new RenameExpressionProfileDTO()
         {
            Species = "New Species",
         };

         A.CallTo(() => _renameExpressionProfileDTOCreator.Create(_expressionProfile.MoleculeName, _expressionProfile.Species, _expressionProfile.Category, _expressionProfile.Type)).Returns(_expressionProfileDTO);
         sut = new NewNameForExpressionProfileBuildingBlockPresenter(_view, _renameExpressionProfileDTOCreator);
      }
   }

   public class When_the_getting_a_new_name_for_expression_profile_and_the_view_is_cancelled : concern_for_NewNameForExpressionProfileBuildingBlockPresenter
   {
      private string _newName;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         _newName = sut.NewNameFrom(_expressionProfile.MoleculeName, _expressionProfile.Species, _expressionProfile.Category, _expressionProfile.Type, new[] { _expressionProfile.Name });
      }

      [Observation]
      public void the_name_returned_should_be_empty()
      {
         _newName.ShouldBeNullOrEmpty();
      }
   }

   public class When_renaming_an_expression_profile_building_block : concern_for_NewNameForExpressionProfileBuildingBlockPresenter
   {
      private string _newName;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         _newName = sut.NewNameFrom(_expressionProfile.MoleculeName, _expressionProfile.Species, _expressionProfile.Category, _expressionProfile.Type, new[] { _expressionProfile.Name });
      }

      [Observation]
      public void should_display_the_rename_expression_profile_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }

      [Observation]
      public void should_bind_the_expression_profile_to_the_view()
      {
         A.CallTo(() => _view.BindTo(_expressionProfileDTO)).MustHaveHappened();
      }

      [Observation]
      public void the_presenter_returns_an_empty_string_if_view_is_cancelled()
      {
         _newName.ShouldBeEqualTo(_expressionProfileDTO.Name);
      }
   }
}