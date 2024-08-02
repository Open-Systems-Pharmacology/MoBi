using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.Parameters;
using OSPSuite.Presentation.Views.Parameters;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.Presenter
{
   public enum ValuePointColumn
   {
      X = 0,
      Y = 1
   }

   public interface IEditTableFormulaPresenter : IEditTypedFormulaPresenter, IListener<AddedValuePointEvent>, IListener<RemovedValuePointEvent>, IListener<TableFormulaUnitChangedEvent>, IListener<TableFormulaValueChangedEvent>, IListener<TableFormulaRestartSolverChangedEvent>
   {
      Unit UnitFor(ValuePointColumn column);
   }

   public interface IMoBiTableFormulaPresenter : ITableFormulaPresenter
   {
      void Edit(TableFormula tableFormula, IBuildingBlock buildingBlock);
   }

   public class MoBiTableFormulaPresenter : TableFormulaPresenter<ITableFormulaView>, IMoBiTableFormulaPresenter, ILatchable
   {
      private readonly ITableFormulaTask _tableFormulaTask;
      private readonly IMoBiFormulaTask _formulaTask;
      private readonly IMoBiContext _context;
      private IBuildingBlock _buildingBlock;
      

      public MoBiTableFormulaPresenter(ITableFormulaView view, ITableFormulaTask tableFormulaTask, IMoBiFormulaTask formulaTask, IMoBiContext context) : base(view, tableFormulaTask.ImportTableFormula)
      {
         _tableFormulaTask = tableFormulaTask;
         _formulaTask = formulaTask;
         _context = context;
      }

      protected override void ApplyImportedFormula(TableFormula importedFormula)
      {
         var macroCommand = new MoBiMacroCommand
         {
            Description = "Import table formula",
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = new ObjectTypeResolver().TypeFor(importedFormula)
         };

         macroCommand.AddRange(_editedFormula.AllPoints.ToList().Select(removeCommand));
         macroCommand.AddRange(importedFormula.AllPoints.Select(addCommand));

         AddCommand(macroCommand);
      }

      public void Edit(TableFormula tableFormula, IBuildingBlock buildingBlock)
      {
         Edit(tableFormula);
         View.ShowUseDerivedValues(show: true);
         View.ShowRestartSolver(show: true);
         _buildingBlock = buildingBlock;
      }

      public override void AddPoint()
      {
         _tableFormulaDTO.AllPoints.Add(new ValuePointDTO(_editedFormula, new ValuePoint(double.NaN, double.NaN)));
      }

      public override void SetXValue(ValuePointDTO valuePointDTO, double newValue)
      {
         if (shouldAdd(valuePointDTO))
         {
            valuePointDTO.ValuePoint.X = _editedFormula.XBaseValueFor(newValue);
            valuePointDTO.X = valuePointDTO.ValuePoint.X;
            if (valuePointDTO.IsValid())
            {
               this.DoWithinLatch(() =>
                  AddCommand(addCommand(valuePointDTO.ValuePoint))
               );
            }

            return;
         }

         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetXValuePoint(_editedFormula, valuePointDTO.ValuePoint, newValue, _buildingBlock).Run(_context))
         );
      }

      private IMoBiCommand addCommand(ValuePoint valuePoint)
      {
         return _formulaTask.AddValuePoint(_editedFormula, valuePoint, _buildingBlock);
      }

      public override void SetRestartSolver(ValuePointDTO valuePointDTO, bool restart)
      {
         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetRestartSolver(_editedFormula, valuePointDTO.ValuePoint, restart, _buildingBlock).Run(_context))
         );
      }

      public override void SetUseDerivedValues(bool useDerivedValues)
      {
         this.DoWithinLatch(() =>
            AddCommand(_formulaTask.EditUseDerivedValues(_editedFormula, useDerivedValues, _tableFormulaDTO.UseDerivedValues, _buildingBlock))
         );
      }

      private bool shouldAdd(ValuePointDTO valuePointDTO)
      {
         return !_editedFormula.AllPoints.Contains(valuePointDTO.ValuePoint);
      }

      public override void SetYValue(ValuePointDTO valuePointDTO, double newValue)
      {
         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetYValuePoint(_editedFormula, valuePointDTO.ValuePoint, newValue, _buildingBlock).Run(_context))
         );
      }

      public override void RemovePoint(ValuePointDTO valuePointDTO)
      {
         var valuePoint = valuePointDTO.ValuePoint;
         AddCommand(removeCommand(valuePoint));
      }

      private IMoBiCommand removeCommand(ValuePoint valuePoint)
      {
         return _formulaTask.RemoveValuePointFromTableFormula(_editedFormula, valuePoint, _buildingBlock);
      }

      public bool IsLatched { get; set; }
   }

   public class EditTableFormulaPresenter : EditTypedFormulaPresenter<IEditTableFormulaView, IEditTableFormulaPresenter, TableFormula>, IEditTableFormulaPresenter
   {
      private readonly ITableFormulaToDTOTableFormulaMapper _tableFormulaToDTOTableFormulaMapper;
      private readonly IMoBiContext _context;
      private readonly ITableFormulaTask _tableFormulaTask;
      private readonly IMoBiTableFormulaPresenter _tableFormulaPresenter;
      private readonly ISimpleChartPresenter _simpleChartPresenter;

      public EditTableFormulaPresenter(IEditTableFormulaView view, ITableFormulaToDTOTableFormulaMapper tableFormulaToDTOTableFormulaMapper, IMoBiContext context, ITableFormulaTask tableFormulaTask,
         IDisplayUnitRetriever displayUnitRetriever, IMoBiTableFormulaPresenter tableFormulaPresenter, ISimpleChartPresenter simpleChartPresenter)
         : base(view, displayUnitRetriever)
      {
         _tableFormulaToDTOTableFormulaMapper = tableFormulaToDTOTableFormulaMapper;
         _context = context;
         _tableFormulaTask = tableFormulaTask;
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
         _view.BindTo(_tableFormulaToDTOTableFormulaMapper.MapFrom(_formula));
         updatePlot();
      }

      private void updatePlot()
      {
         _simpleChartPresenter.Plot(_formula);
      }

      public bool ShouldEnableDelete()
      {
         return _formula.AllPoints.Count() > 1;
      }

      public void SetXUnit(Unit unit)
      {
         // Do not latch because modifying the unit should rebind the whole table
         AddCommand(_tableFormulaTask.SetXUnit(_formula, unit, BuildingBlock).Run(_context));
      }

      public void SetYUnit(Unit unit)
      {
         // Do not latch because modifying the unit should rebind the whole table
         AddCommand(_tableFormulaTask.SetYUnit(_formula, unit, BuildingBlock).Run(_context));
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
         if (!canHandle(eventToHandle))
            return;

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