using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using static System.Double;

namespace MoBi.Presentation
{
   internal class concern_for_EditIndividualParameterPresenter : ContextSpecification<EditIndividualParameterPresenter>
   {
      protected IEditIndividualParameterView _view;
      protected IIndividualParameterToIndividualParameterDTOMapper _individualParameterToIndividualParameterDTOMapper;
      protected IEditValueOriginPresenter _editValueOriginPresenter;
      protected IEditFormulaInPathAndValuesPresenter _editFormulaInPathAndValuesPresenter;
      protected IInteractionTasksForIndividualBuildingBlock _interactionTasksForIndividualBuildingBlock;
      protected IndividualParameter _individualParameter;
      protected IndividualBuildingBlock _buildingBlock;
      protected IPathAndValueEntityToDistributedParameterMapper _pathAndValueEntityToDistributedParameterMapper;
      protected IMoBiFormulaTask _formulaTask;
      protected IEditTaskFor<IndividualBuildingBlock> _editTask;
      protected IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _view = A.Fake<IEditIndividualParameterView>();
         _individualParameterToIndividualParameterDTOMapper = A.Fake<IIndividualParameterToIndividualParameterDTOMapper>();
         _editValueOriginPresenter = A.Fake<IEditValueOriginPresenter>();
         _editFormulaInPathAndValuesPresenter = A.Fake<IEditFormulaInPathAndValuesPresenter>();
         _interactionTasksForIndividualBuildingBlock = A.Fake<IInteractionTasksForIndividualBuildingBlock>();
         _pathAndValueEntityToDistributedParameterMapper = A.Fake<IPathAndValueEntityToDistributedParameterMapper>();
         _editTask = A.Fake<IEditTaskFor<IndividualBuildingBlock>>();
         _formulaTask = A.Fake<IMoBiFormulaTask>();
         _eventPublisher = A.Fake<IEventPublisher>();

         _individualParameter = new IndividualParameter();
         _buildingBlock = new IndividualBuildingBlock { _individualParameter };
         
         A.CallTo(() => _individualParameterToIndividualParameterDTOMapper.MapFrom(A<IndividualParameter>._)).ReturnsLazily(x => new IndividualParameterDTO(x.Arguments.Get<IndividualParameter>(0)));
         sut = new EditIndividualParameterPresenter(_view, _individualParameterToIndividualParameterDTOMapper, _editValueOriginPresenter, _editFormulaInPathAndValuesPresenter, _interactionTasksForIndividualBuildingBlock, _formulaTask, _pathAndValueEntityToDistributedParameterMapper, _editTask, _eventPublisher);
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   internal class When_editing_parameter_with_formula : concern_for_EditIndividualParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         _individualParameter.Value = NaN;
         _individualParameter.Formula = new ConstantFormula(0);
      }

      protected override void Because()
      {
         sut.Edit(_individualParameter, _buildingBlock);
      }

      [Observation]
      public void the_view_should_reveal_the_formula_editor()
      {
         A.CallTo(() => _view.ShowFormulaEdit()).MustHaveHappened();
      }
   }

   internal class When_editing_parameter_with_value : concern_for_EditIndividualParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         _individualParameter.Value = 0;
         _individualParameter.Formula = null;
      }

      protected override void Because()
      {
         sut.Edit(_individualParameter, _buildingBlock);
      }

      [Observation]
      public void the_view_should_reveal_the_formula_editor()
      {
         A.CallTo(() => _view.HideFormulaEdit()).MustHaveHappened();
      }
   }

   internal class When_converting_to_value_from_formula : concern_for_EditIndividualParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         _individualParameter.Value = NaN;
         _individualParameter.Formula = new ConstantFormula(0);
         sut.Edit(_individualParameter, _buildingBlock);

         A.CallTo(() => _interactionTasksForIndividualBuildingBlock.SetValue(_buildingBlock, A<double>._, _individualParameter)).Invokes(x =>
         {
            _individualParameter.Value = x.Arguments.Get<double>(1);
            _individualParameter.Formula = null;
         });
      }

      protected override void Because()
      {
         sut.UpdateValue(4.0);
      }

      [Observation]
      public void the_interaction_task_sets_the_value()
      {
         A.CallTo(() => _interactionTasksForIndividualBuildingBlock.SetValue(_buildingBlock, 4.0, _individualParameter)).MustHaveHappened();
      }

      [Observation]
      public void the_view_should_hide_the_formula_editor()
      {
         A.CallTo(() => _view.HideFormulaEdit()).MustHaveHappened();
      }
   }

   internal class When_updating_the_display_unit : concern_for_EditIndividualParameterPresenter
   {
      private Unit _unit;

      protected override void Context()
      {
         base.Context();
         _individualParameter.Value = NaN;
         _individualParameter.Formula = new ConstantFormula(0);
         sut.Edit(_individualParameter, _buildingBlock);

         _unit = A.Fake<Unit>();
      }

      protected override void Because()
      {
         sut.UpdateUnit(_unit);
      }

      [Observation]
      public void the_interaction_task_sets_the_value()
      {
         A.CallTo(() => _interactionTasksForIndividualBuildingBlock.SetUnit(_buildingBlock, _individualParameter, _unit)).MustHaveHappened();
      }
   }

   internal class When_converting_to_formula_from_value : concern_for_EditIndividualParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         _individualParameter.Value = 0;
         sut.Edit(_individualParameter, _buildingBlock);

         A.CallTo(() => _interactionTasksForIndividualBuildingBlock.SetFormula(_buildingBlock, _individualParameter, A<IFormula>._)).Invokes(x => _individualParameter.Formula = x.Arguments.Get<IFormula>(2));
      }

      protected override void Because()
      {
         sut.ConvertToFormula();
      }

      [Observation]
      public void the_view_should_reveal_the_formula_editor()
      {
         A.CallTo(() => _view.ShowFormulaEdit()).MustHaveHappened();
      }

      [Observation]
      public void the_formula_task_is_used_to_create_the_formula()
      {
         A.CallTo(() => _formulaTask.CreateNewFormulaInBuildingBlock(typeof(ExplicitFormula), _individualParameter.Dimension, A<IEnumerable<string>>._, _buildingBlock, null)).MustHaveHappened();
      }
   }

   internal class When_navigating_to_the_individual_parameter : concern_for_EditIndividualParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_individualParameter, _buildingBlock);
      }

      protected override void Because()
      {
         sut.NavigateToParameter();
      }

      [Observation]
      public void the_edit_task_opens_the_editor()
      {
         A.CallTo(() => _editTask.Edit(_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_event_publisher_publishes_selection_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<EntitySelectedEvent>.That.Matches(x => x.ObjectBase.Equals(_individualParameter)))).MustHaveHappened();
      }
   }
}
