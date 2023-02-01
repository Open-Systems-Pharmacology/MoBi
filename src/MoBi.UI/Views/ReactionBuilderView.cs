using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public abstract partial class ReactionBuilderView<TReactionPartnerBuilder> : BaseUserControl, IReactionBuilderView<TReactionPartnerBuilder> where TReactionPartnerBuilder : IViewItem
   {
      protected GridViewBinder<TReactionPartnerBuilder> _gridBinder;
      protected IReactionPartnerPresenter<TReactionPartnerBuilder> _presenter;
      private readonly UxAddAndRemoveButtonRepository _addRemoveButtonRepository = new UxAddAndRemoveButtonRepository();

      protected ReactionBuilderView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
         _gridBinder = new GridViewBinder<TReactionPartnerBuilder>(gridView);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         gridView.AllowsFiltering = false;
      }

      public void AttachPresenter(IReactionPartnerPresenter<TReactionPartnerBuilder> presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IReadOnlyList<TReactionPartnerBuilder> partnerBuilders)
      {
         _gridBinder.BindToSource(partnerBuilders);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         gridControl.MouseClick += (o, e) => OnEvent(() => GridPartnerMouseClick(_gridBinder, e));

         _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _addRemoveButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH * 2);

         _addRemoveButtonRepository.ButtonClick += (o, e) => OnEvent(() => onButtonClicked(e, _gridBinder.FocusedElement));
      }

      private void onButtonClicked(ButtonPressedEventArgs buttonPressedEventArgs, TReactionPartnerBuilder reactionPartnerBuilderDTO)
      {
         var pressedButton = buttonPressedEventArgs.Button;
         if (pressedButton.Kind.Equals(ButtonPredefines.Plus))
            _presenter.AddNewReactionPartnerBuilder();
         else
            _presenter.Remove(reactionPartnerBuilderDTO);
      }

      protected void GridPartnerMouseClick(GridViewBinder<TReactionPartnerBuilder> gridViewBinder, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;

         if (gridView.CalcHitInfo(e.Location).InDataRow)
            _presenter.CreatePopupMenuFor(gridViewBinder.FocusedElement).At(PointToClient(Cursor.Position));
         else
            _presenter.CreatePopupMenuFor(null).At(PointToClient(Cursor.Position));
      }

      public override bool HasError => base.HasError || _gridBinder.HasError;

      public BarManager PopupBarManager => barManager;
   }
}