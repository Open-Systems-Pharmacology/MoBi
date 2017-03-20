using System.Windows.Forms;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;

namespace MoBi.UI.Views
{
   public partial class EditNeighborhoodView : EditContainerView, IEditNeighborhoodView
   {
      private XtraTabPage tabTransport;
      private GridControl grdTransportsControl;
      private GridView grdTransports;
      private GridViewBinder<TransportDTO> _gridBinderTranport;

      private void initTranportPage()
      {
         tabTransport = tabPagesControl.TabPages.Add("Transports");
         grdTransportsControl = new GridControl();
         grdTransports = (GridView) grdTransportsControl.CreateView(typeof (GridView).Name);
         grdTransportsControl.MainView = grdTransports;
         grdTransports.GridControl = grdTransportsControl;
         tabTransport.Controls.Add(grdTransportsControl);
         grdTransportsControl.Dock = DockStyle.Fill;
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_CreatingNeighborhoods;

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridBinderTranport = new GridViewBinder<TransportDTO>(grdTransports);
         _gridBinderTranport.Bind(dto => dto.Name).AsReadOnly();
         _gridBinderTranport.Bind(dto => dto.Molecule).AsReadOnly();
         _gridBinderTranport.Bind(dto => dto.Source).WithCaption("From").AsReadOnly();
         _gridBinderTranport.Bind(dto => dto.Target).WithCaption("To").AsReadOnly();
         _gridBinderTranport.Bind(dto => dto.Rate).AsReadOnly();
      }

      public override void BindTo(ContainerDTO dto)
      {
         base.BindTo(dto);
         var dtoNeighborhood = (NeighborhoodDTO) dto;
         _gridBinderTranport.BindToSource(dtoNeighborhood.Transports);
      }
   }
}