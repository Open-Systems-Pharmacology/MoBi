using System;
using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;

namespace MoBi.Presentation.Presenter.Main
{
   public interface ISearchPresenter : IListener<ProjectClosedEvent>, 
      IListener<ProjectLoadedEvent>, IMainViewItemPresenter
   {
      IEnumerable<string> GetScopeNames();
      IEnumerable<SearchScope> GetScopes();
      IEnumerable<SearchResultDTO> StartSearch(SearchOptions options);
      void Select(SearchResultDTO focusedElement);
   }

   public class SearchPresenter : AbstractPresenter<ISearchView, ISearchPresenter>, ISearchPresenter
   {
      private readonly IRegionResolver _regionResolver;
      private IRegion _region;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly ISearchResultToDTOSearchResultMapper _searchResultsToDTOSearchResultsMapper;
      private readonly ISearchTasks _searchTask;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiContext _context;

      public SearchPresenter(ISearchView view, IRegionResolver regionResolver, IActiveSubjectRetriever activeSubjectRetriever,
         ISearchResultToDTOSearchResultMapper searchResultsToDTOSearchResultsMapper, ISearchTasks searchTask, IMoBiApplicationController applicationController, IMoBiContext context) : base(view)
      {
         _regionResolver = regionResolver;
         _context = context;
         _applicationController = applicationController;
         _searchTask = searchTask;
         _searchResultsToDTOSearchResultsMapper = searchResultsToDTOSearchResultsMapper;
         _activeSubjectRetriever = activeSubjectRetriever;
         _view.Enabled = false;
      }

      public void ToggleVisibility()
      {
         _region.ToggleVisibility();
      }

      public override void Initialize()
      {
         _region = _regionResolver.RegionWithName(RegionNames.Search);
         _region.Add(_view);
         _view.Start(new SearchOptions());
      }

      public IEnumerable<SearchResultDTO> StartSearch(SearchOptions options)
      {
         var activeBuildingBlock = _activeSubjectRetriever.Active<IObjectBase>();
         return _searchTask.StartSearch(options, activeBuildingBlock).MapAllUsing(_searchResultsToDTOSearchResultsMapper);
      }

      public void Select(SearchResultDTO focusedElement)
      {
         if (focusedElement == null) return;
         _applicationController.Select(focusedElement.Object, focusedElement.ProjectItem, _context.HistoryManager);
      }

      public IEnumerable<string> GetScopeNames()
      {
         return Enum.GetNames(typeof (SearchScope));
      }

      public IEnumerable<SearchScope> GetScopes()
      {
         return EnumHelper.AllValuesFor<SearchScope>();
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         _view.Enabled = false;
         _view.ClearResults();
      }

      public void Handle(ProjectLoadedEvent eventToHandle)
      {
         _view.Enabled = true;
      }
   }
}