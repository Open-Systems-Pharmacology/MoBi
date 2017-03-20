using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using OSPSuite.Assets;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditPassiveTransportBuildingBlockView : EditBuildingBlockBaseView, IEditPassiveTransportBuildingBlockView, IViewWithPopup
   {
      private GridViewBinder<TransportBuilderDTO> _gridBinder;

      public EditPassiveTransportBuildingBlockView(IMainView mainView, IImageListRetriever imageListRetriever) : base(mainView)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_PassiveTransports;

      public override void InitializeResources()
      {
         base.InitializeResources();
         splitContainerControl1.CollapsePanel= SplitCollapsePanel.Panel1;
         EditCaption = AppConstants.Captions.PassiveTransports;
      }

      public override void InitializeBinding()
      {
         _gridBinder = new GridViewBinder<TransportBuilderDTO>(gridView);
         _gridBinder.Bind(dto => dto.Name).AsReadOnly();

         _gridBinder.Bind(dto => dto.Formula)
            .WithRepository(createFormulaComboboxRepositoryItem)
            .WithShowInColumnChooser(true)
            .AsHidden();

         gridView.MouseDown += (o, e) => this.DoWithinExceptionHandler(() => onGridViewMouseDown(e));
      }

      private RepositoryItem createFormulaComboboxRepositoryItem(TransportBuilderDTO dtoObserverBuilder)
      {
         var comboBox = new UxRepositoryItemComboBox(gridView);
         comboBox.FillComboBoxRepositoryWith(editPassiveTransportBuildingBlockPresenter.GetFormulas());
         return comboBox;
      }

      public void AttachPresenter(IEditPassiveTransportBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IEnumerable<TransportBuilderDTO> dtoPassiveTransportBuilders)
      {
         gridView.FocusedRowChanged -= selectionChanged;
         _gridBinder.BindToSource(dtoPassiveTransportBuilders);
         gridView.FocusedRowChanged += selectionChanged;
      }

      public void SetEditView(IView view)
      {
         splitContainerControl1.Panel2.FillWith(view);
      }

      public void ClearEditView()
      {
         splitContainerControl1.Panel2.Clear();
      }

      private void selectionChanged(object sender, FocusedRowChangedEventArgs e)
      {
         this.DoWithinExceptionHandler(() => editPassiveTransportBuildingBlockPresenter.Select(_gridBinder.ElementAt(e.FocusedRowHandle)));
      }

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         var rowHandle = gridView.RowHandleAt(e);
         editPassiveTransportBuildingBlockPresenter.ShowContextMenu(_gridBinder.ElementAt(rowHandle), e.Location);
      }

      public BarManager PopupBarManager
      {
         get { return barManager; }
      }

      private IEditPassiveTransportBuildingBlockPresenter editPassiveTransportBuildingBlockPresenter
      {
         get { return Presenter.DowncastTo<IEditPassiveTransportBuildingBlockPresenter>(); }
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.PassiveTransport; }
      }
   }
}