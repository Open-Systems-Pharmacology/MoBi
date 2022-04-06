using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using MoBi.Assets;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class SearchView : BaseUserControl, ISearchView
   {
      private ISearchPresenter _presenter;
      private ScreenBinder<SearchOptions> _screenBinder;
      private GridViewBinder<SearchResultDTO> _gridResultBinder;
      private SearchOptions _options;

      public SearchView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISearchPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlSearchTerm.Text = AppConstants.Captions.Search;
         layoutControlSearchScope.Text = AppConstants.Captions.SearchScope;
         layoutControlResult.Text = AppConstants.Captions.SearchResults;
         var searchButton = txtSearchExpression.Properties.Buttons[0];
         searchButton.Kind = ButtonPredefines.Glyph;
         searchButton.ImageOptions.SetImage(ApplicationIcons.Search);
         ckWholeName.Text = AppConstants.Captions.SearchWholeName;
         chkCaseSensitive.Text = AppConstants.Captions.CaseSensitive;
         chkRegExSearch.Text = AppConstants.Captions.SearchRegEx;
         txtSearchExpression.ButtonPressed += btSearchPressed;
         txtSearchExpression.KeyDown += keyDown;
         grdResultControl.MouseDoubleClick += gridDoubleClick;
         gridSearchResult.GroupFormat = "{1}";
      }

      private void keyDown(object sender, KeyEventArgs e)
      {
         if (e.KeyCode.Equals(Keys.Enter))
         {
            search();
            e.Handled = true;
         }
      }

      private void gridDoubleClick(object sender, MouseEventArgs e)
      {
         this.DoWithinExceptionHandler(() => _presenter.Select(_gridResultBinder.FocusedElement));
      }

      private void btSearchPressed(object sender, ButtonPressedEventArgs e)
      {
         search();
      }

      private void search()
      {
         _options.Expression = txtSearchExpression.Text;
         _gridResultBinder.BindToSource(_presenter.StartSearch(_options));
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<SearchOptions>();
         _screenBinder.Bind(options => options.Expression).To(txtSearchExpression);
         _screenBinder.Bind(options => options.Scope).To(cbScope)
            .WithValues(_presenter.GetScopes())
            .AndDisplays(_presenter.GetScopeNames());
         _screenBinder.Bind(options => options.WholeWord).To(ckWholeName);
         _screenBinder.Bind(options => options.RegEx).To(chkRegExSearch).OnValueUpdating += onChangeRegEx;
         _screenBinder.Bind(options => options.CaseSensitive).To(chkCaseSensitive);
         _gridResultBinder = new GridViewBinder<SearchResultDTO>(gridSearchResult);
         var colProjectItem = _gridResultBinder.Bind(dto => dto.ProjectItemName).WithCaption(AppConstants.Captions.ProjectItem).AsReadOnly();
         colProjectItem.XtraColumn.GroupIndex = 0;
         _gridResultBinder.Bind(dto => dto.TypeName).WithCaption(AppConstants.Captions.TypeName).AsReadOnly();
         _gridResultBinder.Bind(dto => dto.Path).WithCaption(AppConstants.Captions.Path).AsReadOnly();
      }

      private void onChangeRegEx(SearchOptions searchOptions, PropertyValueSetEventArgs<bool> e)
      {
         chkCaseSensitive.Enabled = !e.NewValue;
      }

      public void Start(SearchOptions options)
      {
         _options = options;
         _screenBinder.BindToSource(options);
      }

      public void ClearResults()
      {
         _gridResultBinder.DeleteBinding();
      }
   }
}