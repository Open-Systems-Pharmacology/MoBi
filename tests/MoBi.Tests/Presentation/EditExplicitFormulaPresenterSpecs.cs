using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Settings;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditExplicitFormulaPresenter : ContextSpecification<IEditExplicitFormulaPresenter>
   {
      protected IEditExplicitFormulaView _view;
      protected IExplicitFormulaToExplicitFormulaDTOMapper _explicitFormulaMapper;
      protected IActiveSubjectRetriever _activeSubjectRetriever;
      protected IMoBiContext _context;
      protected ICircularReferenceChecker _formulaChecker;
      protected IDialogCreator _dialogCreator;
      protected IInteractionTasksForChildren<IFormula, IFormulaUsablePath> _interactionTask;
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IMoBiFormulaTask _moBiFormulaTasks;
      private IDisplayUnitRetriever _displayUnitRetriever;
      protected IDimensionFactory _dimensionFactory;
      protected IUserSettings _userSettings;
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;

      protected override void Context()
      {
         _view = A.Fake<IEditExplicitFormulaView>();
         _explicitFormulaMapper = A.Fake<IExplicitFormulaToExplicitFormulaDTOMapper>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _context = A.Fake<IMoBiContext>();
         _formulaChecker = A.Fake<ICircularReferenceChecker>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _interactionTask = A.Fake<IInteractionTasksForChildren<IFormula, IFormulaUsablePath>>();
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _userSettings = A.Fake<IUserSettings>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _moBiFormulaTasks = new MoBiFormulaTask(_context, A.Fake<IMoBiApplicationController>(), A.Fake<IFormulaTask>(), A.Fake<INameCorrector>(), _dialogCreator);
         sut = new EditExplicitFormulaPresenter(_view, _explicitFormulaMapper, _activeSubjectRetriever, _context, _formulaChecker,
            _moBiFormulaTasks, _reactionDimensionRetriever, _displayUnitRetriever, _viewItemContextMenuFactory, _userSettings, _dimensionFactory);
      }
   }

   internal class When_adding_a_ReferencePath : concern_for_EditExplicitFormulaPresenter
   {
      private ExplicitFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         sut.Edit(_formula);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         _formula.AddObjectPath(new FormulaUsablePath());
         A.CallTo(() => _dimensionFactory.GetDimension(_userSettings.ParameterDefaultDimension)).Returns(HelperForSpecs.AmountDimension);
      }

      protected override void Because()
      {
         sut.CreateNewPath();
      }

      [Observation]
      public void a_new_empty_path_is_created_in_the_formula()
      {
         _formula.ObjectPaths.Count.ShouldBeEqualTo(2);
         string.IsNullOrEmpty(_formula.ObjectPaths[1].PathAsString).ShouldBeTrue();
      }

      [Observation]
      public void the_dimension_is_set_to_user_default_dimension()
      {
         _formula.ObjectPaths[1].Dimension.ShouldBeEqualTo(HelperForSpecs.AmountDimension);
      }

      [Observation]
      public void the_name_is_unique()
      {
         // started the test with an existing path with empty alias, the new one
         // will have the old name with a "1" appended
         _formula.ObjectPaths[1].Alias.ShouldBeEqualTo("1");
      }
   }

   internal class When_cloning_a_ReferencePath : concern_for_EditExplicitFormulaPresenter
   {
      private ExplicitFormulaBuilderDTO _dto;
      private ExplicitFormula _formula;
      private IFormulaUsablePath _formulaUsablePath;
      private FormulaUsablePathDTO _formulaUsablePathDTO;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _commandCollector = A.Fake<ICommandCollector>();
         _formula = new ExplicitFormula();
         _formulaUsablePath = new FormulaUsablePath("path") { Alias = "alias", Dimension = DimensionFactoryForSpecs.MassDimension };
         _formula.AddObjectPath(_formulaUsablePath);

         var formulaUsablePath = new FormulaUsablePath("path") {Alias = "alias"};
         _formulaUsablePathDTO = new FormulaUsablePathDTO(formulaUsablePath, _formula);
         _dto = new ExplicitFormulaBuilderDTO { ObjectPaths = new List<FormulaUsablePathDTO> { _formulaUsablePathDTO } };
         A.CallTo(() => _explicitFormulaMapper.MapFrom(_formula, A<IUsingFormula>._)).Returns(_dto);
         sut.Edit(_formula);
         sut.InitializeWith(_commandCollector);
      }

      protected override void Because()
      {
         sut.ClonePath(_dto.ObjectPaths[0]);
      }

      [Observation]
      public void the_newly_added_path_should_be_a_clone_of_the_original_with_unique_alias()
      {
         _formula.ObjectPaths[1].PathAsString.ShouldBeEqualTo(_formulaUsablePath.PathAsString);
         _formula.ObjectPaths[1].Dimension.ShouldBeEqualTo(_formulaUsablePath.Dimension);
         _formula.ObjectPaths[1].Alias.ShouldNotBeEqualTo(_formulaUsablePath.Alias);
      }
   }

   internal class When_removing_a_ReferencePath : concern_for_EditExplicitFormulaPresenter
   {
      private FormulaUsablePathDTO _pathDTO;
      private ExplicitFormula _formula;
      private IFormulaUsablePath _pathToRemove;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula().WithName("Test");
         _pathToRemove = new FormulaUsablePath("..", "ToRemove") { Alias = "ToRemove" };
         _pathDTO = new FormulaUsablePathDTO(_pathToRemove, _formula);
         _formula.AddObjectPath(_pathToRemove);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         sut.Edit(_formula);
      }

      protected override void Because()
      {
         sut.RemovePath(_pathDTO);
      }

      [Observation]
      public void should_ask_for_remove_command()
      {
         _interactionTask.Remove(_pathToRemove, _formula, A.Fake<IBuildingBlock>());
      }

      [Observation]
      public void should_add_the_command_to_the_history()
      {
         A.CallTo(() => sut.CommandCollector.AddCommand(A<RemoveFormulaUsablePathCommand>._)).MustHaveHappened();
      }
   }

   public class When_editing_a_parameter_RHS : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _parameter;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _parameter = A.Fake<IParameter>().WithName("PARA");
         sut.IsRHS = true;
      }

      protected override void Because()
      {
         sut.Edit(_formula, _parameter);
      }

      [Observation]
      public void should_set_the_formulation_caption_using_the_parameter_name()
      {
         A.CallTo(() => _view.SetFormulaCaption(AppConstants.Captions.ParameterRightHandSide("PARA"))).MustHaveHappened();
      }
   }

   public class When_editing_a_parameter_formula : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _parameter;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _parameter = A.Fake<IParameter>().WithName("PARA");
      }

      protected override void Because()
      {
         sut.Edit(_formula, _parameter);
      }

      [Observation]
      public void should_hide_the_formula_caption()
      {
         A.CallTo(() => _view.HideFormulaCaption());
      }
   }

   public class When_editing_a_transport_formula : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _parameter;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _parameter = A.Fake<ITransportBuilder>();
      }

      protected override void Because()
      {
         sut.Edit(_formula, _parameter);
      }

      [Observation]
      public void should_show_the_amount_formula_caption()
      {
         A.CallTo(() => _view.SetFormulaCaption(AppConstants.Captions.AmountRightHandSide)).MustHaveHappened();
      }
   }

   public class When_editing_a_reaction_formula_in_amount_based_mode : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _parameter;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _parameter = A.Fake<IReactionBuilder>();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
      }

      protected override void Because()
      {
         sut.Edit(_formula, _parameter);
      }

      [Observation]
      public void should_show_the_forula_caption_depepding_on_the_project_reaction_dimension_mode()
      {
         A.CallTo(() => _view.SetFormulaCaption(AppConstants.Captions.AmountRightHandSide)).MustHaveHappened();
      }
   }

   public class When_editing_a_reaction_formula_in_concentration_based_mode : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _parameter;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _parameter = A.Fake<IReactionBuilder>();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
      }

      protected override void Because()
      {
         sut.Edit(_formula, _parameter);
      }

      [Observation]
      public void should_show_the_forula_caption_depepding_on_the_project_reaction_dimension_mode()
      {
         A.CallTo(() => _view.SetFormulaCaption(AppConstants.Captions.ConcentrationRightHandSide)).MustHaveHappened();
      }
   }

}