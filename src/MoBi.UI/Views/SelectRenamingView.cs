using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectRenamingView : BaseModalView, ISelectRenamingView
   {
      private readonly GridViewBinder<SelectStringChangeDTO> _gridViewBinder;
      private IGridViewBoundColumn<SelectStringChangeDTO, bool> _colShouldRename;
      private ISelectRenamingPresenter _presenter;

      public SelectRenamingView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<SelectStringChangeDTO>(grdRenamings);
      }

      public void AttachPresenter(ISelectRenamingPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         grdRenamings.EndGrouping += (o, e) => grdRenamings.ExpandAllGroups();
         grdRenamings.GroupFormat = "[#image]{1}";

         _colShouldRename = _gridViewBinder.Bind(x => x.Selected)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN);

         var colBuildingBlock = _gridViewBinder.Bind(x => x.BuildingBlock)
            .WithCaption(AppConstants.Captions.ContainingBuildingBlock)
            .AsReadOnly();

         colBuildingBlock.XtraColumn.GroupIndex = 0;

         _gridViewBinder.Bind(x => x.Description)
            .WithCaption(AppConstants.Captions.DependentRename)
            .AsReadOnly();

         chkShouldRename.CheckedChanged += (o, e) => OnEvent(onShouldRenameChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.RenameWizardCaption;
         chkShouldRename.Text = AppConstants.Captions.ShouldRenameDependentObjects;
      }

      public void SetData(IEnumerable<SelectStringChangeDTO> dtos, bool renameDependentObjectsDefault)
      {
         RenameDefault = renameDependentObjectsDefault;
         _gridViewBinder.BindToSource(dtos);
      }

      public bool RenameDefault
      {
         get { return chkShouldRename.Checked; }
         set
         {
            chkShouldRename.Checked = value;
            updateColumnRenameEnableState();
         }
      }

      private void updateColumnRenameEnableState()
      {
         _colShouldRename.ReadOnly = !chkShouldRename.Checked;
      }

      private void onShouldRenameChanged()
      {
         _presenter.SetCheckedStateForAll(chkShouldRename.Checked);
         updateColumnRenameEnableState();
      }
   }
}