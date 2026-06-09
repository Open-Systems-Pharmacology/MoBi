using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ReactionListView : BaseUserControl, IReactionListView, IRootViewItem<ReactionBuilder>, IViewWithPopup
   {
      private GridViewBinder<ReactionInfoDTO> _gridViewBinder;
      private IReactionsListSubPresenter _presenter;

      public ReactionListView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder = new GridViewBinder<ReactionInfoDTO>(grdViewReactionList);
         _gridViewBinder.Bind(x => x.Name).WithCaption("Reaction").AsReadOnly();
         _gridViewBinder.Bind(x => x.StoichiometricFormula).WithCaption("Stoichiometric Formula").AsReadOnly();
         _gridViewBinder.Bind(x => x.Kinetic).WithCaption("Kinetic").AsReadOnly();
         grdViewReactionList.FocusedRowChanged += (o, e) => OnEvent(() => selectReaction(_gridViewBinder.FocusedElement));
      }

      public void AttachPresenter(IReactionsListSubPresenter presenter)
      {
         _presenter = presenter;
      }

      public BarManager PopupBarManager
      {
         get { return barManager; }
      }

      public void Show(IEnumerable<ReactionInfoDTO> reactions)
      {
         _gridViewBinder.BindToSource(reactions);
      }

      private void reactionListMouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         if (_gridViewBinder.FocusedElement == null)
         {
            _presenter.CreatePopupMenuFor<IViewItem>(this).At(e.X, e.Y);
         }
         else
         {
            _presenter.CreatePopupMenuFor(_gridViewBinder.FocusedElement).At(e.X, e.Y);
         }
      }

      private void selectReaction(ReactionInfoDTO reaction)
      {
         if (reaction == null) return;

         _presenter.Select(reaction.Id);
      }
   }
}