using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditTableFormulaPresenter : ContextSpecification<EditTableFormulaPresenter>
   {
      private IDisplayUnitRetriever _displayUnitRetriever;
      protected IMoBiFormulaTask _moBiFormulaTask;
      protected IMoBiApplicationController _applicationController;
      protected IEditTableFormulaView _view;
      private ICommandCollector _commandCollector;
      protected IMoBiTableFormulaPresenter _tableFormulaPresenter;
      protected ISimpleChartPresenter _simpleChartPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditTableFormulaView>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _tableFormulaPresenter = A.Fake<IMoBiTableFormulaPresenter>();
         _simpleChartPresenter = A.Fake<ISimpleChartPresenter>();

         sut = new EditTableFormulaPresenter(_view, _displayUnitRetriever, _tableFormulaPresenter, _simpleChartPresenter);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
      }
   }

   public class When_editing_a_table_formula : concern_for_EditTableFormulaPresenter
   {
      private TableFormula _tableFormula;

      protected override void Context()
      {
         base.Context();
         _tableFormula = new TableFormula();
         _tableFormula.AddPoint(0, 0);
         _tableFormula.AddPoint(1, 1);
         sut.BuildingBlock = new SpatialStructure();
      }

      protected override void Because()
      {
         sut.Edit(_tableFormula);
      }

      [Observation]
      public void the_sub_presenters_should_be_used_to_edit()
      {
         A.CallTo(() => _simpleChartPresenter.Plot(_tableFormula)).MustHaveHappened();
         A.CallTo(() => _tableFormulaPresenter.Edit(_tableFormula, sut.BuildingBlock)).MustHaveHappened();
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