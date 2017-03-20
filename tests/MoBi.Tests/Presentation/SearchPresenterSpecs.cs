using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Regions;


namespace MoBi.Presentation
{
   public abstract class concern_for_SearchPresenterSpecs : ContextSpecification<ISearchPresenter>
   {
      protected IMoBiContext _context;
      protected IMoBiApplicationController _applicationController;
      private ISearchTasks _searchTask;
      private ISearchResultToDTOSearchResultMapper _searchResultMapper;
      private IActiveSubjectRetriever _activeSubjectRetriever;
      private IRegionResolver _regionResolver;
      private ISearchView _view;

      protected override void Context()
      {
         _view = A.Fake<ISearchView>();
         _regionResolver = A.Fake<IRegionResolver>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _searchResultMapper = A.Fake<ISearchResultToDTOSearchResultMapper>();
         _searchTask = A.Fake<ISearchTasks>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _context = A.Fake<IMoBiContext>();
         sut = new SearchPresenter(_view, _regionResolver, _activeSubjectRetriever, _searchResultMapper, _searchTask, _applicationController, _context);
      }
   }

   class When_selecting_a_Serch_result : concern_for_SearchPresenterSpecs
   {
      private SearchResultDTO _searchResult;

      protected override void Context()
      {
         base.Context();
         _searchResult = new SearchResultDTO()
            {
               Object = A.Fake<IObjectBase>(),
               Path = "path",
               ProjectItem = A.Fake<IObjectBase>(),
               ProjectItemName = "Name",
               TypeName = "TYpe"
            };
      }

      protected override void Because()
      {
         sut.Select(_searchResult);
      }

      [Observation]
      public void should_call_application_controllers_select_method_with_right_arguments()
      {
         A.CallTo(() => _applicationController.Select(_searchResult.Object,_searchResult.ProjectItem,_context.HistoryManager)).MustHaveHappened();
      }
   }

   class When_selecting_a_null_Serch_result : concern_for_SearchPresenterSpecs
   {
      protected override void Because()
      {
         sut.Select(null);
      }

      [Observation]
      public void should_not_call_application_controllers_select_method()
      {
         A.CallTo(() => _applicationController.Select(A<IObjectBase>._, A<IObjectBase>._, _context.HistoryManager)).MustNotHaveHappened();
      }
   }
}	