using System.Collections.Generic;
using System.Text.RegularExpressions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Services
{
   public interface ISearchVisitor : IVisitor<IBuildingBlock>, IVisitor<IModelCoreSimulation>, IVisitor<MoBiProject>
   {
      string SearchFor { get; set; }
      bool WholeWord { get; set; }
      bool RegExSearch { get; set; }
      IEnumerable<SearchResult> Result { get; }
      bool CaseSensitive { get; set; }
      IEnumerable<SearchResult> SearchIn(IObjectBase searchTarget, IReadOnlyList<IBuildingBlock> buildingBlocks);
   }

   class SearchVisitor : ISearchVisitor
   {
      private List<SearchResult> _result;
      private string _searchExpression;
      private bool _searchExpressionCreated;
      private IObjectBase _projectItem;
      private LocalSearchVisitor _localVisitor;
      private IEnumerable<IBuildingBlock> _allBuildingBlocks;

      public SearchVisitor()
      {
         _result = new List<SearchResult>();
      }

      private string getSearchExpression()
      {
         if (!_searchExpressionCreated)
         {
            _searchExpression = createSearchExpressionFrom(SearchFor);
         }

         return _searchExpression;
      }

      private string createSearchExpressionFrom(string searchFor)
      {
         string searchExpression = searchFor;
         if (!CaseSensitive)
         {
            searchExpression = searchExpression.ToLower();
         }

         if (!RegExSearch)
         {
            searchExpression = Regex.Escape(searchExpression);
         }

         if (WholeWord)
         {
            searchExpression = $"\\b{searchExpression}\\b";
         }

         _searchExpressionCreated = true;
         return searchExpression;
      }

      public bool CaseSensitive { get; set; }
      public string SearchFor { get; set; }
      public bool WholeWord { get; set; }
      public bool RegExSearch { get; set; }

      public IEnumerable<SearchResult> Result => _result;

      public IEnumerable<SearchResult> SearchIn(IObjectBase searchTarget, IReadOnlyList<IBuildingBlock> buildingBlocks)
      {
         try
         {
            _result = new List<SearchResult>();
            _allBuildingBlocks = buildingBlocks;
            _searchExpressionCreated = false;
            _localVisitor = new LocalSearchVisitor(getSearchExpression()) {CaseSensitive = CaseSensitive};
            searchTarget.AcceptVisitor(this);
            return Result;
         }
         finally
         {
            _projectItem = null;
            _localVisitor = null;
            _allBuildingBlocks = null;
         }
      }

      public void Visit(IBuildingBlock objToVisit)
      {
         _projectItem = objToVisit;
         if (!_allBuildingBlocks.ContainsItem(objToVisit)) return;

         searchObjectBase(objToVisit, _projectItem);
      }

      private void searchObjectBase(IObjectBase objToVisit, IObjectBase projectItem)
      {
         _result.AddRange(_localVisitor.SearchIn(objToVisit, projectItem));
      }

      public void Visit(IModelCoreSimulation objToVisit)
      {
         _projectItem = objToVisit;
         searchObjectBase(objToVisit.Model, _projectItem);
      }

      public void Visit(MoBiProject objToVisit)
      {
         // Do Nothing don't check the Project it self.
      }
   }

   public class LocalSearchVisitor : IVisitor<IObjectBase>
   {
      private readonly string _searchExpression;
      private IObjectBase _projectItem;
      private List<SearchResult> _result;
      public bool CaseSensitive { get; set; }

      public IList<SearchResult> Result => _result;

      public LocalSearchVisitor(string searchExpression)
      {
         _searchExpression = searchExpression;
      }

      public IList<SearchResult> SearchIn(IObjectBase objectBase, IObjectBase projectItem)
      {
         _result = new List<SearchResult>();
         _projectItem = projectItem;
         objectBase.AcceptVisitor(this);
         return _result;
      }

      public void Visit(IObjectBase objToVisit)
      {
         searchObjectBase(objToVisit);
      }

      private void searchObjectBase(IObjectBase objToVisit)
      {
         if (searchForIsIn(objToVisit.Name))
         {
            _result.Add(new SearchResult(objToVisit, _projectItem));
         }
         else
         {
            if (searchForIsIn(objToVisit.Description))
            {
               _result.Add(new SearchResult(objToVisit, _projectItem));
            }
         }
      }

      private bool searchForIsIn(string lookIn)
      {
         if (lookIn.IsNullOrEmpty())
            return false;

         if (CaseSensitive)
            return Regex.Matches(lookIn, _searchExpression).Count > 0;

         return Regex.Matches(lookIn.ToLower(), _searchExpression).Count > 0;
      }
   }
}