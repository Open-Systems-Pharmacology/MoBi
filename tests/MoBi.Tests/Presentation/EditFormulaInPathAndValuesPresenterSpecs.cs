using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
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
      protected ParameterValue _entity;
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected ParameterValue _pathAndValueEntity;

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
         _entity = new ParameterValue { Path = new ObjectPath("Organism", "Liver", "volume") }.WithFormula(new ExplicitFormula().WithName("explicit"));
         _buildingBlock = new ParameterValuesBuildingBlock { _entity };
         _pathAndValueEntity = new ParameterValue { Path = new ObjectPath("Organism", "Liver", "volume") }.WithFormula(new ExplicitFormula().WithName("explicit"));
         var dummyWithTheSameName = new ParameterValue { Path = new ObjectPath("Organism", "Lung", "volume") }.WithFormula(new ExplicitFormula().WithName("explicit"));

         sut = new EditFormulaInPathAndValuesPresenter(_editFormulaInPathAndValues,
            _formulaPresenterCache,
            _moBiContext,
            _formulaToFormulaInfoDTOMapper,
            _formulaTypeCaptionRepository,
            _moBiFormulaTask,
            _circularReferenceChecker,
            _selectReferenceAtParameterValuePresenter);
         
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   public class  When_updating_formula : concern_for_EditFormulaInPathAndValuesPresenter
   {
      private IFormula _expectedOldFormula;
      private UsingFormulaDecoder _formulaDecoder;

      protected override void Context()
      {
         base.Context();
         _formulaDecoder = new UsingFormulaDecoder();
         _expectedOldFormula = _entity.Formula;
         sut.Init(_entity, _buildingBlock, _formulaDecoder);
      }

      protected override void Because()
      {
         sut.AddNewFormula("newformula");
      }

      [Observation]
      public void the_update_formula_should_be_called_with_expected_formula()
      {
         A.CallTo(() => _moBiFormulaTask.UpdateFormula(_entity, _expectedOldFormula, A<IFormula>.Ignored, _formulaDecoder, A<IBuildingBlock>.Ignored)).MustHaveHappened();
      }
   }

   public class When_handling_rename_events_and_formula_is_null : concern_for_EditFormulaInPathAndValuesPresenter
   {
      protected override void Context()
      {
         base.Context();
         _pathAndValueEntity.Formula = null;
         sut.Init(_entity, _buildingBlock, new UsingFormulaDecoder());
      }

      protected override void Because()
      {
         sut.Handle(new ObjectPropertyChangedEvent(_pathAndValueEntity));
      }

      [Observation]
      public void the_formula_mapper_is_not_used()
      {
         A.CallTo(() => _formulaToFormulaInfoDTOMapper.MapFrom(A<IFormula>._)).MustHaveHappenedOnceExactly();
      }
   }
}