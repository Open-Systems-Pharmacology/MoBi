using MoBi.Core.Services;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISearchView : IView<ISearchPresenter>
   {
      void Start(SearchOptions options);
      bool Enabled { get; set; }
      void ClearResults();
   }
}