using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Parameters;
using OSPSuite.Presentation.Views.Parameters;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.Presenter
{
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

      public MoBiTableFormulaPresenter(ITableFormulaView view, ITableFormulaTask tableFormulaTask, IMoBiFormulaTask formulaTask, IMoBiContext context) : base(view)
      {
         _tableFormulaTask = tableFormulaTask;
         _formulaTask = formulaTask;
         _context = context;
      }

      protected override DataRepository ImportTablePoints()
      {
         return _tableFormulaTask.ImportTablePointsFor(_editedFormula);
      }

      protected override void ApplyImportedTablePoints(DataRepository importedTablePoints)
      {
         var macroCommand = new MoBiMacroCommand
         {
            Description = "Import table formula",
            CommandType = AppConstants.Commands.EditCommand,
            ObjectType = new ObjectTypeResolver().TypeFor<TableFormula>()
         };

         macroCommand.AddRange(_editedFormula.AllPoints.ToList().Select(removeCommand));
         macroCommand.AddRange(importedTablePoints.BaseGrid.Values.Select(x => addCommand(new ValuePoint(x, importedTablePoints.AllButBaseGrid().Single().GetValue(x)))));

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
            AddCommand(_tableFormulaTask.SetXValuePoint(_editedFormula, valuePointDTO.ValuePoint, newValue, _buildingBlock).RunCommand(_context))
         );
      }

      private IMoBiCommand addCommand(ValuePoint valuePoint)
      {
         return _formulaTask.AddValuePoint(_editedFormula, valuePoint, _buildingBlock);
      }

      public override void SetRestartSolver(ValuePointDTO valuePointDTO, bool restart)
      {
         this.DoWithinLatch(() =>
            AddCommand(_tableFormulaTask.SetRestartSolver(_editedFormula, valuePointDTO.ValuePoint, restart, _buildingBlock).RunCommand(_context))
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
            AddCommand(_tableFormulaTask.SetYValuePoint(_editedFormula, valuePointDTO.ValuePoint, newValue, _buildingBlock).RunCommand(_context))
         );
      }

      public override void RemovePoint(ValuePointDTO valuePointDTO)
      {
         AddCommand(removeCommand(valuePointDTO.ValuePoint));
      }

      private IMoBiCommand removeCommand(ValuePoint valuePoint)
      {
         return _formulaTask.RemoveValuePointFromTableFormula(_editedFormula, valuePoint, _buildingBlock);
      }

      public bool IsLatched { get; set; }
   }
}