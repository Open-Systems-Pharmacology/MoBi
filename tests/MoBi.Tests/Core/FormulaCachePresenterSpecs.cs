using System.Collections.Generic;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_FormulaCachePresenter : ContextSpecification<FormulaCachePresenter>
   {
      protected IFormulaCacheView _view;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IFormulaPresenterCache _formulaPresenterCache;
      protected IMoBiContext _context;
      protected IViewItemContextMenuFactory _contextMenuFactory;
      protected IDialogCreator _dialogCreator;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IFormulaUsageChecker _formulaChecker;
      protected IObjectBaseNamingTask _namingTask;
      protected IClipboardManager _clipBoardManager;

      protected override void Context()
      {
         base.Context();
         _view = A.Fake<IFormulaCacheView>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _context = A.Fake<IMoBiContext>();
         _contextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _formulaChecker = A.Fake<IFormulaUsageChecker>();
         _namingTask = A.Fake<IObjectBaseNamingTask>();
         _clipBoardManager = new ClipboardManager(_cloneManager);
         sut = new FormulaCachePresenter(_view, _formulaMapper, _formulaPresenterCache, _context, _contextMenuFactory, _dialogCreator, _cloneManager, _formulaChecker, _namingTask, _clipBoardManager);
      }
   }

   public class When_removing_a_formula_that_cannot_be_removed : concern_for_FormulaCachePresenter
   {
      private FormulaBuilderDTO _dtoFormula;
      private IBuildingBlock _buildingBlock;
      private IFormula _formula;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         A.CallTo(() => _formulaChecker.FormulaUsedIn(_buildingBlock, _formula)).Returns(true);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(_formula.Name), ViewResult.Yes))
            .Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Remove(_dtoFormula);
      }

      [Observation]
      public void the_formula_should_remain_in_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_formula);
      }

      [Observation]
      public void should_inform_the_user_that_the_formula_is_in_use()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Exceptions.FormulaInUse(_formula))).MustHaveHappened();
      }
   }

   public class When_removing_a_formula_but_the_user_does_not_confirm : concern_for_FormulaCachePresenter
   {
      private FormulaBuilderDTO _dtoFormula;
      private IBuildingBlock _buildingBlock;
      private IFormula _formula;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         A.CallTo(() => _formulaChecker.FormulaUsedIn(_buildingBlock, _formula)).Returns(false);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(_formula.Name), ViewResult.Yes))
            .Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.Remove(_dtoFormula);
      }

      [Observation]
      public void the_formula_should_remain_in_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_formula);
      }

      [Observation]
      public void should_not_publish_a_formula_validate_event_for_removed_formula()
      {
         A.CallTo(() => _context.PublishEvent(A<FormulaValidEvent>._)).MustNotHaveHappened();
      }
   }

   public class When_removing_a_formula_that_can_be_removed : concern_for_FormulaCachePresenter
   {
      private FormulaBuilderDTO _dtoFormula;
      private IBuildingBlock _buildingBlock;
      private IFormula _formula;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         A.CallTo(() => _formulaChecker.FormulaUsedIn(_buildingBlock, _formula)).Returns(false);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(_formula.Name), ViewResult.Yes))
            .Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Remove(_dtoFormula);
      }

      [Observation]
      public void should_publish_a_formula_validate_event_for_removed_formula()
      {
         A.CallTo(() => _context.PublishEvent(A<FormulaValidEvent>._)).MustHaveHappened();
      }

      [Observation]
      public void the_formula_should_not_remain_in_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldNotContain(_formula);
      }
   }

   public class When_cloning_a_formula : concern_for_FormulaCachePresenter
   {
      private FormulaBuilderDTO _dtoFormula;
      private IBuildingBlock _buildingBlock;
      private IFormula _formula;
      private ICommandCollector _commandCollector;
      private IFormula _clonedFormula;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _clonedFormula = new ExplicitFormula().WithName("Formula").WithId("F2");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         A.CallTo(() => _cloneManager.Clone(_formula, A<IFormulaCache>._)).Returns(_clonedFormula);
         A.CallTo(() => _namingTask.NewName(AppConstants.Captions.NewName, AppConstants.Captions.CloneFormulaTitle, _formula.Name, A<IEnumerable<string>>._, null, null)).Returns("NewName");
      }

      protected override void Because()
      {
         sut.Clone(_dtoFormula);
      }

      [Observation]
      public void the_clone_should_be_renamed()
      {
         _clonedFormula.Name.ShouldBeEqualTo("NewName");
      }

      [Observation]
      public void the_clone_should_be_added_to_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_clonedFormula);
      }
   }

   public class When_cloning_a_formula_and_rename_is_canceled : concern_for_FormulaCachePresenter
   {
      private FormulaBuilderDTO _dtoFormula;
      private IBuildingBlock _buildingBlock;
      private IFormula _formula;
      private ICommandCollector _commandCollector;
      private IFormula _clonedFormula;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _clonedFormula = new ExplicitFormula().WithName("Formula").WithId("F2");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         A.CallTo(() => _cloneManager.Clone(_formula, A<IFormulaCache>._)).Returns(_clonedFormula);
         A.CallTo(() => _namingTask.NewName(AppConstants.Captions.NewName, AppConstants.Captions.CloneFormulaTitle, _formula.Name, A<IEnumerable<string>>._, null, null)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Clone(_dtoFormula);
      }

      [Observation]
      public void the_clone_should_not_be_created()
      {
         A.CallTo(() => _cloneManager.Clone(_formula, A<IFormulaCache>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_clone_should_not_be_added_to_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldOnlyContain(_formula);
      }
   }

   public class When_copying_a_formula : concern_for_FormulaCachePresenter
   {
      private FormulaBuilderDTO _dtoFormula;
      private IBuildingBlock _buildingBlock;
      private IFormula _formula;
      private ICommandCollector _commandCollector;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         A.CallTo(() => _formulaChecker.FormulaUsedIn(_buildingBlock, _formula)).Returns(false);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(_formula.Name), ViewResult.Yes))
            .Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Copy(_dtoFormula);
      }

      [Observation]
      public void the_clipboard_manager_is_used_to_track_the_copy()
      {
         _clipBoardManager.ClipBoardContainsType<IFormula>().ShouldBeTrue();
      }
   }

   public abstract class When_pasting_a_formula : concern_for_FormulaCachePresenter
   {
      protected FormulaBuilderDTO _dtoFormula;
      protected IBuildingBlock _buildingBlock;
      protected IFormula _formula;
      protected ICommandCollector _commandCollector;
      protected IFormula _clonedFormula;
      protected FormulaBuilderDTO _clonedFormulaDTO;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoBiSpatialStructure();
         _formula = new ExplicitFormula().WithName("Formula").WithId("F1");
         _clonedFormula = new ExplicitFormula().WithName("Formula").WithId("F2");
         _buildingBlock.AddFormula(_formula);
         _commandCollector = A.Fake<ICommandCollector>();
         sut.InitializeWith(_commandCollector);
         sut.Edit(_buildingBlock);
         _dtoFormula = new FormulaBuilderDTO(_formula);
         _clonedFormulaDTO = new FormulaBuilderDTO(_clonedFormula);

         A.CallTo(() => _formulaMapper.MapFrom(_clonedFormula)).Returns(_clonedFormulaDTO);
         A.CallTo(() => _formulaChecker.FormulaUsedIn(_buildingBlock, _formula)).Returns(false);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(_formula.Name), ViewResult.Yes))
            .Returns(ViewResult.Yes);
         A.CallTo(() => _cloneManager.Clone(_formula)).Returns(_clonedFormula);
         ConfigureRenaming();

         sut.Copy(_dtoFormula);
      }

      protected abstract void ConfigureRenaming();

      protected override void Because()
      {
         sut.Paste(null);
      }
   }

   public class When_pasting_a_formula_with_naming_collision_and_user_cancels : When_pasting_a_formula
   {
      protected override void ConfigureRenaming()
      {
         A.CallTo(() => _namingTask.RenameFor(_clonedFormula, A<IReadOnlyList<string>>._)).Returns(string.Empty);
      }

      [Observation]
      public void the_pasted_formula_is_not_added_to_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_formula);
      }
   }

   public class When_pasting_a_formula_with_naming_collision : When_pasting_a_formula
   {
      protected override void ConfigureRenaming()
      {
         A.CallTo(() => _namingTask.RenameFor(_clonedFormula, A<IReadOnlyList<string>>._)).Returns("NewName");
      }

      [Observation]
      public void the_formula_should_be_renamed()
      {
         _clonedFormula.Name.ShouldBeEqualTo("NewName");
      }

      [Observation]
      public void the_pasted_formula_is_added_to_the_cache()
      {
         _buildingBlock.FormulaCache.ShouldContain(_clonedFormula);
      }

      [Observation]
      public void the_view_should_be_updated_to_focus_the_new_formula()
      {
         A.CallTo(() => _view.Select(_clonedFormulaDTO)).MustHaveHappened();
      }
   }
}