using System.Collections.Generic;
using System.Text.RegularExpressions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Domain.Model;
using NHibernate.Util;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface ISearchVisitor : IVisitor<IBuildingBlock>,IVisitor<IModelCoreSimulation>,IVisitor<IMoBiProject>
   {
      string SearchFor { get; set; }
      bool WholeWord { get; set; }
      bool RegExSearch { get; set; }
      IEnumerable<SearchResult> Result { get; }
      bool CaseSensitiv { get; set; }
      IEnumerable<SearchResult> SearchIn(IObjectBase searchTarget,IMoBiProject project);
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
         if (!CaseSensitiv)
         {
            searchExpression = searchExpression.ToLower();
         }
         if(!RegExSearch)
         {
            searchExpression = Regex.Escape(searchExpression);
         }
         if(WholeWord)
         {
            searchExpression = string.Format("\\b{0}\\b", searchExpression);
         }
         _searchExpressionCreated = true;
         return searchExpression;
      }

      public bool CaseSensitiv { get; set; }
      public string SearchFor{get;set;}
      public bool WholeWord { get; set; }
      public bool RegExSearch { get; set; }


      public IEnumerable<SearchResult> Result
      {
         get { return _result; }
      }

      public IEnumerable<SearchResult> SearchIn(IObjectBase searchTarget,IMoBiProject project)
      {
         try
         {
            _result = new List<SearchResult>();
            _allBuildingBlocks = project.AllBuildingBlocks();
            _searchExpressionCreated = false;
            _localVisitor = new LocalSearchVisitor(getSearchExpression()){CaseSensitiv = CaseSensitiv};
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
         searchObjectBase(objToVisit.Model,_projectItem);
      }

      public void Visit(IMoBiProject objToVisit)
      {
            // Do Nothing dont check the Project it self.
      }
   }

   public class LocalSearchVisitor:IVisitor<IObjectBase>
   {
      private string _searchExpression;
      private IObjectBase _projectItem;
      private List<SearchResult> _result;

      public LocalSearchVisitor(string searchExpression)
      {
         _searchExpression = searchExpression;
      }

      public IList<SearchResult> SearchIn(IObjectBase objectBase,IObjectBase projectItem)
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
         if (lookIn.IsNullOrEmpty()) return false;

         if (CaseSensitiv)
         {
            return Regex.Matches(lookIn, _searchExpression).Any();
         }
         else
         {
            return Regex.Matches(lookIn.ToLower(), _searchExpression).Any();
         }
      }

      public bool CaseSensitiv { get; set; }

      public IList<SearchResult> Result
      {
         get { return _result; }
      }
   }
}