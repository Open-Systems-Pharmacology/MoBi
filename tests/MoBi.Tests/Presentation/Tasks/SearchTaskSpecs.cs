using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Repositories;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_SearchTaskSpecs : ContextSpecification<ISearchTask>
   {
      protected ISearchVisitor _searchVisitor;
      private ISimulationRepository _simulationRepository;
      private IBuildingBlockRepository _buildingBlockRepository;
      private IMoBiProjectRetriever _moBiProjectRetriever;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _searchVisitor = A.Fake<ISearchVisitor>();
         _context = A.Fake<IMoBiContext>();
         _moBiProjectRetriever = new MoBiProjectRetriever(_context);
         _buildingBlockRepository = new BuildingBlockRepository(_moBiProjectRetriever);
         _simulationRepository = new SimulationRepository(_moBiProjectRetriever);
         sut = new SearchTask(_searchVisitor, _moBiProjectRetriever, _buildingBlockRepository, _simulationRepository);
      }
   }

   class When_Start_search_is_called_for_a_building_block : concern_for_SearchTaskSpecs
   {
      private IObjectBase _buildingBlock;
      private SearchOptions _options;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _options = new SearchOptions() { CaseSensitive = false, Expression = "*", Scope = SearchScope.Local, RegEx = true, WholeWord = true };
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _buildingBlock);
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitive.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_buildingBlock, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
      }
   }

   class When_Start_search_is_called_for_whole_Project : concern_for_SearchTaskSpecs
   {
      private IObjectBase _buildingBlock;
      private SearchOptions _options;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _project = A.Fake<MoBiProject>();
         _options = new SearchOptions { CaseSensitive = false, Expression = "*", Scope = SearchScope.Project, RegEx = true, WholeWord = true };
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _buildingBlock);
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitive.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_project, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
      }
   }

   class When_Start_search_is_called_for_all_building_blocks_of_one_Type : concern_for_SearchTaskSpecs
   {
      private InitialConditionsBuildingBlock _buildingBlock;
      private SearchOptions _options;
      private MoBiProject _project;
      private InitialConditionsBuildingBlock _otherBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new InitialConditionsBuildingBlock();
         _project = DomainHelperForSpecs.NewProject();
         _options = new SearchOptions() { CaseSensitive = false, Expression = "*", Scope = SearchScope.AllOfSameType, RegEx = true, WholeWord = true };
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         _otherBuildingBlock = new InitialConditionsBuildingBlock();

         var module = new Module()
         {
            _buildingBlock,
            _otherBuildingBlock
         };

         _project.AddModule(module);
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _buildingBlock);
      }

      [Observation]
      public void should_retrieve_all_building_blocks_of_same_type()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_buildingBlock, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
         A.CallTo(() => _searchVisitor.SearchIn(_otherBuildingBlock, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitive.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_buildingBlock, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
         A.CallTo(() => _searchVisitor.SearchIn(_otherBuildingBlock, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
      }
   }

   class When_Start_search_is_called_for_all_simuzlations : concern_for_SearchTaskSpecs
   {
      private IMoBiSimulation _simulation;
      private SearchOptions _options;
      private MoBiProject _project;
      private IMoBiSimulation _otherSimulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _project = DomainHelperForSpecs.NewProject();
         _options = new SearchOptions { CaseSensitive = false, Expression = "*", Scope = SearchScope.AllOfSameType, RegEx = true, WholeWord = true };
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         _otherSimulation = A.Fake<IMoBiSimulation>();

         _project.AddSimulation(_simulation);
         _project.AddSimulation(_otherSimulation);
      }

      protected override void Because()
      {
         sut.StartSearch(_options, _simulation);
      }

      [Observation]
      public void should_retrieve_all_building_blocks_of_same_type()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_simulation, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_have_set_the_options_of_the_search_visitor()
      {
         _searchVisitor.CaseSensitive.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_simulation, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
         A.CallTo(() => _searchVisitor.SearchIn(_otherSimulation, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
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
         _searchVisitor.CaseSensitive.ShouldBeEqualTo(_options.CaseSensitive);
         _searchVisitor.RegExSearch.ShouldBeEqualTo(_options.RegEx);
         _searchVisitor.SearchFor.ShouldBeEqualTo(_options.Expression);
         _searchVisitor.WholeWord.ShouldBeEqualTo(_options.WholeWord);
      }

      [Observation]
      public void should_search_in_the_buildingBlock()
      {
         A.CallTo(() => _searchVisitor.SearchIn(_simulation, A<IReadOnlyList<IBuildingBlock>>._)).MustHaveHappened();
      }
   }
}