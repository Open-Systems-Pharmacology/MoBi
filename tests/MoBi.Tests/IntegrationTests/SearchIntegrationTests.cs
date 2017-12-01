using System.Linq;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_SearchIntegrationTests : ContextWithLoadedProject
   {
      protected ISearchTask _searchTask;
      protected SearchOptions _searchOptions;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _searchTask = IoC.Resolve<ISearchTask>();
      }
   }

   public class When_performing_a_search_query_for_the_diclofenac_project : concern_for_SearchIntegrationTests
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         LoadProject("PK_Manual_Diclofenac");
         _searchOptions = new SearchOptions
         {
            Scope = SearchScope.Project,
            Expression = "Molecular Weight"
         };
      }

      [Observation]
      public void should_return_the_expected_result_when_querying_for_molecular_weight()
      {
         _searchOptions.CaseSensitive = false;
         var searchResults = _searchTask.StartSearch(_searchOptions, null)
            .GroupBy(x => x.ProjectItem);

         searchResults.Count().ShouldBeGreaterThan(0);
      }

      [Observation]
      public void should_return_the_expected_result_when_querying_for_molecular_weight_case_sensitive()
      {
         _searchOptions.CaseSensitive = true;
         var searchResults = _searchTask.StartSearch(_searchOptions, null);
         searchResults.ShouldBeEmpty();
      }
   }

   public class When_performing_a_search_query_for_the_manual_model_sim_project : concern_for_SearchIntegrationTests
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         LoadProject("ManualModel_Sim");
         _searchOptions = new SearchOptions
         {
            Scope = SearchScope.Project,
         };
      }

      [Observation]
      public void should_return_the_expected_result_when_querying_with_whole_word_and_case_sensitive()
      {
         _searchOptions.Expression = "A";
         _searchOptions.CaseSensitive = true;
         _searchOptions.WholeWord = true;
         var searchResults = _searchTask.StartSearch(_searchOptions, null)
            .GroupBy(x => x.ProjectItem);

         searchResults.Count().ShouldBeGreaterThan(0);
      }

      [Observation]
      public void should_return_the_expected_result_when_querying_for_molecular_weight_case_sensitive()
      {
         _searchOptions.Expression = "PGP+";
         _searchOptions.RegEx = true;
         var searchResults = _searchTask.StartSearch(_searchOptions, null);
         searchResults.Count().ShouldBeGreaterThan(0);
      }
   }

}