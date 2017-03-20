using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Core
{
   public abstract class concern_for_FormulaCachePresenterSpecs : ContextSpecification<IFormulaCachePresenter>
   {
      protected IFormulaCacheView _view;
      protected IFormulaToFormulaBuilderDTOMapper _formulaMapper;
      protected IFormulaPresenterCache _formulaPresenterCache;
      protected IMoBiContext _context;
      protected IViewItemContextMenuFactory _contexteMenuFactory;
      protected IDialogCreator _messagePresenter;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IFormulaUsageChecker _formulaChecker;

      protected override void Context()
      {
         base.Context();
         _view = A.Fake<IFormulaCacheView>();
         _formulaMapper = A.Fake<IFormulaToFormulaBuilderDTOMapper>();
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _context = A.Fake<IMoBiContext>();
         _contexteMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _messagePresenter = A.Fake<IDialogCreator>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _formulaChecker = A.Fake<IFormulaUsageChecker>();
          sut = new FormulaCachePresenter(_view, _formulaMapper, _formulaPresenterCache, _context, _contexteMenuFactory, _messagePresenter, _cloneManager, _formulaChecker);
      }
   }

   class When_removing_a_formula : concern_for_FormulaCachePresenterSpecs
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
         _dtoFormula = new FormulaBuilderDTO();
         _dtoFormula.Id = _formula.Id;
         A.CallTo(() => _formulaChecker.FormulaUsedIn(_buildingBlock,_formula)).Returns(false);
         A.CallTo(() => _messagePresenter.MessageBoxYesNo(AppConstants.Captions.ReallyDeleteFormula(_formula.Name)))
          .Returns(ViewResult.Yes);
         
      }

      protected override void Because()
      {
         sut.Remove(_dtoFormula);
      }

      [Observation]
      public void should_publish_a_formula_validat_event_for_removed_formula()
      {
         A.CallTo(() => _context.PublishEvent(A<FormulaValidEvent>._)).MustHaveHappened();
      }
   }
}	