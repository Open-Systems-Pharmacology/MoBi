using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public enum ValuePointColumn
   {
      X = 0,
      Y = 1
   }

   public interface IEditTableFormulaPresenter : IEditTypedFormulaPresenter, IListener<AddedValuePointEvent>, IListener<RemovedValuePointEvent>, IListener<TableFormulaUnitChangedEvent>, IListener<TableFormulaValueChangedEvent>, IListener<TableFormulaRestartSolverChangedEvent>
   {
      void AddValuePoint();
      void RemoveValuePoint(DTOValuePoint valuePointDTO);
      void SetUseDerivedValuesFor(TableFormulaBuilderDTO dtoTableFormula, bool newValue, bool oldValue);
      void SetXUnit(Unit unit);
      void SetYUnit(Unit unit);
      void SetXValue(DTOValuePoint valuePointDTO, double newValue);
      void SetYValue(DTOValuePoint valuePointDTO, double newValue);

      IEnumerable<Unit> AvailableUnitsFor(ValuePointColumn column);
      Unit UnitFor(ValuePointColumn column);
      void SetUnit(ValuePointColumn column, Unit newUnit);
      void SetRestartSolver(DTOValuePoint valuePoint, bool newRestartSolverValue);
      bool ShouldEnableDelete();
   }

   public class EditTableFormulaPresenter : EditTypedFormulaPresenter<IEditTableFormulaView, IEditTableFormulaPresenter, TableFormula>, IEditTableFormulaPresenter
   {
      private readonly ITableFormulaToDTOTableFormulaMapper _tableFormulaToDTOTableFormulaMapper;
      private readonly IMoBiContext _context;
      private readonly ITableFormulaTask _tableFormulaTask;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public EditTableFormulaPresenter(IEditTableFormulaView view, ITableFormulaToDTOTableFormulaMapper tableFormulaToDTOTableFormulaMapper, IMoBiContext context, ITableFormulaTask tableFormulaTask,
         IMoBiApplicationController applicationController, IMoBiFormulaTask moBiFormulaTask, IDisplayUnitRetriever displayUnitRetriever)
         : base(view, displayUnitRetriever)
      {
         _tableFormulaToDTOTableFormulaMapper = tableFormulaToDTOTableFormulaMapper;
         _context = context;
         _tableFormulaTask = tableFormulaTask;
         _applicationController = applicationController;
         _moBiFormulaTask = moBiFormulaTask;
      }

      public override void Edit(TableFormula objectToEdit)
      {
         _formula = objectToEdit;
         _view.Show(_tableFormulaToDTOTableFormulaMapper.MapFrom(_formula));
      }

      public bool ShouldEnableDelete()
      {
         return _formula.AllPoints().Count() > 1;
      }

      public void AddValuePoint()
      {
         using (var getValuePointPresenter = _applicationController.Start<INewValuePointPresenter>())
         {
            getValuePointPresenter.Dimension = _formula.Dimension;
            ValuePoint newValuePoint = getValuePointPresenter.GetNewValuePoint();

            if (newValuePoint != null)
            {
               AddCommand(_moBiFormulaTask.AddValuePoint(_formula, newValuePoint, BuildingBlock));
            }
         }
      }

      public void RemoveValuePoint(DTOValuePoint valuePointDTO)
      {
         var valuePoint = valuePointDTO.ValuePoint;
         AddCommand(_moBiFormulaTask.RemoveValuePointFromTableFormula(_formula, valuePoint, BuildingBlock));
      }

      public void SetRestartSolver(DTOValuePoint valuePoint, bool newRestartSolverValue)
      {
         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetRestartSolver(_formula, valuePoint.ValuePoint, newRestartSolverValue, BuildingBlock).Run(_context))
         );
      }

      public void SetUseDerivedValuesFor(TableFormulaBuilderDTO dtoTableFormula, bool newValue, bool oldValue)
      {
         AddCommand(_moBiFormulaTask.EditUseDerivedValues(_formula, newValue, oldValue, BuildingBlock));
      }

      public void SetXUnit(Unit unit)
      {
         // Do not latch because modifying the unit should rebind the whole table
         AddCommand(_tableFormulaTask.SetXUnit(_formula, unit, BuildingBlock).Run(_context));
      }

      public void SetXValue(DTOValuePoint valuePointDTO, double newValue)
      {
         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetXValuePoint(_formula, valuePointDTO.ValuePoint, newValue, BuildingBlock).Run(_context))
         );
      }

      public void SetYUnit(Unit unit)
      {
         // Do not latch because modifying the unit should rebind the whole table
         AddCommand(_tableFormulaTask.SetYUnit(_formula, unit, BuildingBlock).Run(_context));
      }

      public void SetYValue(DTOValuePoint valuePointDTO, double newValue)
      {
         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetYValuePoint(_formula, valuePointDTO.ValuePoint, newValue, BuildingBlock).Run(_context))
         );
      }

      public IEnumerable<Unit> AvailableUnitsFor(ValuePointColumn column)
      {
         if (column == ValuePointColumn.X)
            return _formula.XDimension.Units;

         return _formula.Dimension.Units;
      }

      public Unit UnitFor(ValuePointColumn column)
      {
         if (column == ValuePointColumn.X)
            return _formula.XDisplayUnit;

         return _formula.YDisplayUnit;
      }

      public void SetUnit(ValuePointColumn column, Unit newUnit)
      {
         if (column == ValuePointColumn.X)
            SetXUnit(newUnit);
         else
            SetYUnit(newUnit);
      }

      public void Handle(TableFormulaUnitChangedEvent eventToHandle)
      {
         handleTableFormulaEvent(eventToHandle);
      }

      public void Handle(AddedValuePointEvent eventToHandle)
      {
         handleTableFormulaEvent(eventToHandle);
      }

      private bool canHandle(TableFormulaEvent eventToHandle)
      {
         return !IsLatched && Equals(eventToHandle.TableFormula, _formula);
      }

      private void handleTableFormulaEvent(TableFormulaEvent eventToHandle)
      {
         if (!canHandle(eventToHandle)) return;
         Edit(_formula);
      }

      public void Handle(RemovedValuePointEvent eventToHandle)
      {
         handleTableFormulaEvent(eventToHandle);
      }

      public void Handle(TableFormulaValueChangedEvent eventToHandle)
      {
         handleTableFormulaEvent(eventToHandle);
      }

      public void Handle(TableFormulaRestartSolverChangedEvent eventToHandle)
      {
         handleTableFormulaEvent(eventToHandle);
      }
   }
}