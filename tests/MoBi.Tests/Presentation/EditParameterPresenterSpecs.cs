using System;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditParameterPresenter : ContextSpecification<IEditParameterPresenter>
   {
      protected IEditParameterView _view;
      protected IEditFormulaPresenter _editFormulaPresenter;
      protected IParameterToParameterDTOMapper _parameterMapper;
      protected IEditFormulaPresenter _editRHSPresenter;
      protected IInteractionTaskContext _interactionTasksContext;
      protected IGroupRepository _groupRepository;
      private IEditTaskFor<IParameter> _editTasks;
      protected IFavoriteTask _favoriteTask;
      protected IInteractionTasksForParameter _parameterTask;
      protected ICommandCollector _commandCollector;
      protected IEditValueOriginPresenter _editValueOriginPresenter;
      protected ITagsPresenter _tagsPresenter;
      protected IDescriptorConditionListPresenter<IParameter> _descriptorConditionListPresenter;
      protected IParameter _parameter;
      protected ParameterDTO _parameterDTO;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _view = A.Fake<IEditParameterView>();
         _editFormulaPresenter = A.Fake<IEditFormulaPresenter>();
         _parameterMapper = A.Fake<IParameterToParameterDTOMapper>();
         _editRHSPresenter = A.Fake<IEditFormulaPresenter>();
         _interactionTasksContext = A.Fake<IInteractionTaskContext>();
         _groupRepository = A.Fake<IGroupRepository>();
         _editTasks = A.Fake<IEditTaskFor<IParameter>>();
         _favoriteTask = A.Fake<IFavoriteTask>();
         _parameterTask = A.Fake<IInteractionTasksForParameter>();
         _editValueOriginPresenter = A.Fake<IEditValueOriginPresenter>();
         _tagsPresenter = A.Fake<ITagsPresenter>();
         _descriptorConditionListPresenter = A.Fake<IDescriptorConditionListPresenter<IParameter>>();

         _buildingBlock = A.Fake<IBuildingBlock>();

         sut = new EditParameterPresenter(
            _view,
            _editFormulaPresenter,
            _parameterMapper,
            _editRHSPresenter,
            _interactionTasksContext,
            _groupRepository,
            _editTasks,
            _parameterTask,
            new ContextSpecificReferencesRetriever(),
            _favoriteTask,
            _editValueOriginPresenter,
            _tagsPresenter,
            _descriptorConditionListPresenter)
         {
            BuildingBlock = _buildingBlock
         };

         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);

         _parameter = new Parameter().WithId("Para");
         _parameterDTO = new ParameterDTO(_parameter);
         A.CallTo(() => _parameterMapper.MapFrom(_parameter)).Returns(_parameterDTO);
      }
   }

   internal class When_set_is_variable_in_population_is_called : concern_for_EditParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameter);
      }

      protected override void Because()
      {
         sut.SetIsVariablePopulation(true);
      }

      [Observation]
      public void should_add_an_EditParameterCanBeVariedInPopulationCommand_to_context()
      {
         A.CallTo(() => _commandCollector.AddCommand(A<EditParameterCanBeVariedInPopulationCommand>._)).MustHaveHappened();
      }
   }

   public class When_initializing_the_edit_parameter_presenter : concern_for_EditParameterPresenter
   {
      [Observation]
      public void should_only_allow_constant_and_explicit_formula_for_RHS()
      {
         A.CallTo(() => _editRHSPresenter.RemoveAllFormulaTypes()).MustHaveHappened();
         A.CallTo(() => _editRHSPresenter.AddFormulaType<ConstantFormula>()).MustHaveHappened();
         A.CallTo(() => _editRHSPresenter.AddFormulaType<ExplicitFormula>()).MustHaveHappened();
         A.CallTo(() => _editRHSPresenter.SetDefaultFormulaType<ExplicitFormula>()).MustHaveHappened();
      }
   }

   public class When_retrieving_the_parameter_groups : concern_for_EditParameterPresenter
   {
      private IGroup _group1;
      private IGroup _group2;

      protected override void Context()
      {
         base.Context();
         _group1 = A.Fake<IGroup>();
         _group2 = A.Fake<IGroup>();
         _group1.FullName = "1111 First added but last";
         _group2.FullName = "0000 Second added but first";
         A.CallTo(() => _groupRepository.All()).Returns(new[] {_group1, _group2});
      }

      [Observation]
      public void should_return_the_groups_ordered_by_full_name()
      {
         sut.AllGroups().ShouldOnlyContainInOrder(_group2, _group1);
      }
   }

   internal class When_set_is_favorite_in_parameter_is_called : concern_for_EditParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameter);
      }

      protected override void Because()
      {
         sut.SetIsFavorite(true);
      }

      [Observation]
      public void should_call_the_favorite_task()
      {
         A.CallTo(() => _favoriteTask.SetParameterFavorite(_parameter, true)).MustHaveHappened();
      }
   }

   internal class When_the_value_description_is_set_in_a_parameter : concern_for_EditParameterPresenter
   {
      private ValueOrigin _valueOrigin;

      protected override void Context()
      {
         base.Context();
         _valueOrigin = new ValueOrigin();
         sut.Edit(_parameter);
      }

      protected override void Because()
      {
         _editValueOriginPresenter.ValueOriginUpdated(_valueOrigin);
      }

      [Observation]
      public void should_call_the_parameter_task()
      {
         A.CallTo(() => _parameterTask.SetValueOriginForParameter(_parameter, _valueOrigin, _buildingBlock)).MustHaveHappened();
      }
   }

   public class When_evaluating_if_the_edit_parameter_presenter_can_close_for_a_parameter_not_using_rhs : concern_for_EditParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameter);

         A.CallTo(() => _view.HasError).Returns(false);
         A.CallTo(() => _editFormulaPresenter.CanClose).Returns(true);
         A.CallTo(() => _editRHSPresenter.CanClose).Returns(false);

         _parameterDTO.HasRHS = false;
      }

      [Observation]
      public void should_not_use_the_can_close_state_of_the_rhs_presenter()
      {
         sut.CanClose.ShouldBeTrue();
      }
   }

   public class When_evaluating_if_the_edit_parameter_presenter_can_close_for_a_parameter_using_rhs : concern_for_EditParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_parameter);

         A.CallTo(() => _view.HasError).Returns(false);
         A.CallTo(() => _editFormulaPresenter.CanClose).Returns(true);
         A.CallTo(() => _editRHSPresenter.CanClose).Returns(false);

         _parameterDTO.HasRHS = true;
      }

      [Observation]
      public void should_use_the_can_close_state_of_the_rhs_presenter()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }

   public class When_initializing_the_container_tag_conditions_for_the_parameter : concern_for_EditParameterPresenter
   {
      private Func<IParameter, DescriptorCriteria> _creator;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut.BuildingBlock = _buildingBlock;
         A.CallTo(() => _descriptorConditionListPresenter.Edit(_parameter, A<Func<IParameter, DescriptorCriteria>>._, A<Func<IParameter, DescriptorCriteria>>._, _buildingBlock))
            .Invokes(x => _creator = x.GetArgument<Func<IParameter, DescriptorCriteria>>(2));
      }

      protected override void Because()
      {
         sut.Edit(_parameter);
      }

      [Observation]
      public void should_provide_a_container_criteria_creator_that_will_indeed_create_container_criteria_when_called()
      {
         _parameter.ContainerCriteria.ShouldBeNull();
         var criteria = _creator(_parameter);
         _parameter.ContainerCriteria.ShouldNotBeNull();
         _parameter.ContainerCriteria.ShouldBeEqualTo(criteria);
      }
   }
}