using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
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
      private IMoBiFormulaTask _moBiFormulaTask;
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

      protected override void Context()
      {
         base.Context();
         _entity = new ParameterValue().WithName("parameter").WithFormula(new ExplicitFormula().WithName("explicit"));
         _buildingBlock = new ParameterValuesBuildingBlock { _entity };
         _clonedBuildingBlock = new ParameterValuesBuildingBlock
         {
            new ParameterValue().WithName("parameter").WithFormula(new ExplicitFormula().WithName("explicit"))
         };
         A.CallTo(() => _cloneManager.Clone(_buildingBlock)).Returns(_clonedBuildingBlock);
      }

      protected override void Because()
      {
         sut.Init(_entity, _buildingBlock, new UsingFormulaDecoder());
      }

      [Observation]
      public void the_clone_manager_should_clone_the_building_block()
      {
         A.CallTo(() => _cloneManager.Clone(_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_presenter_and_view_are_using_a_clone()
      {
         A.CallTo(() => _formulaToFormulaInfoDTOMapper.MapFrom(_clonedBuildingBlock.First().Formula)).MustHaveHappened();
      }
   }
}
