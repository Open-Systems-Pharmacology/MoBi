using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

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
      protected IInteractionTasksForChildren<IFormula, FormulaUsablePath> _interactionTask;
      protected IReactionDimensionRetriever _reactionDimensionRetriever;
      protected IMoBiFormulaTask _moBiFormulaTask;
      private IDisplayUnitRetriever _displayUnitRetriever;
      protected IDimensionFactory _dimensionFactory;
      protected IUserSettings _userSettings;
      private IEditFormulaPathListPresenter _editFormulaPathListPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditExplicitFormulaView>();
         _explicitFormulaMapper = A.Fake<IExplicitFormulaToExplicitFormulaDTOMapper>();
         _activeSubjectRetriever = A.Fake<IActiveSubjectRetriever>();
         _context = A.Fake<IMoBiContext>();
         _formulaChecker = A.Fake<ICircularReferenceChecker>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _interactionTask = A.Fake<IInteractionTasksForChildren<IFormula, FormulaUsablePath>>();
         _reactionDimensionRetriever = A.Fake<IReactionDimensionRetriever>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _userSettings = A.Fake<IUserSettings>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _editFormulaPathListPresenter = A.Fake<IEditFormulaPathListPresenter>();
         sut = new EditExplicitFormulaPresenter(
            _view, _explicitFormulaMapper, _moBiFormulaTask, _reactionDimensionRetriever, _displayUnitRetriever, _editFormulaPathListPresenter);
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