using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditReactionBuilderPresenter : ContextSpecification<IEditReactionBuilderPresenter>
   {
      protected IEditReactionBuilderView _view;
      private IEditFormulaInContainerPresenter _editFormulaPresenter;
      private ISelectReferenceAtReactionPresenter _selectReferencesPresenter;
      protected IReactionBuilderToReactionBuilderDTOMapper _reactionBuilderMapper;
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private IEditTaskFor<ReactionBuilder> _editTasks;
      private IMoBiContext _context;
      private IFormulaToFormulaBuilderDTOMapper _formulaBuilderMapper;
      protected IEditParametersInContainerPresenter _editReactionParameters;
      private IDescriptorConditionListPresenter<ReactionBuilder> _containerCriteriaPresenter;
      protected IReactionEductsPresenter _reactionEductsPresenter;
      protected IReactionProductsPresenter _reactionProductsPresenter;
      protected IReactionModifiersPresenter _reactionModifiersPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditReactionBuilderView>();
         _editFormulaPresenter = A.Fake<IEditFormulaInContainerPresenter>();
         _selectReferencesPresenter = A.Fake<ISelectReferenceAtReactionPresenter>();
         _reactionBuilderMapper = A.Fake<IReactionBuilderToReactionBuilderDTOMapper>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _editTasks = A.Fake<IEditTaskFor<ReactionBuilder>>();
         _context = A.Fake<IMoBiContext>();
         _formulaBuilderMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _editReactionParameters = A.Fake<IEditParametersInContainerPresenter>();
         _containerCriteriaPresenter = A.Fake<IDescriptorConditionListPresenter<ReactionBuilder>>();
         _reactionEductsPresenter = A.Fake<IReactionEductsPresenter>();
         _reactionProductsPresenter = A.Fake<IReactionProductsPresenter>();
         _reactionModifiersPresenter = A.Fake<IReactionModifiersPresenter>();
         sut = new EditReactionBuilderPresenter(_view, _editFormulaPresenter, _selectReferencesPresenter, _reactionBuilderMapper, _viewItemContextMenuFactory, _editTasks,
            _formulaBuilderMapper, _editReactionParameters, _context, _containerCriteriaPresenter, _reactionEductsPresenter, _reactionProductsPresenter, _reactionModifiersPresenter);
      }
   }

   internal class When_reaction_builder_presenter_is_told_to_edit_null : concern_for_EditReactionBuilderPresenter
   {
      [Observation]
      public void should_not_throw_an_exception()
      {
         sut.Edit(null);
      }
   }

   public class When_editing_a_reaction : concern_for_EditReactionBuilderPresenter
   {
      private ReactionBuilder _reactionBuilder;
      private ReactionBuilderDTO _reactionBuilderDTO;

      protected override void Context()
      {
         base.Context();
         _reactionBuilder = new ReactionBuilder();
         _reactionBuilderDTO = A.Fake<ReactionBuilderDTO>();
         A.CallTo(() => _reactionBuilderMapper.MapFrom(_reactionBuilder)).Returns(_reactionBuilderDTO);
         sut.BuildingBlock = new MoBiReactionBuildingBlock();
      }

      protected override void Because()
      {
         sut.Edit(_reactionBuilder);
      }

      [Observation]
      public void the_edit_reaction_parameters_presenter_is_used_to_edit_reaction_parameters()
      {
         A.CallTo(() => _editReactionParameters.Edit(_reactionBuilder)).MustHaveHappened();
      }

      [Observation]
      public void the_view_must_bind_to_the_dto()
      {
         A.CallTo(() => _view.BindTo(_reactionBuilderDTO)).MustHaveHappened();
      }

      [Observation]
      public void the_reaction_builder_to_reaction_builder_dto_mapper_is_used_to_create_dto()
      {
         A.CallTo(() => _reactionBuilderMapper.MapFrom(_reactionBuilder)).MustHaveHappened();
      }

      [Observation]
      public void the_educts_are_edited_with_the_educts_connection_presenter()
      {
         A.CallTo(() => _reactionEductsPresenter.Edit(_reactionBuilderDTO, sut.BuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_products_are_edited_with_the_products_connection_presenter()
      {
         A.CallTo(() => _reactionProductsPresenter.Edit(_reactionBuilderDTO, sut.BuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_modifiers_are_edited_with_the_modifiers_connection_presenter()
      {
         A.CallTo(() => _reactionProductsPresenter.Edit(_reactionBuilderDTO, sut.BuildingBlock)).MustHaveHappened();
      }
   }

   public class When_selecting_a_parmaeter_in_the_reaction_builder_presenter : concern_for_EditReactionBuilderPresenter
   {
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
      }

      protected override void Because()
      {
         sut.SelectParameter(_parameter);
      }

      [Observation]
      public void should_select_the_parameter_in_the_parameter_list()
      {
         A.CallTo(() => _editReactionParameters.Select(_parameter)).MustHaveHappened();
      }

      [Observation]
      public void should_also_show_the_parameter_view()
      {
         A.CallTo(() => _view.ShowParameters()).MustHaveHappened();
      }
   }
}