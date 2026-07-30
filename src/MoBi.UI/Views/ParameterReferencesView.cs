using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ParameterReferencesView : BaseModalView, IParameterReferencesView
   {
      private readonly GridViewBinder<ParameterReferenceDTO> _gridViewBinder;
      private IParameterReferencesPresenter _presenter;

      public ParameterReferencesView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ParameterReferenceDTO>(gridView);
         gridView.OptionsBehavior.Editable = false;
         gridView.OptionsView.ShowGroupPanel = false;
         gridView.PopupMenuShowing += OnPopupMenuShowing;
         gridControl.MouseDoubleClick += (o, e) => OnEvent(() => onDoubleClick(e));
         btnBack.Click += (o, e) => OnEvent(() => _presenter.GoBack());
      }

      public void AttachPresenter(IParameterReferencesPresenter presenter) => _presenter = presenter;

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.Bind(x => x.UsedBy)
            .WithCaption(AppConstants.Captions.UsedBy)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Type)
            .WithCaption(AppConstants.Captions.Type)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.UsedAs)
            .WithCaption(AppConstants.Captions.UsedAs)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Alias)
            .WithCaption(AppConstants.Captions.Alias)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.FormulaName)
            .WithCaption(AppConstants.Captions.FormulaName)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Formula)
            .WithCaption(AppConstants.Captions.Formula)
            .AsReadOnly();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         CancelVisible = false;
         ExtraVisible = true;
         ExtraCaption = AppConstants.MenuNames.GoTo;
         btnBack.InitWithImage(ApplicationIcons.Back, AppConstants.Captions.Back);
         layoutControlItemBack.AdjustButtonSize(layoutControl);

         // This aligns the breadcrumb text with the taller back button in the same row
         layoutControlItemBack.ContentVertAlignment = VertAlignment.Center;
         layoutControlItemBreadcrumb.ContentVertAlignment = VertAlignment.Center;
      }

      protected override void ExtraClicked() => _presenter.GoTo(_gridViewBinder.FocusedElement);

      private void onDoubleClick(MouseEventArgs e)
      {
         var reference = _gridViewBinder.ElementAt(gridView.RowHandleAt(e));
         if (reference == null)
            return;

         _presenter.FindReferencesFor(reference);
      }

      public void UpdateNavigation(string breadcrumb, bool canGoBack)
      {
         lblBreadcrumb.Text = breadcrumb;
         btnBack.Enabled = canGoBack;
      }

      private void OnPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.HitInfo.HitTest != GridHitTest.RowIndicator || !e.Menu.Items.Any() || gridView.GetSelectedRows().Length != 1)
            return;

         //first menu entry: by convention this is the action also triggered by double click
         if (_presenter.CanFindReferencesFor(_gridViewBinder.FocusedElement))
            e.Menu.Items.Insert(0, findReferencesMenuItem());
      }

      private DXMenuItem findReferencesMenuItem()
      {
         return new DXMenuItem(
            AppConstants.Captions.FindReferences,
            (s, args) => findReferences(),
            ApplicationIcons.Search.ToImage()
         );
      }

      private void findReferences() => _presenter.FindReferencesFor(_gridViewBinder.FocusedElement);

      public void BindTo(IReadOnlyList<ParameterReferenceDTO> references)
      {
         _gridViewBinder.BindToSource(references);
         ExtraEnabled = references.Any();
         gridView.BestFitColumns();
      }
   }
}
