using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.HelpersForTests;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.Parameters;

namespace MoBi.Presentation
{
   public abstract class concern_for_TableFormulaPresenter : ContextSpecification<MoBiTableFormulaPresenter>
   {
      protected ITableFormulaView _view;
      protected ITableFormulaTask _tableFormulaTask;
      protected IMoBiFormulaTask _moBiFormulaTask;
      protected IMoBiContext _context;
      protected SpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _view = A.Fake<ITableFormulaView>();
         _tableFormulaTask = A.Fake<ITableFormulaTask>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _context = A.Fake<IMoBiContext>();
         _spatialStructure = new SpatialStructure();
         sut = new MoBiTableFormulaPresenter(_view, _tableFormulaTask, _moBiFormulaTask, _context);
         sut.InitializeWith(_context.HistoryManager);
      }

      public class When_editing_the_table_formula : concern_for_TableFormulaPresenter
      {
         private TableFormula _tableFormula;

         protected override void Context()
         {
            base.Context();
            _tableFormula = new TableFormula();
         }

         protected override void Because()
         {
            sut.Edit(_tableFormula, _spatialStructure);
         }

         [Observation]
         public void the_view_should_be_configured()
         {
            A.CallTo(() => _view.ShowUseDerivedValues(true)).MustHaveHappened();
            A.CallTo(() => _view.ShowRestartSolver(true)).MustHaveHappened();
         }
      }

      public class When_setting_the_y_value : concern_for_TableFormulaPresenter
      {
         private ValuePointDTO _valuePointDTO;
         private TableFormula _tableFormula;

         protected override void Context()
         {
            base.Context();
            _tableFormula = new TableFormula();
            var valuePoint = new ValuePoint(0, 0);
            _tableFormula.AddPoint(valuePoint);
            _valuePointDTO = new ValuePointDTO(_tableFormula, valuePoint);
            sut.Edit(_tableFormula, _spatialStructure);
         }

         protected override void Because()
         {
            sut.SetYValue(_valuePointDTO, 5.0);
         }

         [Observation]
         public void the_task_should_be_used_to_update_the_value_point()
         {
            A.CallTo(() => _tableFormulaTask.SetYValuePoint(_tableFormula, _valuePointDTO.ValuePoint, 5.0, A<IBuildingBlock>._)).MustHaveHappened();
         }
      }

      public class When_setting_the_restart_solver : concern_for_TableFormulaPresenter
      {
         private ValuePointDTO _valuePointDTO;
         private TableFormula _tableFormula;

         protected override void Context()
         {
            base.Context();
            _tableFormula = new TableFormula();
            var valuePoint = new ValuePoint(0, 0) { RestartSolver = false };
            _tableFormula.AddPoint(valuePoint);
            _valuePointDTO = new ValuePointDTO(_tableFormula, valuePoint);
            sut.Edit(_tableFormula, _spatialStructure);
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
   }

   public class When_setting_use_derived_values : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         sut.Edit(_tableFormula, _spatialStructure);
      }

      protected override void Because()
      {
         sut.SetUseDerivedValues(true);
      }

