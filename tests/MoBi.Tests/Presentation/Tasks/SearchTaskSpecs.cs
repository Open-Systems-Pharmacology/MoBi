using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_SearchTaskSpecs : ContextSpecification<ISearchTask>
   {
      protected ISearchVisitor _searchVisitor;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _searchVisitor = A.Fake<ISearchVisitor>();
         _context = A.Fake<IMoBiContext>();
         sut = new SearchTask(_searchVisitor,_context);
      }
   }

   class When_Start_search_is_called_for_a_building_block: concern_for_SearchTaskSpecs
   {
      private IObjectBase _buildingBlock;
      private SearchOptions _options;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _options = new SearchOptions(){CaseSensitive = false,Expression = "*",Scope = SearchScope.Local,RegEx = true, WholeWord = true};
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _buildingBlock);
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitiv.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_buildingBlock,A<IMoBiProject>._)).MustHaveHappened();
      }
   }

   class When_Start_search_is_called_for_whole_Project: concern_for_SearchTaskSpecs
   {
      private IObjectBase _buildingBlock;
      private SearchOptions _options;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _project = A.Fake<IMoBiProject>();
         _options = new SearchOptions() { CaseSensitive = false, Expression = "*", Scope = SearchScope.Project, RegEx = true, WholeWord = true };
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _buildingBlock);
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitiv.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_project,_project)).MustHaveHappened();
      }
   }

   class When_Start_search_is_called_for_all_building_blocks_of_one_Type : concern_for_SearchTaskSpecs
   {
      private IMoleculeStartValuesBuildingBlock _buildingBlock;
      private SearchOptions _options;
      private IMoBiProject _project;
      private MoleculeStartValuesBuildingBlock _otherBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoleculeStartValuesBuildingBlock();
         _project = A.Fake<IMoBiProject>();
         _options = new SearchOptions() { CaseSensitive = false, Expression = "*", Scope = SearchScope.AllOfSameType, RegEx = true, WholeWord = true };
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         _otherBuildingBlock = new MoleculeStartValuesBuildingBlock();
         A.CallTo(() => _project.MoleculeStartValueBlockCollection).Returns(new []{_buildingBlock,_otherBuildingBlock});  
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _buildingBlock);
      }

      [Observation]
      public void should_retrieve_all_building_blocks_of_same_type()
      {
         A.CallTo(() => _project.MoleculeStartValueBlockCollection).MustHaveHappened();  
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitiv.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_buildingBlock,_project)).MustHaveHappened();
         A.CallTo(() => _searchVisitor.SearchIn(_otherBuildingBlock,_project)).MustHaveHappened();

      }
   }

   class When_Start_search_is_called_for_all_simuzlations : concern_for_SearchTaskSpecs
   {
      private IMoBiSimulation _simulation;
      private SearchOptions _options;
      private IMoBiProject _project;
      private IMoBiSimulation _otherSimulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _project = A.Fake<IMoBiProject>();
         _options = new SearchOptions() { CaseSensitive = false, Expression = "*", Scope = SearchScope.AllOfSameType, RegEx = true, WholeWord = true };
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         _otherSimulation = A.Fake<IMoBiSimulation>(); 
         A.CallTo(() => _project.Simulations).Returns(new []{_simulation,_otherSimulation});  
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _simulation);
      }

      [Observation]
      public void should_retrieve_all_building_blocks_of_same_type()
      {
         A.CallTo(() => _project.Simulations).MustHaveHappened();  
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitiv.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_simulation,A<IMoBiProject>._)).MustHaveHappened();
         A.CallTo(() => _searchVisitor.SearchIn(_otherSimulation, A<IMoBiProject>._)).MustHaveHappened();

      }
   }

   class When_Start_search_is_called_for_a_simulation : concern_for_SearchTaskSpecs
   {
      private IMoBiSimulation _simulation;
      private SearchOptions _options;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _options = new SearchOptions() { CaseSensitive = false, Expression = "*", Scope = SearchScope.Local, RegEx = true, WholeWord = true };
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _simulation);
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitiv.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_simulation, A<IMoBiProject>._)).MustHaveHappened();
      }
   }
}	