using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditEventGroupBuildingBlockPresenter : ContextSpecification<IEditEventGroupBuildingBlockPresenter>
   {
      private IEditEventGroupBuildingBlockView _view;
      private IEventGroupListPresenter _presenter;
      private IFormulaCachePresenter _formulaCachePresenter;
      private IEditApplicationBuilderPresenter _applicationPresenter;
      protected IEditEventGroupPresenter _eventGroupPresenter;
      private IEditEventBuilderPresenter _eventBuilderPresenter;
      private IEditTransportBuilderPresenter _transportBuilderPresenter;
      private IEditContainerPresenter _containerPresenter;
      private IEditFavoritesInEventGroupsPresenter _favoritesPresenter;
      private IUserDefinedParametersPresenter _userDefinedParametersPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditEventGroupBuildingBlockView>();
         _presenter = A.Fake<IEventGroupListPresenter>();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         _applicationPresenter = A.Fake<IEditApplicationBuilderPresenter>();
         _eventGroupPresenter = A.Fake<IEditEventGroupPresenter>();
         _eventBuilderPresenter = A.Fake<IEditEventBuilderPresenter>();
         _transportBuilderPresenter = A.Fake<IEditTransportBuilderPresenter>();
         _containerPresenter = A.Fake<IEditContainerPresenter>();
         _favoritesPresenter = A.Fake<IEditFavoritesInEventGroupsPresenter>();
         _userDefinedParametersPresenter = A.Fake<IUserDefinedParametersPresenter>();

         sut = new EditEventGroupBuildingBlockPresenter(_view, _presenter, _formulaCachePresenter, _applicationPresenter,
            _eventGroupPresenter, _eventBuilderPresenter, _transportBuilderPresenter, _containerPresenter,
            _favoritesPresenter, _userDefinedParametersPresenter);
      }
   }

   public class When_a_event_group_builder_is_selected : concern_for_EditEventGroupBuildingBlockPresenter
   {
      private EventGroupBuildingBlock _eventGroupBuildingBlock;
      private EventGroupBuilder _eventGroup;

      protected override void Context()
      {
         base.Context();
         _eventGroupBuildingBlock = new EventGroupBuildingBlock();
         _eventGroup = new EventGroupBuilder().WithName("EG");
         _eventGroupBuildingBlock.Add(_eventGroup);
         var parameter = new Parameter().WithName("P1").WithParentContainer(_eventGroup);
      }

      protected override void Because()
      {
         sut.Edit(_eventGroupBuildingBlock);
      }

      [Observation]
      public void should_edit_the_event_group_with_presenter()
      {
         A.CallTo(() => _eventGroupPresenter.Edit(_eventGroup)).MustHaveHappened();
      }

      [Observation]
      public void should_not_select_any_parameter()
      {
         A.CallTo(() => _eventGroupPresenter.SelectParameter(A<IParameter>._)).MustNotHaveHappened();
      }
   }
}