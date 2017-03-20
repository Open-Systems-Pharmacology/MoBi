using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class FormulaCacheView : BaseUserControl, IFormulaCacheView, IViewWithPopup
   {
      private IFormulaCachePresenter _presenter;
      private GridViewBinder<FormulaBuilderDTO> _gridBinder;
      private readonly BarManager _barManager;

      public FormulaCacheView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _barManager = new BarManager {Form = this, Images = imageListRetriever.AllImages16x16};
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_AdditionalFeaturesReactionEditing;

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridBinder = new GridViewBinder<FormulaBuilderDTO>(grdFormulaList);
         _gridBinder.Bind(dto => dto.Name).AsReadOnly();
         _gridBinder.Bind(dto => dto.FormulaType)
            .WithCaption(AppConstants.Captions.FormulaType)
            .AsReadOnly();
         _gridBinder.Bind(dto => dto.Dimension).AsReadOnly();
         var buttonRepository = createAddRemoveButtonRepository();

         _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => buttonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         grdFormulaList.FocusedRowChanged += (o, e) => OnEvent(() => _presenter.Select(_gridBinder.ElementAt(e.FocusedRowHandle)));
         grdFormulaList.MouseDown += (o, e) => OnEvent(onGridViewMouseDown,e);
         buttonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.Remove(_gridBinder.FocusedElement));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         splitContainerControl1.CollapsePanel= SplitCollapsePanel.Panel2;
      }

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         var rowHandle = grdFormulaList.RowHandleAt(e);
         _presenter.ShowContextMenu(_gridBinder.ElementAt(rowHandle), e.Location);
      }

   
      private RepositoryItemButtonEdit createAddRemoveButtonRepository()
      {
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.HideTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Delete;
         buttonRepository.Buttons[0].ToolTip = ToolTips.FormulaList.DeleteFormula;
         return buttonRepository;
      }

      public void Show(IEnumerable<FormulaBuilderDTO> dtos)
      {
         _gridBinder.BindToSource(dtos);
      }

      public void SetEditView(IView subView)
      {
         splitContainerControl1.Panel2.FillWith(subView);
      }

      public void Select(FormulaBuilderDTO dtoFormulaBuilder)
      {
         var rowHandle = _gridBinder.RowHandleFor(dtoFormulaBuilder);
         grdFormulaList.FocusedRowHandle = rowHandle;
      }

      public void AttachPresenter(IFormulaCachePresenter presenter)
      {
         _presenter = presenter;
      }

      public BarManager PopupBarManager
      {
         get { return _barManager; }
      }
   }
}