using System.Collections.Generic;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class SimulationChangesView : BaseUserControl, ISimulationChangesView
   {
      private readonly GridViewBinder<OriginalQuantityValueDTO> _gridViewBinder;
      private IGridViewBoundColumn<OriginalQuantityValueDTO, double?> _currentValueColumn;
      private IGridViewBoundColumn<OriginalQuantityValueDTO, double?> _originalValueColumn;

      public SimulationChangesView()
      {
         InitializeComponent();
         gridLayoutControlItem.TextVisible = true;
         gridLayoutControlItem.TextLocation = Locations.Top;
         gridLayoutControlItem.Text = AppConstants.Captions.SimulationChangesSinceConfiguration;
         _gridViewBinder = new GridViewBinder<OriginalQuantityValueDTO>(gridView);
         _gridViewBinder.BindingMode = BindingMode.OneWay;
         gridView.RowCellStyle += gridViewRowCellStyle;
      }

      private void gridViewRowCellStyle(object sender, RowCellStyleEventArgs e)
      {
         if (e.Column == _currentValueColumn.XtraColumn || e.Column == _originalValueColumn.XtraColumn)
            e.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.Path).AsReadOnly();
         _gridViewBinder.Bind(x => x.Type).AsReadOnly();
         _originalValueColumn = _gridViewBinder.Bind(x => x.OriginalValue).WithFormat(dto => dto.OriginalQuantityValueFormatter(x => x.DisplayUnit)).WithCaption(AppConstants.Captions.OriginalValue).AsReadOnly();
         _currentValueColumn = _gridViewBinder.Bind(x => x.CurrentValue).WithFormat(dto => dto.OriginalQuantityValueFormatter(x => x.DisplayUnit)).WithCaption(AppConstants.Captions.CurrentValue).AsReadOnly();
         _gridViewBinder.Bind(x => x.Dimension).AsReadOnly();
      }

      public void AttachPresenter(ISimulationChangesPresenter presenter)
      {
         // No need for a presenter in this readonly view
      }

      public void BindTo(IReadOnlyList<OriginalQuantityValueDTO> originalQuantityValues)
      {
         _gridViewBinder.BindToSource(originalQuantityValues);
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
      }
   }
}