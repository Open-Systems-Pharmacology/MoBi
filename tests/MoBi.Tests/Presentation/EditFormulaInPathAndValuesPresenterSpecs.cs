using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation
{
   public class concern_for_EditFormulaInPathAndValuesPresenter : ContextSpecification<EditFormulaInPathAndValuesPresenter>
   {
      private IEditFormulaInPathAndValuesView _editFormulaInPathAndValues;
      private IFormulaPresenterCache _formulaPresenterCache;
      private IMoBiContext _moBiContext;
      protected IFormulaToFormulaInfoDTOMapper _formulaToFormulaInfoDTOMapper;
      private FormulaTypeCaptionRepository _formulaTypeCaptionRepository;
      protected IMoBiFormulaTask _moBiFormulaTask;
      private ICircularReferenceChecker _circularReferenceChecker;
      protected IInteractionTaskContext _interactionTaskContext;
      private ISelectReferenceAtParameterValuePresenter _selectReferenceAtParameterValuePresenter;
      protected ICloneManagerForBuildingBlock _cloneManager;

      protected override void Context()
      {
         _editFormulaInPathAndValues = A.Fake<IEditFormulaInPathAndValuesView>();
         _formulaPresenterCache = A.Fake<IFormulaPresenterCache>();
         _moBiContext = A.Fake<IMoBiContext>();
         _formulaToFormulaInfoDTOMapper = A.Fake<IFormulaToFormulaInfoDTOMapper>();
         _formulaTypeCaptionRepository = A.Fake<FormulaTypeCaptionRepository>();
         _moBiFormulaTask = A.Fake<IMoBiFormulaTask>();
         _circularReferenceChecker = A.Fake<ICircularReferenceChecker>();
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _selectReferenceAtParameterValuePresenter = A.Fake<ISelectReferenceAtParameterValuePresenter>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();

         sut = new EditFormulaInPathAndValuesPresenter(_editFormulaInPathAndValues,
            _formulaPresenterCache,
            _moBiContext,
            _formulaToFormulaInfoDTOMapper,
            _formulaTypeCaptionRepository,
            _moBiFormulaTask,
            _circularReferenceChecker,
            _selectReferenceAtParameterValuePresenter, _cloneManager);
      }
   }

   public class When_initializing_the_presenter : concern_for_EditFormulaInPathAndValuesPresenter
   {
      private ParameterValue _entity;
      private ParameterValuesBuildingBlock _buildingBlock;
      private ParameterValuesBuildingBlock _clonedBuildingBlock;
      private ParameterValue _pathAndValueEntity;
      private MockUsingFormulaDecoder _mockDecoder;
      private ParameterValue _formulaOwner;
      private IFormula _expectedOldFormula;

      protected override void Context()
      {
         base.Context();
         var formulaDecoder = new UsingFormulaDecoder();
         _entity = new ParameterValue { Path = new ObjectPath("Organism", "Liver", "volume") }.WithFormula(new ExplicitFormula().WithName("explicit"));
         _buildingBlock = new ParameterValuesBuildingBlock { _entity };
         _pathAndValueEntity = new ParameterValue { Path = new ObjectPath("Organism", "Liver", "volume") }.WithFormula(new ExplicitFormula().WithName("explicit"));
         var dummyWithTheSameName = new ParameterValue { Path = new ObjectPath("Organism", "Lung", "volume") }.WithFormula(new ExplicitFormula().WithName("explicit"));

         _clonedBuildingBlock = new ParameterValuesBuildingBlock
         {
            dummyWithTheSameName,
            _pathAndValueEntity
         };

         _mockDecoder = new MockUsingFormulaDecoder();
         A.CallTo(() => _cloneManager.Clone(_buildingBlock)).Returns(_clonedBuildingBlock);

         _formulaOwner = _clonedBuildingBlock.FindByPath(_entity.Path);
         _expectedOldFormula = formulaDecoder.GetFormula(_formulaOwner);
      }

      protected override void Because()
      {
         sut.Init(_entity, _buildingBlock, _mockDecoder);
      }

      [Observation]
      public void the_clone_manager_should_clone_the_building_block()
      {
         A.CallTo(() => _cloneManager.Clone(_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_update_formula_should_be_called_with_expected_formula()
      {
         A.CallTo(() => _moBiFormulaTask.UpdateFormula(_formulaOwner, _expectedOldFormula, A<IFormula>.Ignored, _mockDecoder, A<IBuildingBlock>.Ignored)).MustHaveHappened();
      }

      [Observation]
      public void the_presenter_and_view_are_using_a_clone()
      {
         A.CallTo(() => _formulaToFormulaInfoDTOMapper.MapFrom(_pathAndValueEntity.Formula)).MustHaveHappened();
      }

      public class MockUsingFormulaDecoder : UsingFormulaDecoder
      {
         public MockUsingFormulaDecoder()
         {
            GetFormula = _ => null;
         }
      }
   }
}