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
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditFormulaPathListPresenter : ContextSpecification<EditFormulaPathListPresenter>
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
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   internal abstract class When_setting_a_path : concern_for_EditFormulaPathListPresenter
   {
      protected FormulaUsablePathDTO _formulaUsablePathDTO;
      protected string _aNewPath;
      protected IFormula _formula;
      protected IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("5");
         _aNewPath = "a|new|path";
         _parameter = new Parameter();
         _parameter.Formula = _formula;
         _formulaUsablePathDTO = new FormulaUsablePathDTO(new FormulaUsablePath(), _formula);

         A.CallTo(() => _formulaUsablePathDTOMapper.MapFrom(_formula, A<IUsingFormula>._)).Returns(new[] { _formulaUsablePathDTO });
         A.CallTo(() => _circularReferenceChecker.HasCircularReference(A<ObjectPath>._, A<IUsingFormula>._)).Returns(CreatesCircularRef);
         sut.Edit(_formula, _parameter);
      }

      protected abstract bool CreatesCircularRef { get; }
   }

   internal class When_setting_a_path_that_without_circular_reference : When_setting_a_path
   {
      protected override void Because()
      {
         sut.SetFormulaUsablePath(_aNewPath, _formulaUsablePathDTO);
      }

      [Observation]
      public void the_circular_reference_checker_is_used_to_check_for_circular_references()
      {
         A.CallTo(() => _circularReferenceChecker.HasCircularReference(A<ObjectPath>._, _parameter)).MustHaveHappened();
      }

      [Observation]
      public void the_path_is_changed_by_command()
      {
         A.CallTo(() => _moBiFormulaTask.ChangePathInFormula(A<IFormula>._, A<ObjectPath>._, A<FormulaUsablePath>._, A<IBuildingBlock>._)).MustHaveHappened();
      }

      protected override bool CreatesCircularRef => false;
   }

   internal class When_setting_a_path_that_creates_a_circular_reference_but_the_presenter_is_forbidden_to_check : When_setting_a_path
   {
      protected override void Context()
      {
         base.Context();
         sut.CheckCircularReference = false;
      }

      protected override void Because()
      {
         sut.SetFormulaUsablePath(_aNewPath, _formulaUsablePathDTO);
      }

      [Observation]
      public void the_formula_task_sets_the_formula()
      {
         A.CallTo(() => _moBiFormulaTask.ChangePathInFormula(A<IFormula>._, A<ObjectPath>._, A<FormulaUsablePath>._, A<IBuildingBlock>._)).MustHaveHappened();
      }

      [Observation]
      public void the_circular_reference_checker_is_not_used_to_check_for_circular_references()
      {
         A.CallTo(() => _circularReferenceChecker.HasCircularReference(A<ObjectPath>._, _parameter)).MustNotHaveHappened();
      }

      protected override bool CreatesCircularRef => true;
   }

   internal class When_setting_a_path_that_creates_a_circular_reference : When_setting_a_path
   {
      [Observation]
      public void the_circular_reference_checker_is_used_to_check_for_circular_references()
      {
         The.Action(() => sut.SetFormulaUsablePath(_aNewPath, _formulaUsablePathDTO)).ShouldThrowAn<OSPSuiteException>();
         A.CallTo(() => _circularReferenceChecker.HasCircularReference(A<ObjectPath>._, _parameter)).MustHaveHappened();
         A.CallTo(() => _moBiFormulaTask.ChangePathInFormula(A<IFormula>._, A<ObjectPath>._, A<FormulaUsablePath>._, A<IBuildingBlock>._)).MustNotHaveHappened();
      }

      protected override bool CreatesCircularRef => true;
   }

   internal class When_adding_a_ReferencePath : concern_for_EditFormulaPathListPresenter
   {
      private ExplicitFormula _formula;
      private FormulaUsablePath _newFormulaPath;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         sut.Edit(_formula, null);
         _formula.AddObjectPath(new FormulaUsablePath());
         A.CallTo(() => _dimensionFactory.Dimension(_userSettings.ParameterDefaultDimension)).Returns(DomainHelperForSpecs.AmountDimension);
         A.CallTo(() => _moBiFormulaTask.AddFormulaUsablePath(_formula, A<FormulaUsablePath>._, A<IBuildingBlock>._))
            .Invokes(x => _newFormulaPath = x.GetArgument<FormulaUsablePath>(1));
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
      private FormulaUsablePath _formulaUsablePath;
      private FormulaUsablePathDTO _formulaUsablePathDTO;
      private ICommandCollector _commandCollector;
      private FormulaUsablePath _newFormulaPath;

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

         A.CallTo(() => _moBiFormulaTask.AddFormulaUsablePath(_formula, A<FormulaUsablePath>._, A<IBuildingBlock>._))
            .Invokes(x => _newFormulaPath = x.GetArgument<FormulaUsablePath>(1));
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
      private FormulaUsablePath _pathToRemove;
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