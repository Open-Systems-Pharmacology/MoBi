using System.Collections.Generic;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
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
      protected IMoBiFormulaTask _moBiFormulaTask;
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
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         sut = new EditExplicitFormulaPresenter(_view, _explicitFormulaMapper, _activeSubjectRetriever, _context, _formulaChecker,
            _moBiFormulaTask, _reactionDimensionRetriever, _displayUnitRetriever, _viewItemContextMenuFactory, _userSettings, _dimensionFactory);
      }
   }

   internal class When_adding_a_ReferencePath : concern_for_EditExplicitFormulaPresenter
   {
      private ExplicitFormula _formula;
      private IFormulaUsablePath _newFormulaPath;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula();
         sut.Edit(_formula);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         _formula.AddObjectPath(new FormulaUsablePath());
         A.CallTo(() => _dimensionFactory.Dimension(_userSettings.ParameterDefaultDimension)).Returns(DomainHelperForSpecs.AmountDimension);
         A.CallTo(() => _moBiFormulaTask.AddFormulaUsablePath(_formula, A<IFormulaUsablePath>._, A<IBuildingBlock>._))
            .Invokes(x=>_newFormulaPath = x.GetArgument<IFormulaUsablePath>(1));
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

   internal class When_cloning_a_ReferencePath : concern_for_EditExplicitFormulaPresenter
   {
      private ExplicitFormulaBuilderDTO _dto;
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
         _dto = new ExplicitFormulaBuilderDTO {ObjectPaths = new List<FormulaUsablePathDTO> {_formulaUsablePathDTO}};
         A.CallTo(() => _explicitFormulaMapper.MapFrom(_formula, A<IUsingFormula>._)).Returns(_dto);
         sut.Edit(_formula);
         sut.InitializeWith(_commandCollector);

         A.CallTo(() => _moBiFormulaTask.AddFormulaUsablePath(_formula, A<IFormulaUsablePath>._, A<IBuildingBlock>._))
            .Invokes(x => _newFormulaPath = x.GetArgument<IFormulaUsablePath>(1));

      }

      protected override void Because()
      {
         sut.ClonePath(_dto.ObjectPaths[0]);
      }

      [Observation]
      public void the_newly_added_path_should_be_a_clone_of_the_original_with_unique_alias()
      {
         _newFormulaPath.PathAsString.ShouldBeEqualTo(_formulaUsablePath.PathAsString);
         _newFormulaPath.Dimension.ShouldBeEqualTo(_formulaUsablePath.Dimension);
         _newFormulaPath.Alias.ShouldNotBeEqualTo(_formulaUsablePath.Alias);
      }
   }

   internal class When_removing_a_ReferencePath : concern_for_EditExplicitFormulaPresenter
   {
      private FormulaUsablePathDTO _pathDTO;
      private ExplicitFormula _formula;
      private IFormulaUsablePath _pathToRemove;
      private IMoBiCommand _removeCommand;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula().WithName("Test");
         _pathToRemove = new FormulaUsablePath("..", "ToRemove") {Alias = "ToRemove"};
         _pathDTO = new FormulaUsablePathDTO(_pathToRemove, _formula);
         _formula.AddObjectPath(_pathToRemove);
         sut.InitializeWith(A.Fake<ICommandCollector>());
         sut.Edit(_formula);

         _removeCommand= A.Fake<IMoBiCommand>();
         A.CallTo(() => _moBiFormulaTask.RemoveFormulaUsablePath(_formula, _pathToRemove, A<IBuildingBlock>._)).Returns(_removeCommand);
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
         A.CallTo(() => sut.CommandCollector.AddCommand(_removeCommand)).MustHaveHappened();
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
         A.CallTo(() => _moBiFormulaTask.GetFormulaCaption(_parameter, A<ReactionDimensionMode>._, true)).Returns("THE_RHS_CAPTION");
      }

      protected override void Because()
      {
         sut.Edit(_formula, _parameter);
      }

      [Observation]
      public void should_set_the_formulation_caption_using_the_parameter_name()
      {
         A.CallTo(() => _view.SetFormulaCaption("THE_RHS_CAPTION")).MustHaveHappened();
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
      private IUsingFormula _transportBuilder;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _transportBuilder = A.Fake<ITransportBuilder>();
         A.CallTo(() => _moBiFormulaTask.GetFormulaCaption(_transportBuilder, A<ReactionDimensionMode>._, false)).Returns("THE_RHS_CAPTION");
      }

      protected override void Because()
      {
         sut.Edit(_formula, _transportBuilder);
      }

      [Observation]
      public void should_show_the_amount_formula_caption()
      {
         A.CallTo(() => _view.SetFormulaCaption("THE_RHS_CAPTION")).MustHaveHappened();
      }
   }

   public class When_editing_a_reaction_formula_in_amount_based_mode : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _reactionBuilder;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _reactionBuilder = A.Fake<IReactionBuilder>();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
         A.CallTo(() => _moBiFormulaTask.GetFormulaCaption(_reactionBuilder, ReactionDimensionMode.AmountBased, false)).Returns("THE_RHS_CAPTION");
      }

      protected override void Because()
      {
         sut.Edit(_formula, _reactionBuilder);
      }

      [Observation]
      public void should_show_the_forula_caption_depepding_on_the_project_reaction_dimension_mode()
      {
         A.CallTo(() => _view.SetFormulaCaption("THE_RHS_CAPTION")).MustHaveHappened();
      }
   }

   public class When_editing_a_reaction_formula_in_concentration_based_mode : concern_for_EditExplicitFormulaPresenter
   {
      private IFormula _formula;
      private IUsingFormula _reactionBuilder;

      protected override void Context()
      {
         base.Context();
         _formula = new ExplicitFormula("TOTO");
         _reactionBuilder = A.Fake<IReactionBuilder>();
         A.CallTo(() => _reactionDimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
         A.CallTo(() => _moBiFormulaTask.GetFormulaCaption(_reactionBuilder, ReactionDimensionMode.ConcentrationBased, false)).Returns("THE_RHS_CAPTION");
      }

      protected override void Because()
      {
         sut.Edit(_formula, _reactionBuilder);
      }

      [Observation]
      public void should_show_the_forula_caption_depepding_on_the_project_reaction_dimension_mode()
      {
         A.CallTo(() => _view.SetFormulaCaption("THE_RHS_CAPTION")).MustHaveHappened();
      }
   }
}