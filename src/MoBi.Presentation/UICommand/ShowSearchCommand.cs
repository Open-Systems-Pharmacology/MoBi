using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
   public class ShowSearchCommand : IUICommand
   {
      private readonly ISearchPresenter _searchPresenter;

      public ShowSearchCommand(ISearchPresenter searchPresenter)
      {
         _searchPresenter = searchPresenter;
      }

      public void Execute()
      {
         _searchPresenter.ToggleVisibility();
      }
   }
}