using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Views;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class ObserverListView : BaseUserControl, IObserverListView, IViewWithPopup
   {
      private GridViewBinder<ObserverBuilderDTO> _gridBinder;
      private IObserverBuilderListPresenter _presenter;

      public ObserverListView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_Observers;

      public override void InitializeBinding()
      {
         _gridBinder = new GridViewBinder<ObserverBuilderDTO>(gridView);
         _gridBinder.Bind(dto => dto.Name)
            .AsReadOnly();

         _gridBinder.Bind(dto => dto.MonitorString)
            .WithShowInColumnChooser(true)
            .AsReadOnly()
            .AsHidden();

         var colDimension = _gridBinder.Bind(dto => dto.Dimension)
            .WithEditRepository(dto => createDimensionComboBoxRepository())
            .WithShowInColumnChooser(true)
            .AsHidden();

         colDimension.OnValueUpdating += onPropertySet;

         gridView.MouseDown += (o, e) => OnEvent(onGridViewMouseDown,e);
      }

      private void onPropertySet<T>(ObserverBuilderDTO observerBuilder, PropertyValueSetEventArgs<T> e)
      {
         _presenter.SetPropertyValueFromViewFor(observerBuilder, e.PropertyName, e.NewValue, e.OldValue);
      }

      private RepositoryItem createDimensionComboBoxRepository()
      {
         var comboBox = new UxRepositoryItemComboBox(gridView);
         comboBox.FillComboBoxRepositoryWith(_presenter.GetDimensions());
         return comboBox;
      }

      public void AttachPresenter(IObserverBuilderListPresenter presenter)
      {
         _presenter = presenter;
      }

      public BarManager PopupBarManager
      {
         get { return barManager; }
      }

      public void Show(IEnumerable<ObserverBuilderDTO> dtoObserverBuilders)
      {
         gridView.FocusedRowChanged -= selectItem;
         _gridBinder.BindToSource(dtoObserverBuilders.ToList());
         gridView.FocusedRowChanged += selectItem;
      }

      private void selectItem(object sender,FocusedRowChangedEventArgs e)
      {
          this.DoWithinExceptionHandler(()=>_presenter.Select(_gridBinder.ElementAt(e.FocusedRowHandle)));
      }

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         var rowHandle = gridView.RowHandleAt(e);
         _presenter.ShowContextMenu(_gridBinder.ElementAt(rowHandle), e.Location);
      }
   }
}