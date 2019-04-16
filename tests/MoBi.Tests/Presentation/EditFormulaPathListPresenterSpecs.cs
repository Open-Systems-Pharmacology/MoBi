using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFormulaPathListPresenter : ContextSpecification<IEditFormulaPathListPresenter>
   {
      protected IEditFormulaPathListView _view;
      protected IMoBiFormulaTask _moBiFormulaTask;
      protected IMoBiContext _context;
      protected IDimensionFactory _dimensionFactory;
      protected IUserSettings _userSettings;
      protected IViewItemContextMenuFactory _contextMenuFactory;
      protected ICircularReferenceChecker _circularReferenceChecker;
      protected IFormulaUsablePathToFormulaUsablePathDTOMapper _formulaUsablePathDTOMapper;

      protected override void Context()
      {
         _view = A.Fake<IEditFormulaPathListView>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _context = A.Fake<IMoBiContext>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _userSettings = A.Fake<IUserSettings>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _circularReferenceChecker = A.Fake<ICircularReferenceChecker>();
         _formulaUsablePathDTOMapper = A.Fake<IFormulaUsablePathToFormulaUsablePathDTOMapper>();

         sut = new EditFormulaPathListPresenter(_view, _moBiFormulaTask, _context, _dimensionFactory, _userSettings, _contextMenuFactory, _circularReferenceChecker, _formulaUsablePathDTOMapper);
      }
   }

   internal class When_adding_a_ReferencePath : concern_for_EditFormulaPathListPresenter
   {
      private ExplicitFormula _formula;
      private IFormulaUsablePath _newFormulaPath;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         sut.Edit(_formula, null);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         _formula.AddObjectPath(new FormulaUsablePath());
         A.CallTo(() => _dimensionFactory.Dimension(_userSettings.ParameterDefaultDimension)).Returns(DomainHelperForSpecs.AmountDimension);
         A.CallTo(() => _moBiFormulaTask.AddFormulaUsablePath(_formula, A<IFormulaUsablePath>._, A<IBuildingBlock>._))
            .Invokes(x => _newFormulaPath = x.GetArgument<IFormulaUsablePath>(1));
      }

      protected override void Because()
      {
         sut.CreateNewPath();
      }

      [Observation]
      public void should_delegate_to_the_formula_task_to_add_a_new_path_to_the_formula()
      {
         _newFormulaPath.ShouldNotBeNull();
      }

      [Observation]
      public void the_dimension_is_set_to_user_default_dimension()
      {
         _newFormulaPath.Dimension.ShouldBeEqualTo(DomainHelperForSpecs.AmountDimension);
      }

      [Observation]
      public void the_name_is_unique()
      {
         // started the test with an existing path with empty alias, the new one
         // will have the old name with a "1" appended
         _newFormulaPath.Alias.ShouldBeEqualTo("1");
      }
   }

   internal class When_cloning_a_ReferencePath : concern_for_EditFormulaPathListPresenter
   {
      private ExplicitFormula _formula;
      private IFormulaUsablePath _formulaUsablePath;
      private FormulaUsablePathDTO _formulaUsablePathDTO;
      private ICommandCollector _commandCollector;
      private IFormulaUsablePath _newFormulaPath;

      protected override void Context()
      {
         base.Context();
         _commandCollector = A.Fake<ICommandCollector>();
         _formula = new ExplicitFormula();
         _formulaUsablePath = new FormulaUsablePath("path") {Alias = "alias", Dimension = DimensionFactoryForSpecs.MassDimension};
         _formula.AddObjectPath(_formulaUsablePath);

         var formulaUsablePath = new FormulaUsablePath("path") {Alias = "alias"};
         _formulaUsablePathDTO = new FormulaUsablePathDTO(formulaUsablePath, _formula);
         A.CallTo(() => _formulaUsablePathDTOMapper.MapFrom(_formula, A<IUsingFormula>._)).Returns(new[] {_formulaUsablePathDTO});
         sut.Edit(_formula, null);
         sut.InitializeWith(_commandCollector);

         A.CallTo(() => _moBiFormulaTask.AddFormulaUsablePath(_formula, A<IFormulaUsablePath>._, A<IBuildingBlock>._))
            .Invokes(x => _newFormulaPath = x.GetArgument<IFormulaUsablePath>(1));
      }

      protected override void Because()
      {
         sut.ClonePath(_formulaUsablePathDTO);
      }

      [Observation]
      public void the_newly_added_path_should_be_a_clone_of_the_original_with_unique_alias()
      {
         _newFormulaPath.PathAsString.ShouldBeEqualTo(_formulaUsablePath.PathAsString);
         _newFormulaPath.Dimension.ShouldBeEqualTo(_formulaUsablePath.Dimension);
         _newFormulaPath.Alias.ShouldNotBeEqualTo(_formulaUsablePath.Alias);
      }
   }

   internal class When_removing_a_ReferencePath : concern_for_EditFormulaPathListPresenter
   {
      private FormulaUsablePathDTO _formulaUsablePathDTO;
      private ExplicitFormula _formula;
      private IFormulaUsablePath _pathToRemove;
      private IMoBiCommand _removeCommand;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula().WithName("Test");
         _pathToRemove = new FormulaUsablePath("..", "ToRemove") {Alias = "ToRemove"};
         _formulaUsablePathDTO = new FormulaUsablePathDTO(_pathToRemove, _formula);
         _formula.AddObjectPath(_pathToRemove);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         sut.Edit(_formula, null);

         _removeCommand = A.Fake<IMoBiCommand>();
         A.CallTo(() => _moBiFormulaTask.RemoveFormulaUsablePath(_formula, _pathToRemove, A<IBuildingBlock>._)).Returns(_removeCommand);
      }

      protected override void Because()
      {
         sut.RemovePath(_formulaUsablePathDTO);
      }

      [Observation]
      public void should_ask_for_remove_command()
      {
         A.CallTo(() => _moBiFormulaTask.RemoveFormulaUsablePath(_formula, _pathToRemove, A<IBuildingBlock>._)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_command_to_the_history()
      {
         A.CallTo(() => sut.CommandCollector.AddCommand(_removeCommand)).MustHaveHappened();
      }
   }
}