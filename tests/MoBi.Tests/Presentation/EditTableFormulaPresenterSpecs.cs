using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTableFormulaPresenter : ContextSpecification<EditTableFormulaPresenter>
   {
      private IDisplayUnitRetriever _displayUnitRetriever;
      protected IMoBiFormulaTask _moBiFormulaTask;
      protected IMoBiApplicationController _applicationController;
      protected ITableFormulaTask _tableFormulaTask;
      private IMoBiContext _context;
      protected ITableFormulaToDTOTableFormulaMapper _tableFormulaToDTOTableFormulaMapper;
      protected IEditTableFormulaView _view;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         _view = A.Fake<IEditTableFormulaView>();
         _tableFormulaToDTOTableFormulaMapper = A.Fake<ITableFormulaToDTOTableFormulaMapper>();
         _context = A.Fake<IMoBiContext>();
         _tableFormulaTask = A.Fake<ITableFormulaTask>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();

         sut = new EditTableFormulaPresenter(_view, _tableFormulaToDTOTableFormulaMapper, _context, _tableFormulaTask, _applicationController, _moBiFormulaTask, _displayUnitRetriever);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_changing_the_restart_solver_state : concern_for_EditTableFormulaPresenter
   {
      private DTOValuePoint _valuePointDTO;
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _valuePointDTO = new DTOValuePoint(A.Fake<TableFormulaBuilderDTO>());
         var valuePoint = new ValuePoint(0, 0) { RestartSolver = false };
         _valuePointDTO.ValuePoint = valuePoint;
         _valuePointDTO.RestartSolver = false;
         _tableFormula = new TableFormula();
         sut.Edit(_tableFormula);
      }

      protected override void Because()
      {
         sut.SetRestartSolver(_valuePointDTO, true);
      }

      [Observation]
      public void the_task_should_be_used_to_update_the_value_point()
      {
         A.CallTo(() => _tableFormulaTask.SetRestartSolver(_tableFormula, _valuePointDTO.ValuePoint, true, A<IBuildingBlock>._)).MustHaveHappened();
      }
   }

   public class when_removing_a_point_from_the_table_formula : concern_for_EditTableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private DTOValuePoint _removedPointDTO;
      private ValuePoint _valuePoint;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(0, 0);
         _valuePoint = new ValuePoint(1, 1);
         _tableFormula.AddPoint(_valuePoint);
         sut.Edit(_tableFormula);
         _removedPointDTO = new DTOValuePoint(A.Fake<TableFormulaBuilderDTO>()) { ValuePoint = _valuePoint };
      }

      protected override void Because()
      {
         sut.RemoveValuePoint(_removedPointDTO);
      }

      [Observation]
      public void the_task_is_used_to_remove_the_point_from_the_table()
      {
         A.CallTo(() => _moBiFormulaTask.RemoveValuePointFromTableFormula(_tableFormula, _valuePoint, A<BuildingBlock>._)).MustHaveHappened();
      }
   }

   public class When_adding_a_point_to_the_table_formula : concern_for_EditTableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private INewValuePointPresenter _newValuePointPresenter;
      private ValuePoint _valuePoint;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _newValuePointPresenter = A.Fake<INewValuePointPresenter>();
         A.CallTo(() => _applicationController.Start<INewValuePointPresenter>()).Returns(_newValuePointPresenter);
         _valuePoint = new ValuePoint(0, 0);
         A.CallTo(() => _newValuePointPresenter.GetNewValuePoint()).Returns(_valuePoint);
         sut.Edit(_tableFormula);
      }

      protected override void Because()
      {
         sut.AddValuePoint();
      }

      [Observation]
      public void the_new_point_is_added_by_the_task_to_the_formula()
      {
         A.CallTo(() => _moBiFormulaTask.AddValuePoint(_tableFormula, _valuePoint, A<IBuildingBlock>._)).MustHaveHappened();
      }

      [Observation]
      public void the_application_controller_starts_the_new_value_presenter()
      {
         A.CallTo(() => _applicationController.Start<INewValuePointPresenter>()).MustHaveHappened();
      }
   }

   public class When_editing_a_table_formula : concern_for_EditTableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private TableFormulaBuilderDTO _tableFormulaBuilderDTO;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormulaBuilderDTO = A.Fake<TableFormulaBuilderDTO>();
         _tableFormula.AddPoint(0, 0);
         _tableFormula.AddPoint(1, 1);
         A.CallTo(() => _tableFormulaToDTOTableFormulaMapper.MapFrom(_tableFormula)).Returns(_tableFormulaBuilderDTO);
      }

      protected override void Because()
      {
         sut.Edit(_tableFormula);
      }

      [Observation]
      public void the_view_should_bind_to_the_dto_from_the_mapper()
      {
         A.CallTo(() => _view.Show(_tableFormulaBuilderDTO)).MustHaveHappened();
      }

      [Observation]
      public void The_table_formula_to_table_formula_dto_mapper_is_used_to_create_dtos_for_the_view()
      {
         A.CallTo(() => _tableFormulaToDTOTableFormulaMapper.MapFrom(_tableFormula)).MustHaveHappened();
      }
   }

   public class When_checking_whether_deletes_should_be_allowed_and_multiple_points_in_the_formula_table : concern_for_EditTableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(0, 0);
         _tableFormula.AddPoint(1, 1);
         sut.Edit(_tableFormula);
      }

      [Observation]
      public void the_delete_should_not_be_allowed()
      {
         sut.ShouldEnableDelete().ShouldBeTrue();
      }
   }

   public class When_checking_whether_deletes_should_be_allowed_and_only_one_point_in_the_formula_table : concern_for_EditTableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(0, 0);
         sut.Edit(_tableFormula);
      }

      [Observation]
      public void the_delete_should_not_be_allowed()
      {
         sut.ShouldEnableDelete().ShouldBeFalse();
      }
   }
}