      [Observation]
      public void the_task_should_be_used_to_update_the_property()
      {
         A.CallTo(() => _moBiFormulaTask.EditUseDerivedValues(_tableFormula, true, _tableFormula.UseDerivedValues, A<IBuildingBlock>._)).MustHaveHappened();
      }
   }

   public class When_importing_a_table_formula : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private DataRepository _importedTablePoints;
      protected override void Context()
      {
         base.Context();
         var baseGrid = new BaseGrid("baseGrid", "baseGrid", DimensionFactoryForSpecs.TimeDimension);
         var dataColumn = new DataColumn("column", DimensionFactoryForSpecs.MassDimension, baseGrid)
         {
            Values = new List<float>()
         };

         _importedTablePoints = new DataRepository
         {
            baseGrid,
            dataColumn
         };

         baseGrid.InternalValues.AddRange(new[] {0.0f, 1.0f, 3.0f});
         dataColumn.InternalValues.AddRange(new[] { 0.0f, 1.0f, 3.0f });

         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(0, 0);
         _tableFormula.AddPoint(1, 1);

         sut.Edit(_tableFormula, _spatialStructure);

         A.CallTo(() => _tableFormulaTask.ImportTablePointsFor(_tableFormula)).Returns(_importedTablePoints);
      }

      protected override void Because()
      {
         sut.ImportTable();
      }

      [Observation]
      public void all_points_should_be_removed_by_task()
      {
         A.CallTo(() => _moBiFormulaTask.RemoveValuePointFromTableFormula(_tableFormula, A<ValuePoint>._, _spatialStructure)).MustHaveHappened(_tableFormula.AllPoints.Count, Times.Exactly);
      }

      [Observation]
      public void new_points_should_be_added_by_task()
      {
         A.CallTo(() => _moBiFormulaTask.AddValuePoint(_tableFormula, A<ValuePoint>._, _spatialStructure)).MustHaveHappened(_importedTablePoints.BaseGrid.Count, Times.Exactly);
      }
   }

   public class When_changing_the_restart_solver_state : concern_for_TableFormulaPresenter
   {
      private ValuePointDTO _valuePointDTO;
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         var valuePoint = new ValuePoint(0, 0) { RestartSolver = false };
         _tableFormula.AddPoint(valuePoint);
         _valuePointDTO = new ValuePointDTO(_tableFormula, valuePoint);
         _tableFormula.AddPoint(valuePoint);
         sut.Edit(_tableFormula, _spatialStructure);
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

   public class When_removing_a_point_from_the_table_formula : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private ValuePointDTO _removedPointDTO;
      private ValuePoint _valuePoint;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(0, 0);
         _valuePoint = new ValuePoint(1, 1);
         _tableFormula.AddPoint(_valuePoint);
         sut.Edit(_tableFormula, _spatialStructure);
         _removedPointDTO = new ValuePointDTO(_tableFormula, _valuePoint);
      }

      protected override void Because()
      {
         sut.RemovePoint(_removedPointDTO);
      }

      [Observation]
      public void the_task_is_used_to_remove_the_point_from_the_table()
      {
         A.CallTo(() => _moBiFormulaTask.RemoveValuePointFromTableFormula(_tableFormula, _valuePoint, A<BuildingBlock>._)).MustHaveHappened();
      }
   }

   public class When_changing_the_x_arg_for_a_point_not_in_the_table : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private ValuePoint _valuePoint;
      private TableFormulaDTO _dto;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _valuePoint = new ValuePoint(0, 0);
         _tableFormula.AddPoint(_valuePoint);
         A.CallTo(() => _view.BindTo(A<TableFormulaDTO>._)).Invokes(x => { _dto = x.Arguments.Get<TableFormulaDTO>(0); });
         sut.Edit(_tableFormula, _spatialStructure);
         sut.AddPoint();
      }

      protected override void Because()
      {
         sut.SetXValue(_dto.AllPoints.ToArray()[1], 5.0);
      }

      [Observation]
      public void the_point_is_added_when_valid()
      {
         A.CallTo(() => _moBiFormulaTask.AddValuePoint(_tableFormula, _dto.AllPoints.ToArray()[1].ValuePoint, A<BuildingBlock>._)).MustHaveHappened();
      }
   }

   public class When_changing_the_x_arg_for_a_point_in_the_table : concern_for_TableFormulaPresenter
   {
      private TableFormula _tableFormula;
      private ValuePoint _valuePoint;
      private TableFormulaDTO _dto;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _valuePoint = new ValuePoint(0, 0);
         _tableFormula.AddPoint(_valuePoint);
         A.CallTo(() => _view.BindTo(A<TableFormulaDTO>._)).Invokes(x => _dto = x.Arguments.Get<TableFormulaDTO>(0));
         sut.Edit(_tableFormula, _spatialStructure);
      }

      protected override void Because()
      {
         sut.SetXValue(_dto.AllPoints.First(), 5.0);
      }

      [Observation]
      public void the_point_is_updated_by_the_task()
      {
         A.CallTo(() => _tableFormulaTask.SetXValuePoint(_tableFormula, _valuePoint, 5.0, A<BuildingBlock>._)).MustHaveHappened();
      }
   }
}