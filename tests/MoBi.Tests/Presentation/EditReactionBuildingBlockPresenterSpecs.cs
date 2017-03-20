﻿using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditReactionBuildingBlockPresenter : ContextSpecification<EditReactionBuildingBlockPresenter>
   {
      private IEditFavoritesInReactionsPresenter _editFavoritesInReactionsPresenter;
      protected IReactionsListSubPresenter _reactionListPresenter;
      private IEditReactionBuildingBlockView _view;
      protected IReactionDiagramPresenter _reactionDiagramPresenter;
      protected IEditReactionBuilderPresenter _editReactionBuilderPresenter;
      private IFormulaCachePresenter _formulaCachePresenter;

      protected override void Context()
      {
         _editFavoritesInReactionsPresenter = A.Fake<IEditFavoritesInReactionsPresenter>();
         _reactionDiagramPresenter = A.Fake<IReactionDiagramPresenter>();
         _reactionListPresenter = A.Fake<IReactionsListSubPresenter>();
         _view = A.Fake<IEditReactionBuildingBlockView>();
         _editReactionBuilderPresenter = A.Fake<IEditReactionBuilderPresenter>();
         _formulaCachePresenter = A.Fake<IFormulaCachePresenter>();

         sut = new EditReactionBuildingBlockPresenter(_view, _reactionListPresenter, _reactionDiagramPresenter, _editReactionBuilderPresenter, _formulaCachePresenter, _editFavoritesInReactionsPresenter);
      }
   }

   public class When_handling_the_removed_event_and_the_parent_building_block_is_the_one_edited : concern_for_EditReactionBuildingBlockPresenter
   {
      private IObjectBase _removedObject;
      private IMoBiReactionBuildingBlock _parent;

      protected override void Context()
      {
         base.Context();
         _removedObject = new ReactionBuilder();
         _parent = A.Fake<IMoBiReactionBuildingBlock>();
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
      private IMoBiReactionBuildingBlock _parent;

      protected override void Context()
      {
         base.Context();
         _removedObject = new ReactionBuilder();
         _parent = A.Fake<IMoBiReactionBuildingBlock>();
         sut.Edit(A.Fake<IMoBiReactionBuildingBlock>());
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
      private IReactionBuilder _reaction;
      private IMoBiReactionBuildingBlock _reactionBuildingBlock;

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
