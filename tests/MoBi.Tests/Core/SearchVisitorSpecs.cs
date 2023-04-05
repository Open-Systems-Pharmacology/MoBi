using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;

namespace MoBi.Core
{
   public abstract class concern_for_SearchVisitor : ContextSpecification<LocalSearchVisitor>
   {
      protected IObjectBase _testObject;
      protected string _searchExpression;

      protected override void Context()
      {
         _searchExpression = "look for";
         _testObject = new Parameter();
         _testObject.Name = "test";
         _testObject.Description = "Nothing to read her";
         sut = new LocalSearchVisitor(_searchExpression);
      }
   }

   internal class When_Visiting_an_object_named_like_the_search_expression : concern_for_SearchVisitor
   {
      protected override void Context()
      {
         base.Context();
         _testObject.Name = _searchExpression;
      }

      protected override void Because()
      {
         sut.SearchIn(_testObject, A.Fake<IObjectBase>());
      }

      [Observation]
      public void should_add_test_object_to_result_collection()
      {
         sut.Result.Select(x => x.FoundObject).ShouldContain(_testObject);
      }
   }

   internal class When_Visiting_an_object_with_description_that_contains_the_search_expression : concern_for_SearchVisitor
   {
      protected override void Context()
      {
         base.Context();
         _testObject.Description = string.Format("What do you {0}?", _searchExpression);
      }

      protected override void Because()
      {
         sut.SearchIn(_testObject, A.Fake<IObjectBase>());
      }

      [Observation]
      public void should_add_test_object_to_result_collection()
      {
         sut.Result.Select(x => x.FoundObject).ShouldContain(_testObject);
      }
   }

   internal class When_Visiting_an_object_that_has_nothing_todo_with_the_search_expression : concern_for_SearchVisitor
   {
      protected override void Because()
      {
         sut.SearchIn(_testObject, A.Fake<IObjectBase>());
      }

      [Observation]
      public void should_add_test_object_to_result_collection()
      {
         sut.Result.Select(x => x.FoundObject).Contains(_testObject).ShouldBeFalse();
      }
   }

   internal class When_searching_an_object_with_case_insensitive_match_and_case_sensitve_setting : concern_for_SearchVisitor
   {
      protected override void Context()
      {
         base.Context();
         sut = new LocalSearchVisitor(_searchExpression.ToUpper());
         sut.CaseSensitive = true;
      }

      protected override void Because()
      {
         sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_return_empty_results()
      {
         sut.Result.ShouldBeEmpty();
      }
   }

   public abstract class concern_for_SearchVisitorComplexTest : ContextSpecification<ISearchVisitor>
   {
      protected IObjectBase _testObject;
      protected string _searchExpression;
      protected Container _subContainer1;
      protected Container _subContainer2;
      protected Parameter _p1;
      protected Parameter _p2;
      protected Parameter _p3;
      protected Parameter _p4;
      public IContainer _root;

      protected override void Context()
      {
         _searchExpression = "look for";
         _root = new Container().WithName("test").WithDescription("Nothing to read her");
         _testObject = new ModelCoreSimulation{Model = new Model() {Root = _root}};
         _subContainer1 = new Container().WithName(_searchExpression.ToUpper()).WithDescription("TOOL");
         _subContainer2 = new Container().WithName(_searchExpression + "not").WithDescription("TOOL");
         _root.Add(_subContainer1);
         _root.Add(_subContainer2);
         _p1 = new Parameter().WithName("P1").WithDescription(string.Format("What do you {0}?", _searchExpression));
         _p2 = new Parameter().WithName(_searchExpression);
         _subContainer1.Add(_p1);
         _subContainer1.Add(_p2);
         _p3 = new Parameter().WithName("P3").WithDescription("P§");
         _p4 = new Parameter().WithName("p4").WithDescription("_____" + _searchExpression);
         _subContainer2.Add(_p3);
         _subContainer2.Add(_p4);
         sut = new SearchVisitor();
         sut.SearchFor = _searchExpression;
      }
   }

   internal class When_searching_a_container_structure : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_right_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_subContainer1, _subContainer2, _p1, _p2, _p4);
      }
   }

   internal class When_searching_a_container_structure_for_whole_word : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Context()
      {
         base.Context();
         sut.WholeWord = true;
      }

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_right_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_subContainer1, _p1, _p2);
      }
   }

   internal class When_searching_a_container_structure_with_all_regEx_pattern : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Context()
      {
         base.Context();
         sut.SearchFor = ".*";
         sut.RegExSearch = true;
      }

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_allobjects_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_root, _subContainer1, _subContainer2, _p1, _p2, _p3, _p4);
      }
   }

   internal class When_searching_a_container_structure_with_CaseSensitveSetting : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Context()
      {
         base.Context();
         sut.CaseSensitive = true;
      }

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_allobjects_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_subContainer2, _p1, _p2, _p4);
      }
   }

   internal class When_searching_a_container_structure_with_regEx_pattern_start_with_P_case_insensitive : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Context()
      {
         base.Context();
         sut.SearchFor = "P.*";
         sut.RegExSearch = true;
      }

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_allobjects_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_p1, _p3, _p4);
      }
   }

   internal class When_searching_a_container_structure_with_regEx_pattern_start_with_P_case_sensitive : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Context()
      {
         base.Context();
         sut.SearchFor = "P.*";
         sut.RegExSearch = true;
         sut.CaseSensitive = true;
      }

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_allobjects_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_p1, _p3);
      }
   }

   internal class When_searching_a_container_structure_for_whole_word_and_case_sensetive : concern_for_SearchVisitorComplexTest
   {
      private IEnumerable<SearchResult> _results;

      protected override void Context()
      {
         base.Context();
         sut.WholeWord = true;
         sut.CaseSensitive = true;
      }

      protected override void Because()
      {
         _results = sut.SearchIn(_testObject, A.Fake<MoBiProject>());
      }

      [Observation]
      public void should_find_the_allobjects_objects()
      {
         _results.Select(x => x.FoundObject).ShouldOnlyContain(_p1, _p2);
      }
   }
}