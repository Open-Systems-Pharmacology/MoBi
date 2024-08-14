using MoBi.Core.Events;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTableFormulaPresenter : 
      IEditTypedFormulaPresenter, 
      IListener<AddedValuePointEvent>, 
      IListener<RemovedValuePointEvent>, 
      IListener<TableFormulaUnitChangedEvent>, 
      IListener<TableFormulaValueChangedEvent>, 
      IListener<TableFormulaRestartSolverChangedEvent>
   {
      
   }

   public class EditTableFormulaPresenter : EditTypedFormulaPresenter<IEditTableFormulaView, IEditTableFormulaPresenter, TableFormula>, IEditTableFormulaPresenter
   {
      private readonly IMoBiTableFormulaPresenter _tableFormulaPresenter;
      private readonly ISimpleChartPresenter _simpleChartPresenter;

      public EditTableFormulaPresenter(
         IEditTableFormulaView view, 
         IDisplayUnitRetriever displayUnitRetriever, 
         IMoBiTableFormulaPresenter tableFormulaPresenter, 
         ISimpleChartPresenter simpleChartPresenter)
         : base(view, displayUnitRetriever)
      {
         _tableFormulaPresenter = tableFormulaPresenter;
         _simpleChartPresenter = simpleChartPresenter;
         _view.AddTableView(_tableFormulaPresenter.BaseView);
         _view.AddChartView(_simpleChartPresenter.BaseView);

         AddSubPresenters(_tableFormulaPresenter, _simpleChartPresenter);
      }

      public override void Edit(TableFormula objectToEdit)
      {
         _formula = objectToEdit;
         _tableFormulaPresenter.Edit(_formula, BuildingBlock);
         _simpleChartPresenter.Plot(_formula);
      }

      public bool ShouldEnableDelete() => _formula.AllPoints.Count > 1;

      public void Handle(TableFormulaUnitChangedEvent eventToHandle) => handleTableFormulaEvent(eventToHandle);

      public void Handle(AddedValuePointEvent eventToHandle) => handleTableFormulaEvent(eventToHandle);

      private bool canHandle(TableFormulaEvent eventToHandle) => !IsLatched && Equals(eventToHandle.TableFormula, _formula);

      private void handleTableFormulaEvent(TableFormulaEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         Edit(_formula);
      }

      public void Handle(RemovedValuePointEvent eventToHandle) => handleTableFormulaEvent(eventToHandle);

      public void Handle(TableFormulaValueChangedEvent eventToHandle) => handleTableFormulaEvent(eventToHandle);

      public void Handle(TableFormulaRestartSolverChangedEvent eventToHandle) => handleTableFormulaEvent(eventToHandle);
   }
}