using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditReactionBuildingBlockPresenter : ContextSpecification<EditReactionBuildingBlockPresenter>
   {
      protected IEditFavoritesInReactionsPresenter _editFavoritesInReactionsPresenter;
      protected IReactionsListSubPresenter _reactionListPresenter;
      private IEditReactionBuildingBlockView _view;
      protected IReactionDiagramPresenter _reactionDiagramPresenter;
      protected IEditReactionBuilderPresenter _editReactionBuilderPresenter;
      protected IFormulaCachePresenter _formulaCachePresenter;
      protected IUserDefinedParametersPresenter _userDefinedParametersPresenter;

      protected override void Context()
      {
         _editFavoritesInReactionsPresenter = A.Fake<IEditFavoritesInReactionsPresenter>();
         _reactionDiagramPresenter = A.Fake<IReactionDiagramPresenter>();
         _reactionListPresenter = A.Fake<IReactionsListSubPresenter>();
         _view = A.Fake<IEditReactionBuildingBlockView>();
         _editReactionBuilderPresenter = A.Fake<IEditReactionBuilderPresenter>();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();
         _userDefinedParametersPresenter = A.Fake<IUserDefinedParametersPresenter>();

         sut = new EditReactionBuildingBlockPresenter(_view, _reactionListPresenter, _reactionDiagramPresenter,
            _editReactionBuilderPresenter, _formulaCachePresenter,
            _editFavoritesInReactionsPresenter, _userDefinedParametersPresenter);
      }
   }

   public class When_editing_a_reaction_building_block_presenter : concern_for_EditReactionBuildingBlockPresenter
   {
      private MoBiReactionBuildingBlock _reactionBuildingBlock;
      private ReactionBuilder _reaction;

      protected override void Context()
      {
         base.Context();
         _reaction = new ReactionBuilder();
         _reactionBuildingBlock = new MoBiReactionBuildingBlock {_reaction};
      }

      protected override void Because()
      {
         sut.Edit(_reactionBuildingBlock);
      }

      [Observation]
      public void should_display_the_list_of_all_rections_defined_in_the_building_block()
      {
         A.CallTo(() => _reactionListPresenter.Edit(_reactionBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_diagram_of_all_rections_defined_in_the_building_block()
      {
         A.CallTo(() => _reactionDiagramPresenter.Edit(_reactionBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_edit_the_first_reaction_of_the_reaciton_building_block_if_defined()
      {
         A.CallTo(() => _editReactionBuilderPresenter.Edit(_reaction)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_favorites_defined_in_the_reaciton_building_blocks()
      {
         A.CallTo(() => _editFavoritesInReactionsPresenter.Edit(_reactionBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_user_defined_paramters_defined_in_the_reaction_building_block()
      {
         A.CallTo(() => _userDefinedParametersPresenter.ShowUserDefinedParametersIn(_reactionBuildingBlock)).MustHaveHappened();
      }
   }

   public class When_the_edit_reaction_building_block_presenter_is_updating_the_list_of_user_defined_parameters : concern_for_EditReactionBuildingBlockPresenter
   {
      private MoBiReactionBuildingBlock _reactionBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _reactionBuildingBlock = new MoBiReactionBuildingBlock();
         sut.Edit(_reactionBuildingBlock);
      }

      protected override void Because()
      {
         sut.UpdateUserDefinedParameters();
      }

      [Observation]
      public void should_refresh_the_user_defined_parmaeters_defined_in_the_edited_reaction_building_block()
      {
         A.CallTo(() => _userDefinedParametersPresenter.ShowUserDefinedParametersIn(_reactionBuildingBlock)).MustHaveHappened();
      }
   }

   public class When_handling_the_removed_event_and_the_parent_building_block_is_the_one_edited : concern_for_EditReactionBuildingBlockPresenter
   {
      private IObjectBase _removedObject;
      private MoBiReactionBuildingBlock _parent;

      protected override void Context()
      {
         base.Context();
         _removedObject = new ReactionBuilder();
         _parent = A.Fake<MoBiReactionBuildingBlock>();
         sut.Edit(_parent);
      }

      protected override void Because()
      {
         sut.Handle(new RemovedEvent(_removedObject, _parent));
      }

      [Observation]
      public void should_result_in_edit_being_called_with_the_parent()
      {
         // using this edit call to indicate whether the sut has called edit
         A.CallTo(() => _reactionListPresenter.Edit(_parent)).MustHaveHappened();
      }
   }

   public class When_handling_the_removed_event_and_the_parent_building_block_is_not_the_one_edited : concern_for_EditReactionBuildingBlockPresenter
   {
      private IObjectBase _removedObject;
      private MoBiReactionBuildingBlock _parent;

      protected override void Context()
      {
         base.Context();
         _removedObject = new ReactionBuilder();
         _parent = A.Fake<MoBiReactionBuildingBlock>();
         sut.Edit(A.Fake<MoBiReactionBuildingBlock>());
      }

      protected override void Because()
      {
         sut.Handle(new RemovedEvent(_removedObject, _parent));
      }

      [Observation]
      public void should_not_result_in_edit_being_called_with_the_parent()
      {
         // using this edit call to indicate whether the sut has called edit
         A.CallTo(() => _reactionListPresenter.Edit(_parent)).MustNotHaveHappened();
      }
   }

   public class When_the_edit_reaction_building_presenter_is_being_notifed_that_a_reaction_was_selected : concern_for_EditReactionBuildingBlockPresenter
   {
      private ReactionBuilder _reaction;
      private MoBiReactionBuildingBlock _reactionBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _reaction = new ReactionBuilder().WithId("1");
         _reactionBuildingBlock = new MoBiReactionBuildingBlock() {_reaction};
         sut.Edit(_reactionBuildingBlock);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_reaction, new object()));
      }

      [Observation]
      public void should_edit_the_reaction_in_the_reaction_presenter()
      {
         A.CallTo(() => _editReactionBuilderPresenter.Edit(_reaction)).MustHaveHappened();
      }

      [Observation]
      public void should_select_the_reaction_in_the_diagram()
      {
         A.CallTo(() => _reactionDiagramPresenter.Select(_reaction)).MustHaveHappened();
      }
   }
}